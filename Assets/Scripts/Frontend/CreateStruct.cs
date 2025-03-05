using UnityEngine;
using Assets.Scripts.Util;
using Assets.Scripts.Backend.Simulation;
using Assets.Scripts.Backend.Engine;
using Assets.Scripts.Backend.PersistentJunctionSave;
using TMPro;
using System;
using System.IO;
using UnityEngine.SceneManagement;
using System.Diagnostics; // To parse strings

public class CreateStruct : MonoBehaviour
{
        private uint LeftFlow;
        private uint ForwardFlow;
        private uint RightFlow;

        // Directions are not specified as the function in this code is run separately for each direction
        public TMP_InputField direction1Text;
        public TMP_InputField direction2Text;
        public TMP_InputField direction3Text;
        public TMP_InputField TotalFlowText;
        public TMP_InputField priorityText;

        public GameObject errorPanel;

        public CardinalDirection direction; // Manually set in unity

        // Run when submit is clicked on the second page
        public void GetInputs(){
                CardinalDirection terminalDirection = CardinalDirection.West;
                int parsedVal1 = 0;
                int parsedVal2 = 0;
                int parsedVal3 = 0;
                int parsedVal4 = 0;
                if (!Int32.TryParse(direction1Text.text, out parsedVal1)){ // If runs when number cannot be parsed
                        StaticData.failFlowParse = true;
                }
                if (!Int32.TryParse(direction2Text.text, out parsedVal2)){
                        StaticData.failFlowParse = true;
                }
                if (!Int32.TryParse(direction3Text.text, out parsedVal3)){
                        StaticData.failFlowParse = true;
                }
                if (!Int32.TryParse(TotalFlowText.text, out parsedVal4)){
                        StaticData.failFlowParse = true;
                }
                
                double prioNum=0;
                if (double.TryParse(priorityText.text,out prioNum)){
                        StaticData.totPrio+=prioNum;
                }
                else{
                        StaticData.failFlowParse = true;
                }
                // Input sanitisation checks
                if (
                        StaticData.failFlowParse || 
                        prioNum < 0 || 
                        parsedVal1 < 0 || 
                        parsedVal2 < 0 || 
                        parsedVal3 < 0 ||
                        parsedVal1 + parsedVal2 + parsedVal3 != parsedVal4 ||
                        (direction == terminalDirection && StaticData.totPrio != 4)
                ){
                        StaticData.failFlowParse = true; // Needs to be set incase loop is entered through invalid priority
                        StaticData.arrIndex = 0;
                        if (direction == terminalDirection){
                                StaticData.totPrio = 0;
                                StaticData.failFlowParse = false;
                        }
                        errorPanel.SetActive(true);

                }
                else{
                        StaticData.priority[StaticData.arrIndex] = (direction,prioNum); 
                        StaticData.arrIndex++;

                        uint direction1Flow = Convert.ToUInt32(parsedVal1); // Needed to fit struct type
                        uint direction2Flow = Convert.ToUInt32(parsedVal2);
                        uint direction3Flow = Convert.ToUInt32(parsedVal3);

                        switch(direction){
                                case CardinalDirection.North:
                                StaticData.northbound = new DirectionDetails( 
                                        direction2Flow, // Exiting East
                                        direction1Flow, // Exiting North
                                        direction3Flow, // Exiting West
                                        StaticData.northbound.LaneCountOutbound,
                                        StaticData.northbound.LaneCountInbound,
                                        StaticData.northbound.HasLeftTurn,
                                        StaticData.northbound.HasPedestrianCrossing
                                );
                                break;
                                case CardinalDirection.East:
                                StaticData.eastbound = new DirectionDetails(
                                        direction2Flow, // Exiting South
                                        direction3Flow, // Exiting East
                                        direction1Flow, // Exiting North
                                        StaticData.eastbound.LaneCountOutbound,
                                        StaticData.eastbound.LaneCountInbound,
                                        StaticData.eastbound.HasLeftTurn,
                                        StaticData.eastbound.HasPedestrianCrossing
                                );
                                break;
                                case CardinalDirection.South:
                                StaticData.southbound = new DirectionDetails(
                                        direction3Flow, // Exiting West
                                        direction1Flow, // Exiting South
                                        direction2Flow, // Exiting East
                                        StaticData.southbound.LaneCountOutbound,
                                        StaticData.southbound.LaneCountInbound,
                                        StaticData.southbound.HasLeftTurn,
                                        StaticData.southbound.HasPedestrianCrossing
                                );
                                break;
                                case CardinalDirection.West:
                                StaticData.westbound = new DirectionDetails(
                                        direction1Flow, // Exiting North
                                        direction3Flow, // Exiting West
                                        direction2Flow, // Exiting South
                                        StaticData.westbound.LaneCountOutbound,
                                        StaticData.westbound.LaneCountInbound,
                                        StaticData.westbound.HasLeftTurn,
                                        StaticData.westbound.HasPedestrianCrossing
                                );
                                        StaticData.totPrio = 0;
                                        StaticData.arrIndex = 0;
                                        InputParameters toBackend = new InputParameters(StaticData.northbound,StaticData.eastbound, StaticData.southbound, StaticData.westbound, StaticData.priority);
                                        Simulation simulation = new Simulation(new Engine(1000000),toBackend,1000);
                                        ResultTrafficSimulation results = simulation.RunSimulation();
                                        PersistentJunctionSave.SaveResult(toBackend,results);       
                                        SceneManager.LoadScene("ResultsScreen"); // West Direction is run last so when west runs, switch scene
                                break;
                                                
                                }
                }
        }
}
