using UnityEngine;
using UnityEngine.SceneManagement;

namespace Valk.Networking
{
    public class UIMainMenu : MonoBehaviour
    {
        public void Multiplayer()
        {
            SceneManager.LoadScene("Connect");
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
}

