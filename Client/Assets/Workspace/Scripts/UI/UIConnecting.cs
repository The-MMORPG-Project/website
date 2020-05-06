using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using TMPro;

using Unity.Networking.Transport;

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
        StartCoroutine(CheckConnection());
    }

    IEnumerator CheckConnection()
    {
        int i = 0;
        while (!Client.IsConnected())
        {
            // Animate connecting text
            Text.text = message + dots[i % dots.Length];
            i++;
            yield return new WaitForSeconds(0.5f);
        }

        SceneManager.LoadScene("Create Account");
    }
}
