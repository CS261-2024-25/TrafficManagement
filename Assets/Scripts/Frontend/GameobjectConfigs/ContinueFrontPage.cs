using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueFrontPage : MonoBehaviour
{

    /// <summary>
    /// Used to switch onto main application from start screen
    /// </summary>
    public void Continue() 
    {
        SceneManager.LoadScene("ParameterSelectScreen");
    }
}
