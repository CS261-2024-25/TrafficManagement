using UnityEngine;
using UnityEngine.SceneManagement;

public class VwPrevResultsBtn : MonoBehaviour
{
    public void BtnClickVwPrevResults(){
        Debug.LogError("Button Clicked! Trying to load PrioritiseJuncMetric SCENE");
        SceneManager.LoadScene("PrioritiseJuncMetric");
    }

}
