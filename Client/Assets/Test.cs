using System;
using System.Threading;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft;
using Newtonsoft.Json;

public class Test : MonoBehaviour
{
    private static string _url = "http://localhost:3000";
    public static int RequestsCount = 15;
    private static HttpClient _http = new HttpClient();

    async void Start()
    {
        //await Fetch();
        await Post();
    }

    async Task Fetch()
    {
        for (int i = 0; i < RequestsCount; i++)
        {
            var response = await _http.GetAsync(_url);
            Debug.Log($"API responded with: {response.StatusCode}");
            string responseBody = await response.Content.ReadAsStringAsync();
            //Tester t = JsonConvert.DeserializeObject<Tester>(responseBody);
            //Debug.Log(t.data);
        }
    }

    async Task Post()
    {
        var user = new User();
        user.Name = "Bob Marley";

        var json = JsonConvert.SerializeObject(user);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _http.PostAsync(_url + "/api/login", content);

        string result = response.Content.ReadAsStringAsync().Result;
        Debug.Log(result);
    }
}
