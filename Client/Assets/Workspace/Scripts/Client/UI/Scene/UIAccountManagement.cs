using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

using TMPro;

namespace Valk.Networking
{
    public class UIAccountManagement : MonoBehaviour
    {
        // Regions
        public GameObject goDropdownRegions;
        private TMP_Dropdown dropdownRegions;

        // Create Account
        public GameObject goInputFieldCreateName, goInputFieldCreatePass;
        private TMP_InputField inputFieldCreateName, inputFieldCreatePass;

        // Login Account
        public GameObject goInputFieldLoginName, goInputFieldLoginPass;
        private TMP_InputField inputFieldLoginName, inputFieldLoginPass;

        // Text
        public GameObject goTextMessage;
        private static TMP_Text textMessage;

        private Coroutine animateDots;

        private bool sendingRequest;
        public static bool ConnectingENet;

        private void Start()
        {
            dropdownRegions = goDropdownRegions.GetComponent<TMP_Dropdown>();

            inputFieldCreateName = goInputFieldCreateName.GetComponent<TMP_InputField>();
            inputFieldCreatePass = goInputFieldCreatePass.GetComponent<TMP_InputField>();

            inputFieldLoginName = goInputFieldLoginName.GetComponent<TMP_InputField>();
            inputFieldLoginPass = goInputFieldLoginPass.GetComponent<TMP_InputField>();

            textMessage = goTextMessage.GetComponent<TMP_Text>();
        }

        public static void UpdateText(string text)
        {
            textMessage.text = text;
        }

        public async void CreateAccount()
        {
            if (sendingRequest || ConnectingENet)
            {
                return;
            }

            if (inputFieldCreateName.text.Equals(""))
            {
                UpdateText("Please enter a username for registration..");
                return;
            }

            if (inputFieldCreatePass.text.Equals(""))
            {
                UpdateText("Please enter a password for registration..");
                return;
            }

            //Network.Send(PacketType.ClientCreateAccount, PacketFlags.Reliable, inputFieldCreateName.text, inputFieldCreatePass.text);

            string name = inputFieldCreateName.text;
            string pass = inputFieldCreatePass.text;

            if (name.Length < 3 || name.Length > 15)
            {
                UpdateText("Account name must be between 3 and 15 characters");
                return;
            }

            if (pass.Length < 5 || pass.Length > 30)
            {
                UpdateText("Account password must be between 5 and 30 characters");
                return;
            }

            animateDots = StartCoroutine(AnimateDots("Sending request to web server"));
            sendingRequest = true;

            WebUser user = new WebUser();
            user.Name = name;
            user.Password = pass;
            string data = await WebServer.Post("/api/register", user);
            WebResponse response = JsonConvert.DeserializeObject<WebResponse>(data);

            // Closed the client while awaiting for a response
            if (this == null) 
            {
                return;
            }

            StopCoroutine(animateDots);
            sendingRequest = false;

            if (response.Error == 1)
            {
                UpdateText("An error occured while sending the request");
                return;
            }

            StatusCode code = (StatusCode)response.Status;
            if (code == StatusCode.REGISTER_ACCOUNT_ALREADY_EXISTS)
            {
                UpdateText($"The account '{name}' already exists");
                return;
            }

            if (code == StatusCode.REGISTER_USERNAME_INVALID)
            {
                UpdateText("Invalid username. Account Creation failed");
                return;
            }

            if (code == StatusCode.REGISTER_PASSWORD_INVALID)
            {
                UpdateText("Invalid password. Account Creation failed");
                return;
            }

            if (code == StatusCode.REGISTER_SUCCESS)
            {
                UpdateText($"Registered account '{name}' successfully");
            }
        }

        public async void Connect()
        {
            // LOGIN
            if (sendingRequest || ConnectingENet)
            {
                return;
            }

            if (inputFieldLoginName.text.Equals(""))
            {
                UpdateText("Please enter a username for login..");
                return;
            }

            if (inputFieldLoginPass.text.Equals(""))
            {
                UpdateText("Please enter a password for login..");
                return;
            }

            string name = inputFieldLoginName.text;
            string pass = inputFieldLoginPass.text;

            animateDots = StartCoroutine(AnimateDots("Sending request to web server"));
            sendingRequest = true;

            //Network.Send(PacketType.ClientLoginAccount, PacketFlags.Reliable, inputFieldLoginName.text, inputFieldLoginPass.text);
            WebUser user = new WebUser();
            user.Name = name;
            user.Password = pass;
            string data = await WebServer.Post("/api/login", user);
            WebResponse response = JsonConvert.DeserializeObject<WebResponse>(data);

            // Closed the client while awaiting for a response
            if (this == null) 
            {
                return;
            }

            StopCoroutine(animateDots);
            sendingRequest = false;

            if (response.Error == 1)
            {
                UpdateText("An error occured while sending the request");
                return;
            }

            StatusCode code = (StatusCode)response.Status;
            if (code == StatusCode.LOGIN_DOESNT_EXIST)
            {
                UpdateText($"Login for account '{name}' does not exist");
                return;
            }

            if (code == StatusCode.LOGIN_WRONG_PASSWORD)
            {
                UpdateText($"Failed to login to account '{name}', wrong password");
                return;
            }

            if (code == StatusCode.LOGIN_SUCCESS)
            {
                UpdateText($"Successfully logged into account '{name}'");
                //SceneManager.LoadScene("Account");
                //StartCoroutine(ASyncLoadGame());
            }

            // CONNECT
            string IP = "127.0.0.1";

            int index = dropdownRegions.value;
            if (index == 0) // Auto
            {
                //TODO
            }

            if (index == 1) // Canada
            {
                IP = "142.160.70.176";
            }

            animateDots = StartCoroutine(AnimateDots("Attempting to connect to ENet server"));

            ENetClient.Connect(IP);
            ConnectingENet = true;
            StartCoroutine(ENetConnect());
        }

        public IEnumerator ASyncLoadGame()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Main");
            while (!asyncLoad.isDone)
            {
                yield return null;
                // Scene done loading, we could do extra things here if needed
            }
        }

        private IEnumerator ENetConnect() 
        {
            while (!ENetClient.IsConnected()) 
            {
                if (!ConnectingENet) 
                {
                    StopCoroutine(animateDots);
                    yield break;
                }

                yield return new WaitForSeconds(0.1f);
            }

            StopCoroutine(animateDots);
            SceneManager.LoadScene("Account");
        }

        private IEnumerator AnimateDots(string message)
        {
            string[] dots = new string[] { "", ".", "..", "..." };

            int i = 0;
            while (true)
            {
                // Animate connecting text
                UpdateText(message + dots[i++ % dots.Length]);
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}