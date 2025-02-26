using UnityEngine;
using Assets.Scripts.Util;
using TMPro;
using UnityEngine.UI; // For toggles
using System;
using System.IO; // To parse strings
using UnityEngine.SceneManagement;

public class ConfigureDirection : MonoBehaviour
{
        // Values to be put into struct
        private uint LaneCountOutbound;
        private uint LaneCountInbound;
        private bool HasLeftTurn;
        private bool HasPedestrianCrossing;

        // Values from input fields
        public TMP_InputField outboundText;
        public TMP_InputField inboundText;
        public Toggle leftToggle;
        public Toggle crossingToggle;

        // Values for the error panel
        public GameObject errorPanel;

        public CardinalDirection direction; // Manually set in unity
        
        // Run when submit is clicken on the first page
        public void GetInputs(){
                int parsedVal = 0;
                int parsedVal2 = 0;
                if (!Int32.TryParse(outboundText.text, out parsedVal)){
                        StaticData.failDirectionParse = true;
                }
                LaneCountOutbound = Convert.ToUInt32(parsedVal);

                if (!Int32.TryParse(inboundText.text, out parsedVal2)){
                        StaticData.failDirectionParse = true;
                }
                if (StaticData.failDirectionParse){
                        if (direction == CardinalDirection.West){
                                StaticData.failDirectionParse = false;
                        }
                        errorPanel.SetActive(true);
                }
                else{
                        LaneCountOutbound = Convert.ToUInt32(parsedVal);
                        LaneCountInbound = Convert.ToUInt32(parsedVal2);
                        if (LaneCountInbound + LaneCountOutbound != 5){
                                StaticData.failDirectionParse = true;
                                errorPanel.SetActive(true);
                        }
                        else{
                                HasLeftTurn = leftToggle.isOn;
                                HasPedestrianCrossing = crossingToggle.isOn;

                                switch(direction){  // Traffic flows are set to 0 as they will be set later in CreateStruct
                                        case CardinalDirection.North:
                                        StaticData.northbound = new DirectionDetails(0,0,0,LaneCountOutbound,LaneCountInbound,HasLeftTurn,HasPedestrianCrossing);
                                        break;
                                        case CardinalDirection.East:
                                        StaticData.eastbound = new DirectionDetails(0,0,0,LaneCountOutbound,LaneCountInbound,HasLeftTurn,HasPedestrianCrossing);
                                        break;
                                        case CardinalDirection.South:
                                        StaticData.southbound = new DirectionDetails(0,0,0,LaneCountOutbound,LaneCountInbound,HasLeftTurn,HasPedestrianCrossing);
                                        break;
                                        case CardinalDirection.West:
                                        StaticData.westbound = new DirectionDetails(0,0,0,LaneCountOutbound,LaneCountInbound,HasLeftTurn,HasPedestrianCrossing);
                                        SceneManager.LoadScene("TrafficFlowSelect"); // West run last so that switches scene
                                        break;      
                                }
                        }
                }
        }

}
