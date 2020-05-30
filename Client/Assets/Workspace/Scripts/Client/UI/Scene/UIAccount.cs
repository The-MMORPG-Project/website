using UnityEngine;
using UnityEngine.SceneManagement;

namespace Valk.Networking 
{
    public class UIAccount : MonoBehaviour 
    {
        public void Play()
        {
            SceneManager.LoadScene("Main");
        }
    }
}