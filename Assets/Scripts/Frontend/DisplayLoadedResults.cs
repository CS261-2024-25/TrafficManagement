using UnityEngine;
using TMPro;
using Assets.Scripts.Util;
using UnityEngine.SceneManagement;
using Assets.Scripts.Backend.PersistentJunctionSave;
using  System;

public class DisplayLoadedResults : MonoBehaviour
{
    
    //Output Field Variables
    public TMP_Text northAvgWaitText, northMaxWaitText, northMaxQueueText;
    public TMP_Text southAvgWaitText, southMaxWaitText, southMaxQueueText;
    public TMP_Text eastAvgWaitText, eastMaxWaitText, eastMaxQueueText;
    public TMP_Text westAvgWaitText, westMaxWaitText, westMaxQueueText;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        ClearResults();
        PopulateResults();
    }

    // Update methodd not needed 

    public void ClearResults(){
        //removed loop AND HARDCODED --> NO CHANCE OF ERROR
        northAvgWaitText.text = "";
        northMaxWaitText.text = "";
        northMaxQueueText.text = "";

        southAvgWaitText.text = "";
        southMaxWaitText.text = "";
        southMaxQueueText.text = "";

        eastAvgWaitText.text = "";
        eastMaxWaitText.text = "";
        eastMaxQueueText.text = "";

        westAvgWaitText.text = "";
        westMaxWaitText.text = "";
        westMaxQueueText.text = "";
    }

    public void PopulateResults(){
        if (LoadedResultInstanceManager.Instance != null){
        //Population code from previous results screen
            ResultJunctionEntrance north=LoadedResultInstanceManager.Instance.northResult;
            ResultJunctionEntrance south=LoadedResultInstanceManager.Instance.southResult;
            ResultJunctionEntrance east=LoadedResultInstanceManager.Instance.eastResult;
            ResultJunctionEntrance west=LoadedResultInstanceManager.Instance.westResult;
            
            northAvgWaitText.text = $"{north.AverageWaitTime:F1} sec";
            northMaxWaitText.text = $"{north.MaxWaitTime:F1} sec";
            northMaxQueueText.text = $"{Math.Ceiling(north.MaxQueueLength)} vehicles";

            southAvgWaitText.text = $"{south.AverageWaitTime:F1} sec";
            southMaxWaitText.text = $"{south.MaxWaitTime:F1} sec";
            southMaxQueueText.text = $"{Math.Ceiling(south.MaxQueueLength)} vehicles";

            eastAvgWaitText.text = $"{east.AverageWaitTime:F1} sec";
            eastMaxWaitText.text = $"{east.MaxWaitTime:F1} sec";
            eastMaxQueueText.text = $"{Math.Ceiling(east.MaxQueueLength)} vehicles";

            westAvgWaitText.text = $"{west.AverageWaitTime:F1} sec";
            westMaxWaitText.text = $"{west.MaxWaitTime:F1} sec";
            westMaxQueueText.text = $"{Math.Ceiling(west.MaxQueueLength)} vehicles";
        }
        else{
            Debug.LogError("LoadedReusultInstanceManager Instance not initialised (is null) ");
        }
    }

    public void btnClickViewJunctionSimulation(){
        SceneManager.LoadScene("GraphicalJunctionSimulationScreen");
        Debug.LogError("Button Clicked! Trying to load JunctionSimulation SCENE");
        
    }



}
