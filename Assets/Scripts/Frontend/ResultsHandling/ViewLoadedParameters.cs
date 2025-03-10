using UnityEngine;
using Assets.Scripts.Util;
using TMPro;

public class ViewLoadedParameters : MonoBehaviour
{
    
    public CardinalDirection directionSelected;
    public TMP_Text isLeftEnabledTxtOut;
    public TMP_Text isCrossingEnabledTxtOut;
    public TMP_Text noIncomingLanesTxtOut;

    public TMP_Text noOutgoingLanesTxtOut;

    public TMP_Text incomingTrafficFlowTxtOut;

    public TMP_Text dir1OutgoingTrafficFlowTxtOut;

    public TMP_Text dir2OutgoingTrafficFlowTxtOut;

    public TMP_Text dir3OutgoingTrafficFlowTxtOut;

    public TMP_Text noForDirPriorityTxtOut;


    // Start is called once before the first execution of Update after the MonoBehaviour is created


    /// <summary>
    /// Displays the input parameters of the currently selected junction on the ViewInputs screen for a given direction.
    /// This is called once for each direction and the results are fetched from StaticData.
    /// </summary>
    void Start()
    {
        DirectionDetails direction;
        switch (directionSelected){
            case CardinalDirection.North:
                direction = StaticData.northbound;
                break;
            case CardinalDirection.South:
                direction = StaticData.southbound;
                break;
            case CardinalDirection.East:
                direction = StaticData.eastbound;
                break;
            case CardinalDirection.West:
                direction = StaticData.westbound;
                break;
            default: // Should never run
                direction = null;
                break;
        }

        isLeftEnabledTxtOut.text += direction.HasLeftTurn ? "Enabled" : "Disabled";
        isCrossingEnabledTxtOut.text +=direction.HasPedestrianCrossing ? "Enabled" : "Disabled";
        noIncomingLanesTxtOut.text += direction.LaneCountInbound.ToString() ;
        noOutgoingLanesTxtOut.text +=direction.LaneCountOutbound.ToString() ;
        dir1OutgoingTrafficFlowTxtOut.text +=direction.LeftFlow.ToString() ;
        dir2OutgoingTrafficFlowTxtOut.text +=direction.ForwardFlow.ToString() ;
        dir3OutgoingTrafficFlowTxtOut.text +=direction.RightFlow.ToString() ;
        
        uint incomingTrafficFlowNumber= direction.RightFlow + direction.ForwardFlow + direction.RightFlow;
        incomingTrafficFlowTxtOut.text+= incomingTrafficFlowNumber.ToString();

        foreach ((CardinalDirection, double) tup in StaticData.priority){
            if (tup.Item1 == directionSelected){
                noForDirPriorityTxtOut.text += tup.Item2;
            }
        }
        
    }
}
