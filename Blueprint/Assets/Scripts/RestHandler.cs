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

    private bool checkPasswordValid(string password) {
        Regex rgx = new Regex(passwordRegex);
        return rgx.IsMatch(password);
    }

    public RestHandler(string baseUrl) {
        this.baseUrl = baseUrl;
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

    public UserCredentials AuthenticateUser(UserCredentials user) {
        //prepare JSON payload & local variables
        string json = JsonUtility.ToJson(new PayloadAuthenticate(user));
        UserCredentials output = null;

        //attempt user authentication
        try {
            string response = PerformPOST(":8000/api/v1/authenticate", json);
            ResponseAuthenticate tokens = JsonUtility.FromJson<ResponseAuthenticate>(response);

            output =
                new UserCredentials(user.getUsername(), user.getPassword(), tokens.access, tokens.refresh);
        }
        catch (WebException e) {
            Debug.Log(user.getUsername() + " | " + e.Message);
            
            //return user without tokens in Exception instance
            output = new UserCredentials(user.getUsername(), user.getPassword(), null, null);
        }
        
        return output;
    }
    
    public UserCredentials RegisterUser(string username, string password) {
        //check validity of password
        if (!checkPasswordValid(password)) {
            throw new InvalidCredentialException("Password not valid.");
        }
        
        //prepare JSON payload & local variables
        string json = JsonUtility.ToJson(new PayloadAuthenticate(username, password));
        UserCredentials output = null;
        
        //attempt user registration
        try {
            string response = PerformPOST(":8000/api/v1/authenticate/register", json);
            ResponseAuthenticate tokens = JsonUtility.FromJson<ResponseAuthenticate>(response);
        
            output = new UserCredentials(username, password, tokens.access, tokens.refresh);
        }
        catch (WebException e) {
            var responseDetailed = e.Response as HttpWebResponse;
            int httpStatus = (int)responseDetailed.StatusCode;

            //bad request
            if (httpStatus == 400) {
                Debug.Log(responseDetailed.StatusDescription);
            }
            
            Debug.Log(username + " | " + e.Message);
            
            //returns user without tokens in exception instance
            output = new UserCredentials(username, password, null, null);
        }

        return output;
    }
}

//payload classes (DAOs)
[Serializable]
public class PayloadAuthenticate {
    public string username;
    public string password;

    public PayloadAuthenticate(UserCredentials user) {
        this.username = user.getUsername();
        this.password = user.getPassword();
    }

    public PayloadAuthenticate(string username, string password) {
        this.username = username;
        this.password = password;
    }
}

[Serializable]
public class ResponseAuthenticate {
    public string access;
    public string refresh;
}