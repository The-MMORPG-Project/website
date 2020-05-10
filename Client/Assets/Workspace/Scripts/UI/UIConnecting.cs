using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

using TMPro;

namespace Valk.Networking
{
    public class UIConnecting : MonoBehaviour
    {
        public GameObject GoText;
        private TextMeshProUGUI Text;

        string message = "Attempting to Connect to Servers";
        string[] dots = new string[] { "", ".", "..", "..." };

        void Start()
        {
            Client.Connect();

            Text = GoText.GetComponent<TextMeshProUGUI>();
            StartCoroutine(AnimateDots());
        }

        void FixedUpdate()
        {
            if (!Client.IsConnected())
                return;

            SceneManager.LoadScene("Account Management");
        }

        IEnumerator AnimateDots()
        {
            int i = 0;
            while (!Client.IsConnected())
            {
                // Animate connecting text
                Text.text = message + dots[i++ % dots.Length];
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}

