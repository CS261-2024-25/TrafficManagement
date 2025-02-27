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
    public TMP_Text northAvgWaitText;
    public TMP_Text northMaxWaitText;
    public TMP_Text northMaxQueueText;

    public TMP_Text southAvgWaitText;
    public TMP_Text southMaxWaitText;
    public TMP_Text southMaxQueueText;

    public TMP_Text eastAvgWaitText;
    public TMP_Text eastMaxWaitText;
    public TMP_Text eastMaxQueueText;

    public TMP_Text westAvgWaitText;
    public TMP_Text westMaxWaitText;
    public TMP_Text westMaxQueueText;

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


    public void btnClkViewResults(){
        //simulationResults = RetriveSimulationResults()
        //UpdateResults(simulationResults); //To be uncommented when objects available from backend
        UpdateResults(); //To comment out when objects available from backend
    }

    public void btnClkViewJunctionSimulation(){
        SceneManager.LoadScene("GraphicalJunctionSimulation");
    }
    
}
