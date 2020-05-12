using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

using ENet;

namespace Valk.Networking
{
    public class UIAccountManagement : MonoBehaviour
    {
        // Create Account
        public GameObject goInputFieldCreateName;
        public GameObject goInputFieldCreatePass;
        private TMP_InputField inputFieldCreateName;
        private TMP_InputField inputFieldCreatePass;

        // Login Account
        public GameObject goInputFieldLoginName;
        public GameObject goInputFieldLoginPass;
        private TMP_InputField inputFieldLoginName;
        private TMP_InputField inputFieldLoginPass;

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

        public void CreateAccount()
        {
            if (inputFieldCreateName.text.Equals(""))
                return;

            if (inputFieldCreatePass.text.Equals(""))
                return;

            Network.Send(PacketType.ClientCreateAccount, PacketFlags.Reliable, inputFieldCreateName.text, inputFieldCreatePass.text);
        }

        public void Login()
        {
            if (inputFieldLoginName.text.Equals(""))
                return;

            if (inputFieldLoginPass.text.Equals(""))
                return;

            Network.Send(PacketType.ClientLoginAccount, PacketFlags.Reliable, inputFieldLoginName.text, inputFieldLoginPass.text);
        }
    }
}