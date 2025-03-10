using UnityEngine;
using TMPro;
using Assets.Scripts.Util;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Assets.Scripts.Backend.PersistentJunctionSave;
using  System;
using UnityEngine.UI;

public class LoadSavedResult : MonoBehaviour
{
    public int totalJunctionResultsSaved=0;
    public string priorityTittleInitialText="Priority for each metric is: Reletive to the others & can be any number from 0 to 3 (inclusive)";
    
    //UI input field variables
    public TMP_InputField averageWaitTimeCoefficientText;
    public TMP_InputField maximumWaitTimeCoefficientText;
    public TMP_InputField maximumQueueLengthCoefficientText;

    //UI output field variable
    public TMP_Text numberOfSavedJunctionResultsText;
    public TMP_Text priorityExplanationWithNoSavedJuncsText;

    //UI input field variables
    public TMP_InputField juncResultInstanceToFetch;

    //UI error panel
    public GameObject errorPanel;
    public TMP_Text errorText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    /// <summary>
    /// First clears text then adds updated methods by calling relevant  methods
    /// </summary>
    void Start()
    {
        ClearAllText();
        totalJunctionResultsSaved=GetTotalNumberOfResults();
        numberOfSavedJunctionResultsText.text = $"{totalJunctionResultsSaved} results";
        priorityExplanationWithNoSavedJuncsText.text=priorityTittleInitialText;
        if (totalJunctionResultsSaved==1){
            priorityExplanationWithNoSavedJuncsText.text += $" across {totalJunctionResultsSaved} Junction Configuration";
        }
        else{
            priorityExplanationWithNoSavedJuncsText.text += $" across {totalJunctionResultsSaved} Junction Configurations";
        }
    }

    /// <summary>
    /// Clears all previous results text
    /// </summary>
    public void ClearAllText(){
        //HARDCODED --> NO CHANCE OF ERROR
        averageWaitTimeCoefficientText.text = "";
        maximumWaitTimeCoefficientText.text = "";
        maximumQueueLengthCoefficientText.text = "";

        numberOfSavedJunctionResultsText.text = "";
        juncResultInstanceToFetch.text = "";
    }

    /// <summary>
    /// Gets number of results stored in the JSON
    /// </summary>
    /// <returns>Number of results stored</returns>
    public int GetTotalNumberOfResults(){
        (InputParameters, ResultTrafficSimulation)[] allResults;
        bool isLoadSuccess = PersistentJunctionSave.LoadAllResults(out allResults);
        if (isLoadSuccess ) {
            return allResults.Length;
        }
        else{
            return 0;
        }
    }

    /// <summary>
    /// reads user input fields and parses them to assign variables to them
    /// </summary>
    /// <param name="avgWaitCoeff">variable for average wait coefficient to be stored in</param>
    /// <param name="maxWaitCoeff">variable for max wait coefficient to be stored in</param>
    /// <param name="maxQueueCoeff">variable for max queue coefficient to be stored in</param>
    /// <param name="instanceToFetch">variable for the index of the result to be fetched</param>
    /// <returns>true if succeeds, false if not</returns>
    public bool TryGetInputValues(out double avgWaitCoeff, out double maxWaitCoeff, out double maxQueueCoeff, out int instanceToFetch){
        bool success = true;
        if (!double.TryParse(averageWaitTimeCoefficientText.text, out avgWaitCoeff))
        {
            success = false;
        }

        if (!double.TryParse(maximumWaitTimeCoefficientText.text, out maxWaitCoeff))
        {
            success = false;
        }
        
        if (!double.TryParse(maximumQueueLengthCoefficientText.text, out maxQueueCoeff))
        {
            success = false;
        }
        
        if (!int.TryParse(juncResultInstanceToFetch.text, out instanceToFetch))
        {
            success = false;
        }
        
        return success;
    }


    /// <summary>
    /// Prepares input data to be stored
    /// </summary>
    /// <param name="northResult">North configuration</param>
    /// <param name="southResult">South configuration</param>
    /// <param name="eastResult">East configuration</param>
    /// <param name="westResult">West configuration</param>
    /// <returns>true if data manager is initialised, false otherwise</returns>
    public bool StorePersistentResultsForScreens(ResultJunctionEntrance northResult,ResultJunctionEntrance southResult,ResultJunctionEntrance eastResult,ResultJunctionEntrance westResult){
        if (LoadedResultInstanceManager.Instance == null) {
            Debug.LogError("Persistent data manager is not initialized.");
            return false;
        }
        
        LoadedResultInstanceManager.Instance.northResult=northResult;
        LoadedResultInstanceManager.Instance.southResult=southResult;
        LoadedResultInstanceManager.Instance.eastResult=eastResult;
        LoadedResultInstanceManager.Instance.westResult=westResult;
        
        return true;

    }



