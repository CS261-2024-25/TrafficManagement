using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
    public void BackToParameter(){
        SceneManager.LoadScene("ParameterSelectScreen");
    }

    //Method to go back to  config screen for Traffic flow  FROM results screen 
    public void BackToTrafficFlowScreen(){
        SceneManager.LoadScene("TrafficFlowSelect");
    }
}
