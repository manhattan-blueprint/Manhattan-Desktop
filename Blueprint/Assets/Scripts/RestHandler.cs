using System;
using System.IO;
using System.Net;
using System.Security.Authentication;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

public class RestHandler {
    private string _base_url;
    private string _password_regex = "(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{5,16}";

    private bool check_password_valid(string password) {
        Regex rgx = new Regex(_password_regex);
        return rgx.IsMatch(password);
    }

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

    public UserCredentials AuthenticateUser(UserCredentials user) {
        //prepare JSON payload & local variables
        string json = JsonUtility.ToJson(new PayloadAuthenticate(user));
        UserCredentials output = null;

        //attempt user authentication
        try {
            string response = performPOST(":8000/api/v1/authenticate", json);
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
        if (!check_password_valid(password)) {
            throw new InvalidCredentialException("Password not valid.");
        }
        
        //prepare JSON payload & local variables
        string json = JsonUtility.ToJson(new PayloadAuthenticate(username, password));
        UserCredentials output = null;
        
        //attempt user registration
        try {
            string response = performPOST(":8000/api/v1/authenticate/register", json);
            ResponseAuthenticate tokens = JsonUtility.FromJson<ResponseAuthenticate>(response);
        
            output = new UserCredentials(username, password, tokens.access, tokens.refresh);
        }
        catch (WebException e) {
            var response_detailed = e.Response as HttpWebResponse;
            int http_status = (int)response_detailed.StatusCode;

            //bad request
            if (http_status == 400) {
                Debug.Log(response_detailed.StatusDescription);
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