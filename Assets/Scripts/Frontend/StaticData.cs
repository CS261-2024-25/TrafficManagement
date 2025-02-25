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
}
