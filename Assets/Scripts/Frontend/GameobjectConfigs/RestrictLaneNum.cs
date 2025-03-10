using UnityEngine;
using TMPro;

public class RestrictLaneNum : MonoBehaviour
{
    public TMP_InputField inputText;

    /// <summary>
    /// Attatched to all text inputs on parameter select screen to only only valid lane numbers to be entered.
    /// Instantly resets if not valid appears to user as they are unable to add them
    /// </summary>
    public void ResetInvalidText(){
        if (inputText.text != "1" && inputText.text != "2" && inputText.text != "3" && inputText.text != "4"){
            inputText.text = "";
        }
    }
}
