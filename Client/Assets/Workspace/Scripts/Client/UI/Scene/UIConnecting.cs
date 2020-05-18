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

        private void Start()
        {
            Text = GoText.GetComponent<TextMeshProUGUI>();
            StartCoroutine(AnimateDots());
        }

        private void FixedUpdate()
        {
            if (!ENetClient.IsConnected())
                return;

            SceneManager.LoadScene("Account Management");
        }

        private IEnumerator AnimateDots()
        {
            int i = 0;
            while (!ENetClient.IsConnected())
            {
                // Animate connecting text
                Text.text = message + dots[i++ % dots.Length];
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}

