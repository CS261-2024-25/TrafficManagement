using UnityEngine;
using UnityEngine.SceneManagement;

public class VwPrevResultsBtn : MonoBehaviour
{
    public void BtnClickVwPrevResults(){
        SceneManager.LoadScene("PrioritiseJuncMetric");
    }

    public void ViewInputs(){
        SceneManager.LoadScene("ViewInputs");
    }

}
