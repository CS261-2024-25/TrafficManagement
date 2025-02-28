using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Assets.Scripts.Frontend.StackTypes{
public class ToggleOrTextbox{
    public TMP_InputField textBox {get;}
    public Toggle toggle {get;}
    public string text {get;}
    public bool on {get;}

    public ToggleOrTextbox
    (
        TMP_InputField textBox, // Pass in Null if using a toggle
        Toggle toggle, // Pass in Null if using if using a textBox
        string text,
        bool on
    )
    {
        this.textBox = textBox;
        this.toggle = toggle;
        this.text = text;
        this.on = on;
    }
}



}

