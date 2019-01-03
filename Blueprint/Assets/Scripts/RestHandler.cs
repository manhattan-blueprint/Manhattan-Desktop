using System;
using System.IO;
using System.Net;
using System.Security.Authentication;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class RestHandler {
    private string baseUrl;
    private string passwordRegex = "(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{5,16}";
    
    // Constructor
    public RestHandler(string baseUrl) {
        this.baseUrl = baseUrl;
    }

    public bool checkPasswordValid(string password) {
        Regex rgx = new Regex(passwordRegex);
        return rgx.IsMatch(password);
    }

    public string PerformGET(string endpoint) {
        string requestUrl = string.Concat(baseUrl, endpoint);
        HttpWebRequest request = (HttpWebRequest) WebRequest.Create(String.Format(requestUrl));

        HttpWebResponse response = (HttpWebResponse) request.GetResponse();
        Stream stream = response.GetResponseStream();
        StreamReader reader = new StreamReader(stream);

        string strResponse = reader.ReadToEnd();

        reader.Close();
        response.Close();
        return strResponse;
    }

    public string PerformPOST(string endpoint, string postData) {
        string requestUrl = string.Concat(baseUrl, endpoint);
        HttpWebRequest request = (HttpWebRequest) WebRequest.Create(requestUrl);

        var data = Encoding.ASCII.GetBytes(postData);

        request.Method = "POST";
        request.ContentType = "application/json";
        request.ContentLength = data.Length;

        using (var stream = request.GetRequestStream()) {
            stream.Write(data, 0, data.Length);
        }

        HttpWebResponse response = (HttpWebResponse) request.GetResponse();
        string strResponse = new StreamReader(response.GetResponseStream()).ReadToEnd();

        return strResponse;
    }

    
}
