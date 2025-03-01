using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Assets.Scripts.Frontend.Stack{
    public class ToggleOrTextbox{
        public TMP_InputField TextBox {get;}
        public Toggle Toggle {get;}
        public string Text {get;set;}
        public bool On {get;set;}

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

