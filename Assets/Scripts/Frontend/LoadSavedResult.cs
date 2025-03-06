using UnityEngine;
using TMPro;
using Assets.Scripts.Util;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Assets.Scripts.Backend.PersistentJunctionSave;
using  System;

public class LoadSavedResult : MonoBehaviour
{
    public int totalJunctionResultsSaved=0;
    
    //UI input field variables
    public TMP_InputField averageWaitTimeCoefficientText;
    public TMP_InputField maximumWaitTimeCoefficientText;
    public TMP_InputField maximumQueueLengthCoefficientText;

    //UI output field variable
    public TMP_Text numberOfSavedJunctionResultsText;

    //UI input field variables
    public TMP_InputField juncResultInstanceToFetch;

    //UI error panel
    public GameObject errorPanel;
    public TMP_Text errorText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ClearAllText();
        totalJunctionResultsSaved=GetTotalNumberOfResults();
        numberOfSavedJunctionResultsText.text = $"{totalJunctionResultsSaved} results";
        
    }

    public void ClearAllText(){
        //HARDCODED --> NO CHANCE OF ERROR
        averageWaitTimeCoefficientText.text = "";
        maximumWaitTimeCoefficientText.text = "";
        maximumQueueLengthCoefficientText.text = "";

        numberOfSavedJunctionResultsText.text = "";
        juncResultInstanceToFetch.text = "";
    }

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

    public bool TryGetInputValues(out double avgWaitCoeff, out double maxWaitCoeff, out double maxQueueCoeff, out int instanceToFetch){
        bool success = true;
        if (!double.TryParse(averageWaitTimeCoefficientText.text, out avgWaitCoeff))
        {
            //Debug.LogError("Failed to parse average wait time coefficient.");
            success = false;
        }

        if (!double.TryParse(maximumWaitTimeCoefficientText.text, out maxWaitCoeff))
        {
            //Debug.LogError("Failed to parse maximum wait time coefficient.");
            success = false;
        }
        
        if (!double.TryParse(maximumQueueLengthCoefficientText.text, out maxQueueCoeff))
        {
            //Debug.LogError("Failed to parse maximum queue length coefficient.");
            success = false;
        }
        
        if (!int.TryParse(juncResultInstanceToFetch.text, out instanceToFetch))
        {
            //Debug.LogError("Failed to parse junction result instance.");
            success = false;
        }
        
        return success;
    }


    public bool StorePersistentResultsForScreens(ResultJunctionEntrance northResult,ResultJunctionEntrance southResult,ResultJunctionEntrance eastResult,ResultJunctionEntrance westResult){
        if (LoadedReusultInstanceManager.Instance == null) {
            Debug.LogError("Persistent data manager is not initialized.");
            return false;
        }
        
        LoadedReusultInstanceManager.Instance.northResult=northResult;
        LoadedReusultInstanceManager.Instance.southResult=southResult;
        LoadedReusultInstanceManager.Instance.eastResult=eastResult;
        LoadedReusultInstanceManager.Instance.westResult=westResult;
        
        return true;

    }



    //public (ResultJunctionEntrance north, ResultJunctionEntrance south, ResultJunctionEntrance east, ResultJunctionEntrance west) GetSpecifiedSimulationResults() {
    public bool GetSpecifiedSimulationResults() {
        double avgWaitCoeff, maxWaitCoeff, maxQueueCoeff;
        int instanceToFetch;

        bool isInputParseSuccess = TryGetInputValues(out avgWaitCoeff, out maxWaitCoeff,out  maxQueueCoeff, out instanceToFetch);

        if (!isInputParseSuccess ||
            (avgWaitCoeff > 3) ||
            (avgWaitCoeff < 0) ||
            (maxWaitCoeff > 3) ||
            (maxWaitCoeff < 0) ||
            (maxQueueCoeff > 3) ||
            (maxQueueCoeff < 0) 
        )
        {
            Debug.Log(isInputParseSuccess);
            errorText.text = "Priority numbers must be between 0 and 3 (inclusive)."; //To remake msg
            errorPanel.SetActive(true);
            //return (null, null, null, null);
            return false;
        }
        else if (
            (instanceToFetch < 1) ||
            (instanceToFetch > totalJunctionResultsSaved)
        ){
            errorText.text = "The junction result to display must be less than the number of saved configurations"; //To remake msg
            errorPanel.SetActive(true);
            //return (null, null, null, null);
            return false;
        }

        else
        {
            List<(double, (InputParameters, ResultTrafficSimulation))> allResults;
            bool isLoadSuccess = PersistentJunctionSave.LoadByEfficiency(avgWaitCoeff, maxWaitCoeff, maxQueueCoeff, out allResults );
            
            //allResults.Count minimises chance of error is page is reloaded after totalJunctionResultsSaved initialised
            if (isLoadSuccess && allResults.Count > 0 && instanceToFetch>=1 && instanceToFetch<=allResults.Count) {
                var selectedTuple = allResults[instanceToFetch-1]; // Convert 1-based user input to 0-based index
                ResultTrafficSimulation resultInstanceToFetch = selectedTuple.Item2.Item2;

                ResultJunctionEntrance northResult = resultInstanceToFetch.ResultWithDirection(CardinalDirection.North);
                ResultJunctionEntrance southResult = resultInstanceToFetch.ResultWithDirection(CardinalDirection.South);
                ResultJunctionEntrance eastResult = resultInstanceToFetch.ResultWithDirection(CardinalDirection.East);
                ResultJunctionEntrance westResult = resultInstanceToFetch.ResultWithDirection(CardinalDirection.West);
                bool isStoringSuccess= StorePersistentResultsForScreens(northResult, southResult, eastResult, westResult); //added line for more clarity
                return isStoringSuccess;
            }
            else{
                errorText.text = "The junction result to display must be less than the number of saved configurations";
                //Debug.LogError("No simulation results found - please run the simulation first.");
                errorPanel.SetActive(true);
                //return (null, null, null, null);
                return false;
            }
        }

    } 
    public void BtnClickViewLoadedResults(){
        //var (north,south,east,west) = GetSpecifiedSimulationResults();
        //Debug.LogError("Button Clicked! Testing user inputs");
        bool isLoadingSuccess=GetSpecifiedSimulationResults();
        if (!isLoadingSuccess)
        {
            Debug.LogError("Failed to load junction results. Cannot proceed to next scene.");
            return;
        }
        else{
            Debug.LogError("isLoadingSuccess= true! Trying to load Junction results SCENE");
            SceneManager.LoadScene("DisplayLoadedResults");
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    
}

