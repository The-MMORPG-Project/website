using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

using TMPro;

using ENet;

namespace Valk.Networking
{
    public class UIAccountManagement : MonoBehaviour
    {
        // Create Account
        public GameObject goInputFieldCreateName, goInputFieldCreatePass;
        private TMP_InputField inputFieldCreateName, inputFieldCreatePass;

        // Login Account
        public GameObject goInputFieldLoginName, goInputFieldLoginPass;
        private TMP_InputField inputFieldLoginName, inputFieldLoginPass;

        // Text
        public GameObject goTextMessage;
        private static TMP_Text textMessage;

        void Start()
        {
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
            if (inputFieldCreateName.text.Equals(""))
                return;

            if (inputFieldCreatePass.text.Equals(""))
                return;

            //Network.Send(PacketType.ClientCreateAccount, PacketFlags.Reliable, inputFieldCreateName.text, inputFieldCreatePass.text);
            
            string name = inputFieldCreateName.text;
            string pass = inputFieldCreatePass.text;

            if (name.Length < 3 || name.Length > 15) 
            {
                UpdateText("Account name must be between 3 and 15 characters.");
                return;
            }

            if (pass.Length < 5 || pass.Length > 30)
            {
                UpdateText("Account password must be between 5 and 30 characters.");
                return;
            }

            UpdateText("..."); // Animate these dots later on to indicate we are sending request to server...
            
            User user = new User();
            user.Name = inputFieldCreateName.text;
            user.Password = inputFieldCreatePass.text;
            string data = await WebServer.Post("/api/register", user);
            Response response = JsonConvert.DeserializeObject<Response>(data);
            StatusCode code = (StatusCode)response.Status;
            if (code == StatusCode.REGISTER_ACCOUNT_ALREADY_EXISTS) 
            {
                UpdateText($"The account '{name}' already exists.");
            }

            if (code == StatusCode.REGISTER_USERNAME_INVALID) 
            {
                UpdateText("Invalid username. Account Creation failed.");
            }

            if (code == StatusCode.REGISTER_PASSWORD_INVALID) 
            {
                UpdateText("Invalid password. Account Creation failed.");
            }

            if (code == StatusCode.REGISTER_SUCCESS) 
            {
                UpdateText($"Registered account '{name}' successfully.");
            }
        }

        public async void Login()
        {
            if (inputFieldLoginName.text.Equals(""))
                return;

            if (inputFieldLoginPass.text.Equals(""))
                return;

            //Network.Send(PacketType.ClientLoginAccount, PacketFlags.Reliable, inputFieldLoginName.text, inputFieldLoginPass.text);
            User user = new User();
            user.Name = inputFieldLoginName.text;
            user.Password = inputFieldLoginPass.text;
            string response = await WebServer.Post("/api/login", user);
            Debug.Log(response);
        }
    }
}