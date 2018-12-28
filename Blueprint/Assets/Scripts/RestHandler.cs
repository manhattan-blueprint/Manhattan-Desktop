using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

public class RestHandler {
    private string _base_url;

    public RestHandler(string baseUrl) {
        _base_url = baseUrl;
    }

    private string GetBaseURL() {
        return _base_url;
    }

    public string performGET(string endpoint) {
        string request_url = string.Concat(GetBaseURL(), endpoint);
        HttpWebRequest request = (HttpWebRequest) WebRequest.Create(String.Format(request_url));

        HttpWebResponse response = (HttpWebResponse) request.GetResponse();
        Stream stream = response.GetResponseStream();
        StreamReader reader = new StreamReader(stream);

        string str_response = reader.ReadToEnd();

        reader.Close();
        response.Close();
        return str_response;
    }


    public string performPOST(string endpoint, string post_data) {
        string request_url = string.Concat(GetBaseURL(), endpoint);
        HttpWebRequest request = (HttpWebRequest) WebRequest.Create(request_url);

        var data = Encoding.ASCII.GetBytes(post_data);

        request.Method = "POST";
        request.ContentType = "application/json";
        request.ContentLength = data.Length;

        using (var stream = request.GetRequestStream()) {
            stream.Write(data, 0, data.Length);
        }

        HttpWebResponse response = (HttpWebResponse) request.GetResponse();
        string str_response = new StreamReader(response.GetResponseStream()).ReadToEnd();

        return str_response;
    }

    //returns refresh and access tokens
    public UserCredentials AuthenticateUser(UserCredentials user, string endpoint) {
        string json = JsonUtility.ToJson(new PayloadAuthenticate(user));
        //string response = performPOST("/api/v1/authenticate", json);
        string response = performPOST(endpoint, json);
        ResponseAuthenticate tokens = JsonUtility.FromJson<ResponseAuthenticate>(response);

        UserCredentials output =
            new UserCredentials(user.getUsername(), user.getPassword(), tokens.access, tokens.refresh);
        
        return output;
    }
}


//payload classes
[Serializable]
public class PayloadAuthenticate {
    public string username;
    public string password;

    public PayloadAuthenticate(UserCredentials user) {
        this.username = user.getUsername();
        this.password = user.getPassword();
    }
}

[Serializable]
public class ResponseAuthenticate {
    public string access;
    public string refresh;
}