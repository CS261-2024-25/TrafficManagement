using UnityEngine;

public class ClosePanel : MonoBehaviour
{
    public GameObject panel;

    public void HidePanel(){
        panel.SetActive(false);
    }
}
