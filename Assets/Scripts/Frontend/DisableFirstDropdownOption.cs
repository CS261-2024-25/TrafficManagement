using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisableFirstDropdownOption : MonoBehaviour
{
    public GameObject objectToCheck;
    void Start (){
        TMP_Dropdown[] dropdowns;
        dropdowns = objectToCheck.GetComponentsInChildren<TMP_Dropdown>();
        foreach (TMP_Dropdown dropdown in dropdowns){
            dropdown.onValueChanged.AddListener((index) => // Listener used on change to ensure the user can't select option saying priority
            {
                if (index == 0) dropdown.value = 1;
            });
        }
    }
}
