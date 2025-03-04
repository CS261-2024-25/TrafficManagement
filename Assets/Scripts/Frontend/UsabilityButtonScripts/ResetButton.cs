using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResetButton : MonoBehaviour
{
    private TMP_InputField[] textBoxes;
    public GameObject objectToCheck;
    private Toggle[] toggles;

    void Start()
    {
        textBoxes = objectToCheck.GetComponentsInChildren<TMP_InputField>();
        toggles = objectToCheck.GetComponentsInChildren<Toggle>();

    }

    public void Reset()
    {
        foreach(TMP_InputField textBox in textBoxes){
            textBox.text = "";
        }

        foreach(Toggle toggle in toggles){
            toggle.isOn = true;
        }
    }
}
