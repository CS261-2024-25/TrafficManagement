using System.ComponentModel;
using UnityEngine;
using TMPro;

public class SetRoadPanels : MonoBehaviour
{
    public GameObject parent;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Sets all panels to false  
        foreach (Transform child in parent.transform)
        {
            child.gameObject.SetActive(false);
        }

        GameObject northOn;
        switch (StaticData.northbound.LaneCountInbound + StaticData.northbound.LaneCountOutbound)
        {
            case 2:
                northOn = GameObject.Find("Canvas/Panel/Centre/North 2");
                break;
            case 3:
                northOn = GameObject.Find("Canvas/Panel/Centre/North 3");
                break;
            case 4:
                northOn = GameObject.Find("Canvas/Panel/Centre/North 4"); 
                break;
            case 5:
                northOn = GameObject.Find("Canvas/Panel/Centre/North 5");
                break;
            default:
                northOn = null;
                Debug.LogError("Invalid panel number");
                break;
        }
        if (northOn){
            northOn.SetActive(true);
        }


        GameObject southOn;
        switch (StaticData.southbound.LaneCountInbound + StaticData.southbound.LaneCountOutbound)
        {
            case 2:
                southOn = GameObject.Find("Canvas/Panel/Centre/South 2");
                break;
            case 3:
                southOn = GameObject.Find("Canvas/Panel/Centre/South 3");
                break;
            case 4:
                southOn = GameObject.Find("Canvas/Panel/Centre/South 4");
                break;
            case 5:
                southOn = GameObject.Find("Canvas/Panel/Centre/South 5");
                break;
            default:
                southOn = null;
                Debug.LogError("Invalid panel number");
                break;
        }
        if (southOn){
            southOn.SetActive(true);
        }

        GameObject eastOn;
        switch (StaticData.eastbound.LaneCountInbound + StaticData.eastbound.LaneCountOutbound)
        {
            case 2:
                eastOn = GameObject.Find("Canvas/Panel/Centre/East 2");
                break;
            case 3:
                eastOn = GameObject.Find("Canvas/Panel/Centre/East 3");
                break;
            case 4:
                eastOn = GameObject.Find("Canvas/Panel/Centre/East 4");
                break;
            case 5:
                eastOn = GameObject.Find("Canvas/Panel/Centre/East 5");
                break;
            default:
                eastOn = null;
                Debug.LogError("Invalid panel number");
                break;
        }
        if (eastOn){
            eastOn.SetActive(true);
        }

        GameObject westOn;
        switch (StaticData.westbound.LaneCountInbound + StaticData.westbound.LaneCountOutbound)
        {
            case 2:
                westOn = GameObject.Find("Canvas/Panel/Centre/West 2");
                break;
            case 3:
                westOn = GameObject.Find("Canvas/Panel/Centre/West 3");
                break;
            case 4:
                westOn = GameObject.Find("Canvas/Panel/Centre/West 4");
                break;
            case 5:
                westOn = GameObject.Find("Canvas/Panel/Centre/West 5");
                break;
            default:
                westOn = null;
                Debug.LogError("Invalid panel number");
                break;
        }
        if (westOn){
            westOn.SetActive(true);
        }
    }

}