    /// <summary>
    /// Gets the relevant stored junction data from secondary storage based on the user's priority and rank inputs and stores them in persistent data
    /// </summary>
    /// <returns>True if successful, false otherwise</returns>
    public bool GetSpecifiedSimulationResults() {
        double avgWaitCoeff, maxWaitCoeff, maxQueueCoeff;
        int instanceToFetch;

        bool isInputParseSuccess = TryGetInputValues(out avgWaitCoeff, out maxWaitCoeff,out  maxQueueCoeff, out instanceToFetch);
        
        //1st if statement --> to be uncommented when testing can be done with existing junction results stored
       if (totalJunctionResultsSaved==0){
            Debug.LogError("No Junction results");
            errorText.text = "There are currently no stored juction configurations to view.Thus efficiency of junctions cannot be compared";
            errorPanel.SetActive(true);
            return false;
        }
        else if (!isInputParseSuccess ||
            (avgWaitCoeff > 3) ||
            (avgWaitCoeff < 0) ||
            (maxWaitCoeff > 3) ||
            (maxWaitCoeff < 0) ||
            (maxQueueCoeff > 3) ||
            (maxQueueCoeff < 0) 
        )
        {
            errorText.text = "Priority numbers must be between 0 and 3 (inclusive). Please adjust your entries."; 
            errorPanel.SetActive(true);
            return false;
        }
        /*commented out for testing that a DoNotDestroy object is created AS there is no result data stored 
        --> to be uncommented when testing can be done with existing junction results stored*/ 
        /* else if (
            (instanceToFetch < 1) ||
            (instanceToFetch > totalJunctionResultsSaved)
        ){
            errorText.text = "The junction result to display must be less than the number of saved configurations"; //To remake msg
            //errorText.text += "\nEnsure the junction result to display is less than the number of saved configurations. Please renter your entry.";
            errorPanel.SetActive(true);
            return false;
        } */
        
        else
        {
            List<(double, (InputParameters, ResultTrafficSimulation))> allResults;
            bool isLoadSuccess = PersistentJunctionSave.LoadByEfficiency(avgWaitCoeff, maxWaitCoeff, maxQueueCoeff, out allResults );
            
            //allResults.Count minimises chance of error is page is reloaded after totalJunctionResultsSaved initialised
            if (isLoadSuccess) {
                StaticData.saved = true;
                var selectedTuple = allResults[instanceToFetch-1]; // Convert 1-based user input to 0-based index

                InputParameters inputs = selectedTuple.Item2.Item1; // Saved in static data for use by graphical display
                StaticData.northbound = inputs.Northbound;
                StaticData.southbound = inputs.Southbound;
                StaticData.eastbound = inputs.Eastbound;
                StaticData.westbound = inputs.Westbound;
                StaticData.priority = inputs.Priority;

                ResultTrafficSimulation resultInstanceToFetch = selectedTuple.Item2.Item2;

                ResultJunctionEntrance northResult = resultInstanceToFetch.ResultWithDirection(CardinalDirection.North);
                ResultJunctionEntrance southResult = resultInstanceToFetch.ResultWithDirection(CardinalDirection.South);
                ResultJunctionEntrance eastResult = resultInstanceToFetch.ResultWithDirection(CardinalDirection.East);
                ResultJunctionEntrance westResult = resultInstanceToFetch.ResultWithDirection(CardinalDirection.West);
                bool isStoringSuccess= StorePersistentResultsForScreens(northResult, southResult, eastResult, westResult); //added line for more clarity
                return isStoringSuccess;
            }
            else{
                errorText.text = "No Junction Efficiency Results found - please enter configuration parameters and run the simulation first.";
                Debug.LogError("No simulation results found - please run the simulation first.");
                errorPanel.SetActive(true);
                return false;

            }
        }

    } 

    /// <summary>
    /// Loads relevant scene when submit is clicked
    /// </summary>
    public void BtnClickViewLoadedResults(){
        bool isLoadingSuccess=GetSpecifiedSimulationResults();
        if (!isLoadingSuccess)
        {
            Debug.LogError("Failed to load junction results. Cannot proceed to next scene.");
            return;
        }
        else{
            Debug.LogError("isLoadingSuccess= true! Trying to load Junction results SCENE");
            SceneManager.LoadScene("ResultsScreen");
        }
    }

    
}

