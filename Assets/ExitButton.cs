using UnityEngine;

public class ExitButton : MonoBehaviour // This script is used by Exit manager so multiple quit buttons can be used
{
    public void Exit(){
        //Debug.Log("Quit"); // Uncomment for testing
        Application.Quit();
    }
}
