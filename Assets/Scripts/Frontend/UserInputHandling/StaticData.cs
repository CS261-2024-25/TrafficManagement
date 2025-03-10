using UnityEngine;
using Assets.Scripts.Util;

public class StaticData : MonoBehaviour
{
    // All data in this file is saved between scene transitions
    public static DirectionDetails northbound;
    public static DirectionDetails eastbound;
    public static DirectionDetails southbound;
    public static DirectionDetails westbound;
    public static (CardinalDirection, double)[] priority = new (CardinalDirection,double)[4];
    public static int arrIndex = 0;
    public static bool hasLeftTurn = false;
    public static bool oneIncoming = false;
    public static bool failDirectionParse = false; // Check for ConfigureDirection
    public static bool failFlowParse = false; // Check for CreateStruct
    public static double totPrio = 0; 
    public static bool saved = false; // If user input was a previously saved junction then sets to true
}
