using UnityEngine;

public class ClosePanel : MonoBehaviour
{
    public GameObject panel;

    /// <summary>
    /// Toggles off panel
    /// </summary>
    public void HidePanel(){
        panel.SetActive(false);
    }

    /// <summary>
    /// Toggles on panel
    /// </summary>
    public void ShowPanel(){
        panel.SetActive(true);
    }
}
