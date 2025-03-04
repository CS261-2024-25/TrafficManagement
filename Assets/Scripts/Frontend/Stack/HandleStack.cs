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
        private Stack<ToggleOrTextbox> redoStack;
        private Dictionary<TMP_InputField,string> prevText;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            pastActions = new Stack<ToggleOrTextbox>();
            redoStack = new Stack<ToggleOrTextbox>();
            prevText = new Dictionary<TMP_InputField,string>();
        }

        // Called when a textbox is deselected and adds change to stack of changes
        public void TextBoxUpdate(TMP_InputField textBox){
            ToggleOrTextbox toAdd = new ToggleOrTextbox(textBox,null,prevText[textBox],false); // There will always be a prevText value as you have to select a textbox before you deselect
            pastActions.Push(toAdd);
            prevText[textBox] = textBox.text; // prevText set here as well to account for redos
        }

        // Called when a toggle is changed and adds change to stack of changes
        public void ToggleUpdate(Toggle toggle){
            ToggleOrTextbox toAdd = new ToggleOrTextbox(null,toggle,"",!toggle.isOn); // Toggle values is always switched when changed  so prev value will be negation
            pastActions.Push(toAdd);
        }

        // Called when textbox is selected to get the current value and set it as previous
        public void TextBoxSelect(TMP_InputField textBox){
            prevText[textBox] = textBox.text;
        }

        // Called when undo button is clicked
        public void Undo(){
            ToggleOrTextbox top;
            bool hasObject = pastActions.TryPop(out top);
            if (hasObject){
                redoStack.Push(top);
                if (top.TextBox){
                    if (!prevText.ContainsKey(top.TextBox)){
                        prevText[top.TextBox] = "";
                    }
                    prevText[top.TextBox] = top.TextBox.text;
                    top.TextBox.text = top.Text;
                    top.Text = prevText[top.TextBox]; // Set to prev value for redo stack

                }
                else{
                    top.Toggle.isOn = top.On;
                    pastActions.Pop(); // Need to pop again as the change from the undo is added to the stack as the event listener is on toggle value change
                    top.On = !top.On; // Set to prev value for redo stack usage
                }
            }
        }

        public void Redo(){
            ToggleOrTextbox top;
            bool hasObject = redoStack.TryPop(out top);
            if (hasObject){
                if (top.TextBox){
                    prevText[top.TextBox] = top.TextBox.text;
                    top.TextBox.text = top.Text;
                    TextBoxUpdate(top.TextBox);
                }
                else{
                    top.Toggle.isOn = top.On;
                    pastActions.Pop(); // Need to pop again as the change from the redo is added to the stack as the event listener is on toggle value change
                    ToggleUpdate(top.Toggle);
                }
            }
        }

        // Redo goes to previous states after a reset so stacks are all reset to avoid unexpected behaviour
        public void OnReset(){
            pastActions.Clear();
            redoStack.Clear();
            prevText.Clear();
        }
    }
}
