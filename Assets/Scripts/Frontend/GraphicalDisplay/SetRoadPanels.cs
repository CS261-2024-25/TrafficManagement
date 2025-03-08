using System.ComponentModel;
using UnityEngine;

public class SetRoadPanels : MonoBehaviour
{
    public GameObject parent;
    // Panels for each combination of lanes
    // public GameObject north2;
    // public GameObject north3;
    // public GameObject north4;
    // public GameObject north5;

    // public GameObject south2;
    // public GameObject south3;
    // public GameObject south4;
    // public GameObject south5;

    // public GameObject east2;
    // public GameObject east3;
    // public GameObject east4;
    // public GameObject east5;

    // public GameObject west2;
    // public GameObject west3;
    // public GameObject west4;
    // public GameObject west5;
    
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
                northOn = GameObject.Find("Canvas/Panel/North 2");
                break;
            case 3:
                northOn = GameObject.Find("Canvas/Panel/North 3");
                break;
            case 4:
                northOn = GameObject.Find("Canvas/Panel/North 4"); 
                break;
            case 5:
                northOn = GameObject.Find("Canvas/Panel/North 5");
                break;
            default:
                northOn = null;
                Debug.LogError("Invalid panel number");
                break;
        }
        if (northOn != null){
            northOn.SetActive(true);
        }


        GameObject southOn;
        switch (StaticData.southbound.LaneCountInbound + StaticData.southbound.LaneCountOutbound)
        {
            case 2:
                southOn = GameObject.Find("Canvas/Panel/South 2");
                break;
            case 3:
                southOn = GameObject.Find("Canvas/Panel/South 3");
                break;
            case 4:
                southOn = GameObject.Find("Canvas/Panel/South 4");
                break;
            case 5:
                southOn = GameObject.Find("Canvas/Panel/South 5");
                break;
            default:
                southOn = null;
                Debug.LogError("Invalid panel number");
                break;
        }
        if (southOn != null){
            southOn.SetActive(true);
        }

        GameObject eastOn;
        switch (StaticData.eastbound.LaneCountInbound + StaticData.eastbound.LaneCountOutbound)
        {
            case 2:
                eastOn = GameObject.Find("Canvas/Panel/East 2");
                break;
            case 3:
                eastOn = GameObject.Find("Canvas/Panel/East 3");
                break;
            case 4:
                eastOn = GameObject.Find("Canvas/Panel/East 4");
                break;
            case 5:
                eastOn = GameObject.Find("Canvas/Panel/East 5");
                break;
            default:
                eastOn = null;
                Debug.LogError("Invalid panel number");
                break;
        }
        if (eastOn != null){
            eastOn.SetActive(true);
        }

        GameObject westOn;
        switch (StaticData.westbound.LaneCountInbound + StaticData.westbound.LaneCountOutbound)
        {
            case 2:
                westOn = GameObject.Find("Canvas/Panel/West 2");
                break;
            case 3:
                westOn = GameObject.Find("Canvas/Panel/West 3");
                break;
            case 4:
                westOn = GameObject.Find("Canvas/Panel/West 4");
                break;
            case 5:
                westOn = GameObject.Find("Canvas/Panel/West 5");
                break;
            default:
                westOn = null;
                Debug.LogError("Invalid panel number");
                break;
        }
        if (westOn != null){
            westOn.SetActive(true);
        }
    }

}
