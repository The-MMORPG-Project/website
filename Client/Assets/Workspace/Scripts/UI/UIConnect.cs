using UnityEngine;
using UnityEngine.SceneManagement;

using TMPro;

namespace Valk.Networking
{
    public class UIConnect : MonoBehaviour
    {
        private const string DEFAULT_IP = "127.0.0.1";

        public GameObject GoInputField;
        private TMP_InputField InputField;

        private void Start()
        {
            InputField = GoInputField.GetComponent<TMP_InputField>();
            InputField.text = DEFAULT_IP;
        }

        public void Connect()
        {
            if (InputField.text == "")
                return;

            Client.Connect(InputField.text);
            SceneManager.LoadScene("Connecting");
        }
    }
}
