using UnityEngine;
using System;
using System.Collections.Generic;
using Assets.Scripts.Frontend.StackTypes;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class HandleStack : MonoBehaviour
{
    private Stack<ToggleOrTextbox> pastActions;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pastActions = new Stack<ToggleOrTextbox>();
    }

    // Called when a textbox updates
    public void TextBoxUpdate(TMP_InputField textBox){
        ToggleOrTextbox toAdd = new ToggleOrTextbox(textBox,null,textBox.text,false);
        pastActions.Push(toAdd);
    }

    public void ToggleUpdate(Toggle toggle){
        ToggleOrTextbox toAdd = new ToggleOrTextbox(null,toggle,"",toggle.isOn);
        pastActions.Push(toAdd);
    }

    public void undo(){
        ToggleOrTextbox top;
        bool hasObject = pastActions.TryPop(out top);
        if (hasObject){
            if (top.textBox){
                top.textBox.text = top.text;
            }
            else{
                top.toggle.isOn = top.on;
            }
        }
    }
}
