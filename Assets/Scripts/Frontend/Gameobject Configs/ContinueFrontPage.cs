using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueFrontPage : MonoBehaviour
{
    public void Continue() 
    {
        Debug.Log("continue clicked");
        Console.WriteLine("hello");
        SceneManager.LoadScene("ParameterSelectScreen");
    }
}
