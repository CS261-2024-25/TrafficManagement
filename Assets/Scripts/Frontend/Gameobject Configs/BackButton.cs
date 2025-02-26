using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
    public void BackToParameter(){
        SceneManager.LoadScene("ParameterSelectScreen");
    }
}
