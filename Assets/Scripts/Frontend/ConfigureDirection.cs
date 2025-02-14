using UnityEngine;
using Assets.Scripts.Util;
using TMPro;
using UnityEngine.UI; // For toggles
using System;
using System.IO; // To parse strings

public class ConfigureDirection : MonoBehaviour
{
        // Values to be put into struct
        private uint LeftFlow;
        private uint ForwardFlow;
        private uint RightFlow;
        private uint LaneCountOutbound;
        private uint LaneCountInbound;
        private bool HasLeftTurn;
        private bool HasPedestrianCrossing;

        public TMP_InputField outboundText;
        public TMP_InputField inboundText;
        public Toggle leftToggle;
        public Toggle crossingToggle;
        

        public void GetInputs(){
                int parsedVal = 0;
                if (!Int32.TryParse(outboundText.text, out parsedVal)){
                        throw new IOException("Could not parse outbound lane number to an integer");
                }
                LaneCountOutbound = Convert.ToUInt32(parsedVal);

                if (!Int32.TryParse(inboundText.text, out parsedVal)){
                        throw new IOException("Could not parse inbound lane number to an integer");
                }
                LaneCountInbound = Convert.ToUInt32(parsedVal);
                HasLeftTurn = leftToggle.isOn;
                HasPedestrianCrossing = crossingToggle.isOn;
        }

}
