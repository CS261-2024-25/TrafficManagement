using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
    //Method to go back to parameter select svcreen FROM Traffic Flow screen
    public void BackToParameter(){
        SceneManager.LoadScene("ParameterSelectScreen");
    }

    //Method to go back to config screen for Traffic flow FROM results screen 
    public void BackToTrafficFlowScreen(){
        SceneManager.LoadScene("TrafficFlowSelect");
    }

    //Method to go back to config screen for Traffic flow FROM results screen 
    public void BackToPrioritiseJuncMetricScene(){
        SceneManager.LoadScene("PrioritiseJuncMetric");
    }

    //Method to go back to results screen FROM graphic display screen/load prev results 
    public void BackToResults(){
        SceneManager.LoadScene("ResultsScreen");
    }
}
