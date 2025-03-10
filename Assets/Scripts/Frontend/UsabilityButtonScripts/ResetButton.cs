using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResetButton : MonoBehaviour
{
    private TMP_InputField[] textBoxes;
    public GameObject objectToCheck;
    private Toggle[] toggles;

    /// <summary>
    /// Gets list of all present toggles and textboxes on screen (script usually attatched to main panel)
    /// </summary>
    void Start()
    {
        textBoxes = objectToCheck.GetComponentsInChildren<TMP_InputField>();
        toggles = objectToCheck.GetComponentsInChildren<Toggle>();

    }

    /// <summary>
    /// Sets all toggles to on and all textboxes to empty
    /// </summary>
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
