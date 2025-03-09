using UnityEngine;
using Assets.Scripts.Backend.PersistentJunctionSave;
using Assets.Scripts.Util;

//Script for donot destroy game object script for  persistent junction data 

public class LoadedResultInstanceManager : MonoBehaviour
{
    public static LoadedResultInstanceManager Instance; 

    public ResultJunctionEntrance northResult; 
    public ResultJunctionEntrance southResult; 
    public ResultJunctionEntrance eastResult; 
    public ResultJunctionEntrance westResult; 

    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        } 

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
