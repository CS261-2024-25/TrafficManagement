using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

using System.IO; // To parse strings
using Assets.Scripts.Util;
using UnityEngine.SceneManagement;

public class DisplayResults : MonoBehaviour
{
    //Output Field Variables
    public TMP_Text northAvgWaitText, northMaxWaitText, northMaxQueueText;
    public TMP_Text southAvgWaitText, southMaxWaitText, southMaxQueueText;
    public TMP_Text eastAvgWaitText, eastMaxWaitText, eastMaxQueueText;
    public TMP_Text westAvgWaitText, westMaxWaitText, westMaxQueueText;

    void Start()
    {
        // hides or resets the text fields as soon as the scene starts
        ClearResults();
    }



    //public void UpdateResults(TrafficSimulationResults SimulationResults ) //To be uncommented when objects available from backend
    public void UpdateResults()
    {
        // Fetch results from static dictionary/Backend Result Object 
        // JunctionResults north = SimulationResults.results["Northbound"];
        // JunctionResults south = SimulationResults.results["Southbound"];
        // JunctionResults east = SimulationResults.results["Eastbound"];
        // JunctionResults west = SimulationResults.results["Westbound"];

        
        // Sets text values FOR TIME with one decimal place formatting (only numbers + "sec")
        // Sets text values FOR QUEUE LENGTH rounded up to nearest integer (as we want maximum queue) , (only numbers + "sec")
        /*northAvgWaitText.text = $"{north.AvgWaitTime:F1} sec";
        northMaxWaitText.text = $"{north.MaxWaitTime:F1} sec";
        northMaxQueueText.text = $"{Math.Ceiling(north.MaxQueueLength)} vehicles";

        southAvgWaitText.text = $"{south.AvgWaitTime:F1} sec";
        southMaxWaitText.text = $"{south.MaxWaitTime:F1} sec";
        southMaxQueueText.text = $"{Math.Ceiling(south.MaxQueueLength)} vehicles";

        eastAvgWaitText.text = $"{east.AvgWaitTime:F1} sec";
        eastMaxWaitText.text = $"{east.MaxWaitTime:F1} sec";
        eastMaxQueueText.text = $"{Math.Ceiling(east.MaxQueueLength)} vehicles";

        westAvgWaitText.text = $"{west.AvgWaitTime:F1} sec";
        westMaxWaitText.text = $"{west.MaxWaitTime:F1} sec";
        westMaxQueueText.text = $"{Math.Ceiling(west.MaxQueueLength)} vehicles";
        */
    }

    public void ClearResults(){
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


    public void btnClickViewResults(){
        //simulationResults = RetriveSimulationResults()
        //UpdateResults(simulationResults); //To be uncommented when objects available from backend
        UpdateResults(); //To comment out when objects available from backend
    }

    public void btnClickViewJunctionSimulation(){
        SceneManager.LoadScene("GraphicalJunctionSimulationScreen");
        Debug.Log("Button Clicked! Trying to load scene...");
        
    }

    
}
/*

using UnityEngine;
using TMPro;
using Assets.Scripts.Util;
using UnityEngine.SceneManagement;
//using  Assets.Scripts.Backend.PersistentJunctionSave;
using Assets.Scripts.Backend.PersistentJunctionSave;
using  System;


public class DisplayResults : MonoBehaviour
{
    //Output Field Variables
    public TMP_Text northAvgWaitText, northMaxWaitText, northMaxQueueText;
    public TMP_Text southAvgWaitText, southMaxWaitText, southMaxQueueText;
    public TMP_Text eastAvgWaitText, eastMaxWaitText, eastMaxQueueText;
    public TMP_Text westAvgWaitText, westMaxWaitText, westMaxQueueText;


    void Start()
    {
        // hides or resets the text fields as soon as the scene starts
        ClearResults();
    }


    public (ResultJunctionEntrance north, ResultJunctionEntrance south, ResultJunctionEntrance east, ResultJunctionEntrance west) GetLatestSimulationResults() {
        (InputParameters, ResultTrafficSimulation)[] allResults;
        bool isLoadSuccess = PersistentJunctionSave.LoadAllResults(out allResults);
        if (isLoadSuccess && allResults.Length > 0) {
            ResultTrafficSimulation mostRecent = allResults[allResults.Length - 1].Item2;
            ResultJunctionEntrance northResult = mostRecent.ResultWithDirection(CardinalDirection.North);
            ResultJunctionEntrance southResult = mostRecent.ResultWithDirection(CardinalDirection.South);
            ResultJunctionEntrance eastResult = mostRecent.ResultWithDirection(CardinalDirection.East);
            ResultJunctionEntrance westResult = mostRecent.ResultWithDirection(CardinalDirection.West);
            return (northResult, southResult, eastResult, westResult);
        }
        else{
            
            Debug.Log("No simulation results found - please run the simulation first.");
            return (null, null, null, null);
        }

    }

    //public void UpdateResults(TrafficSimulationResults SimulationResults ) //To be uncommented when objects available from backend
    public void UpdateResults()
    {
        // Fetch results from Backend
        var (north,south,east,west) = GetLatestSimulationResults();
        // Sets text values FOR TIME with one decimal place formatting (only numbers + "sec")
        // Sets text values FOR QUEUE LENGTH rounded up to nearest integer (as we want maximum queue) , (only numbers + "sec")
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

    public void ClearResults(){
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


    public void btnClickViewResults(){
        //simulationResults = RetriveSimulationResults()
        //UpdateResults(simulationResults); //To be uncommented when objects available from backend
        UpdateResults(); //To comment out when objects available from backend
    }

    public void btnClickViewJunctionSimulation(){
        SceneManager.LoadScene("GraphicalJunctionSimulationScreen");
        Debug.Log("Button Clicked! Trying to load scene...");
        
    }

    
}



*/