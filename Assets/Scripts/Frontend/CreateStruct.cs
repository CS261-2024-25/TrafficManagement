using UnityEngine;
using Assets.Scripts.Util;
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
        public TMP_InputField priorityText;
        public CardinalDirection direction; // Manually set in unity

        // Run when submit is clicked on the second page
        public void GetInputs(){
                int parsedVal = 0;
                if (!Int32.TryParse(direction1Text.text, out parsedVal)){
                        throw new IOException("Could not parse direction 1 to an integer");
                }
                uint direction1Flow = Convert.ToUInt32(parsedVal);

                if (!Int32.TryParse(direction2Text.text, out parsedVal)){
                        throw new IOException("Could not parse direction 2 to an integer");
                }
                uint direction2Flow = Convert.ToUInt32(parsedVal);

                if (!Int32.TryParse(direction3Text.text, out parsedVal)){
                        throw new IOException("Could not parse direction 3 to an integer");
                }
                uint direction3Flow = Convert.ToUInt32(parsedVal);

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
                           break;
                                
                }
                // If value is 0, then no priority was selected

                double prioNum = Convert.ToDouble(priorityText.text);
                
                StaticData.priority[StaticData.arrIndex] = (direction,prioNum); 
                StaticData.arrIndex++;
                // West Direction is run last so when west runs, switch scene and create struct
                if (direction == CardinalDirection.West){
                    SceneManager.LoadScene("ResultsScreen");
                }
        }
}
