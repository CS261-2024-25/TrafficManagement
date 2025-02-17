using UnityEngine;
using TMPro;

public class RestrictLaneNum : MonoBehaviour
{
    public TMP_InputField inputText;

/**
    Function should be used when a lane num is updated and resets invalid text
*/
    public void ResetInvalidText(){
        if (inputText.text != "1" && inputText.text != "2" && inputText.text != "3" && inputText.text != "4" && inputText.text != "5"){
            inputText.text = "";
        }
    }
}
