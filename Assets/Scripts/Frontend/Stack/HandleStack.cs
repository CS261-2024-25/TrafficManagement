using UnityEngine;
using System;
using System.Collections.Generic;
using Assets.Scripts.Frontend.Stack;
using TMPro;
using UnityEngine.UI;
using System.Linq;

namespace Assets.Scripts.Frontend.Stack{
    public class HandleStack : MonoBehaviour
    {
        private Stack<ToggleOrTextbox> pastActions;
        private Dictionary<TMP_InputField,string> prevText;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            pastActions = new Stack<ToggleOrTextbox>();
            prevText = new Dictionary<TMP_InputField,string>();
        }

        // Called when a textbox is deselected and adds change to stack of changes
        public void TextBoxUpdate(TMP_InputField textBox){
            ToggleOrTextbox toAdd = new ToggleOrTextbox(textBox,null,prevText[textBox],false); // There will always be a prevText value as you have to select a textbox before you deselect
            pastActions.Push(toAdd);
            prevText[textBox] = textBox.text; // prevText set here as well to account for if user resets
        }

        // Called when a toggle is changed and adds change to stack of changes
        public void ToggleUpdate(Toggle toggle){
            ToggleOrTextbox toAdd = new ToggleOrTextbox(null,toggle,"",!toggle.isOn); // Toggle values is always switched when changed  so prev value will be negation
            pastActions.Push(toAdd);
        }

        // Called when textbox is selected to get the previous value
        public void TextBoxSelect(TMP_InputField textBox){
            prevText[textBox] = textBox.text;
        }

        // Called when undo button is clicked
        public void Undo(){
            ToggleOrTextbox top;
            bool hasObject = pastActions.TryPop(out top);
            if (hasObject){
                if (top.textBox){
                    top.textBox.text = top.text;
                }
                else{
                    top.toggle.isOn = top.on;
                    pastActions.Pop(); // Need to pop again as the chnage from the undo is added to the stack as the event listener is on toggle value change
                }
            }
        }
    }
}
