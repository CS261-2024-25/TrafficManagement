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

        /// <summary>
        /// Initialises stack structures
        /// </summary>
        void Start()
        {
            pastActions = new Stack<ToggleOrTextbox>();
            redoStack = new Stack<ToggleOrTextbox>();
            prevText = new Dictionary<TMP_InputField,string>();
        }

        /// <summary>
        /// Called when a textbox is deselected and adds change to stack of changes
        /// </summary>
        /// <param name="textBox">textbox that was changed</param>
        public void TextBoxUpdate(TMP_InputField textBox){
            ToggleOrTextbox toAdd = new ToggleOrTextbox(textBox,null,prevText[textBox],false); // There will always be a prevText value as you have to select a textbox before you deselect
            pastActions.Push(toAdd);
            prevText[textBox] = textBox.text; // prevText set here as well to account for redos
        }

        /// <summary>
        /// Called when a toggle is changed and adds change to stack of changes
        /// </summary>
        /// <param name="toggle">toggle changed</param>
        public void ToggleUpdate(Toggle toggle){
            ToggleOrTextbox toAdd = new ToggleOrTextbox(null,toggle,"",!toggle.isOn); // Toggle values is always switched when changed  so prev value will be negation
            pastActions.Push(toAdd);
        }

        /// <summary>
        /// Called when textbox is selected to get the current value and set it as previous
        /// </summary>
        /// <param name="textBox">textbox selected</param>
        public void TextBoxSelect(TMP_InputField textBox){
            prevText[textBox] = textBox.text;
        }

        /// <summary>
        /// Called when undo button is clicked and pops the most recent value off the pastActions stack and restores the state of the text box/toggle mentioned
        /// in that stack object to the value stored in the stack object. Pushes the object to the redo stack for usage there.
        /// </summary>
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

        /// <summary>
        /// Used when Redo is clicked and pops a value off the redo stack and restores the data of the relevant gameobject to that state
        /// </summary>
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

        /// <summary>
        /// Called when reset is clicked to avoid any unexpected behaviour
        /// </summary>
        public void OnReset(){
            pastActions.Clear();
            redoStack.Clear();
            prevText.Clear();
        }
    }
}
