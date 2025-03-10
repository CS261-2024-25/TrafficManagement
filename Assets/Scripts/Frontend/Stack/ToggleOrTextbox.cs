using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Assets.Scripts.Frontend.Stack{
    public class ToggleOrTextbox{
        public TMP_InputField TextBox {get;}
        public Toggle Toggle {get;}
        public string Text {get;set;}
        public bool On {get;set;}

        /// <summary>
        /// Objects that will be pushed onto undo and redo stacks. Only one of textbox or toggle will be set (other will be null).
        /// If textbox is set then text is the inner value otherwise on is the inner value.
        /// </summary>
        /// <param name="textBox">textbox (null if toggle present)</param>
        /// <param name="toggle">toggle object (null if textbox present)</param>
        /// <param name="text">Previous inner text of textbox ("" if toggle)</param>
        /// <param name="on">Previous toggle state (set to false if textbox) </param>
        public ToggleOrTextbox
        (
            TMP_InputField textBox, // Pass in Null if using a toggle
            Toggle toggle, // Pass in Null if using if using a textBox
            string text,
            bool on
        )
        {
            this.TextBox = textBox;
            this.Toggle = toggle;
            this.Text = text;
            this.On = on;
        }
    }



}

