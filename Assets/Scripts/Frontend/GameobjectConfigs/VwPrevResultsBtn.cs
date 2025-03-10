using UnityEngine;
using UnityEngine.SceneManagement;

public class VwPrevResultsBtn : MonoBehaviour
{
    /// <summary>
    /// Used on button on results page to switch to previous junction screen
    /// </summary>
    public void BtnClickVwPrevResults(){
        SceneManager.LoadScene("PrioritiseJuncMetric");
    }

    /// <summary>
    /// Used on button on results page to switch to view inputs screen
    /// </summary>
    public void ViewInputs(){
        SceneManager.LoadScene("ViewInputs");
    }

}
