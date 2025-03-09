using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
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

    public void BackToLoadedResultsScene(){
        SceneManager.LoadScene("LoadedResultsScreen");
    }


    //Method to go back to results screen FROM graphic display screen 
    public void BackToResults(){
        SceneManager.LoadScene("ResultsScreen");
    }
}
