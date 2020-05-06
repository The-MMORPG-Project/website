using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class UICreateAccount : MonoBehaviour
{
    public GameObject GoInput;
    private TMP_InputField InputField;

    void Start() 
    {
        InputField = GoInput.GetComponent<TMP_InputField>();
    }

    public void Login() 
    {
        if (!InputField.text.Equals(""))
            Client.CreateAccount(InputField.text);
    }
}
