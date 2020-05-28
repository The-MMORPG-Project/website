using System;
using System.Collections;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public enum StatusCode
{
    REGISTER_SUCCESS,
    REGISTER_ACCOUNT_ALREADY_EXISTS,
    REGISTER_PASSWORD_INVALID,
    REGISTER_USERNAME_INVALID,
    LOGIN_DOESNT_EXIST,
    LOGIN_WRONG_PASSWORD,
    LOGIN_SUCCESS
}

public class WebServer : MonoBehaviour
{
    public static bool Connected = false;

    private const string URL = "http://localhost:3000";
    private static HttpClient http = new HttpClient();

    public static async Task Status()
    {
        int requestsCount = 15;
        for (int i = 0; i < requestsCount; i++)
        {
            var response = await http.GetAsync(URL);
            Debug.Log($"API responded with: {response.StatusCode}");
            string responseBody = await response.Content.ReadAsStringAsync();
            //Tester t = JsonConvert.DeserializeObject<Tester>(responseBody);
            //Debug.Log(t.data);
        }
    }

    public static async Task Fetch(string path)
    {
        var response = await http.GetAsync(URL);
        Debug.Log($"API responded with: {response.StatusCode}");
        string responseBody = await response.Content.ReadAsStringAsync();
    }

    public static async Task<string> Post(string path, object obj)
    {
        try
        {
            var json = JsonConvert.SerializeObject(obj);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await http.PostAsync(URL + path, content);
            string result = await response.Content.ReadAsStringAsync();
            return result;
        }
        catch (HttpRequestException)
        {
            // Web server must be down
            return "{ Error: 1 }";
        }
    }
}
