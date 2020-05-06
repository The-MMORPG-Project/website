using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMainMenu : MonoBehaviour
{
    public void Multiplayer() 
    {
        SceneManager.LoadScene("Connecting");
    }

    public void Options() 
    {
        SceneManager.LoadScene("Options");
    }

    public void Exit() 
    {
        Application.Quit();
    }
}
