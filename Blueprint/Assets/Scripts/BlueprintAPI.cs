using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Security.Authentication;
using UnityEngine;

public class BlueprintAPI {
    private string baseUrl;
    private RestHandler rs;

    public BlueprintAPI(string baseUrl) {
        this.baseUrl = baseUrl;
        this.rs = new RestHandler(baseUrl);
    }

    public UserCredentials AuthenticateUser(UserCredentials user) {
        // Prepare JSON payload & local variables
        string json = JsonUtility.ToJson(new PayloadAuthenticate(user));
        UserCredentials output = null;

        // Attempt user authentication
        try {
            string response = rs.PerformPOST(":8000/api/v1/authenticate", json);
            ResponseAuthenticate tokens = JsonUtility.FromJson<ResponseAuthenticate>(response);

            output =
                new UserCredentials(user.getUsername(), user.getPassword(), tokens.access, tokens.refresh);
        }
        catch (WebException e) {
            Debug.Log(user.getUsername() + " | " + e.Message);
            
            // Return user without tokens in Exception instance
            output = new UserCredentials(user.getUsername(), user.getPassword(), null, null);
        }
        
        return output;
    }
    
    public UserCredentials RegisterUser(string username, string password) {
        // Check validity of password
        if (!rs.checkPasswordValid(password)) {
            throw new InvalidCredentialException("Password not valid.");
        }
        
        // Prepare JSON payload & local variables
        string json = JsonUtility.ToJson(new PayloadAuthenticate(username, password));
        UserCredentials output = null;
        
        // Attempt user registration
        try {
            string response = rs.PerformPOST(":8000/api/v1/authenticate/register", json);
            ResponseAuthenticate tokens = JsonUtility.FromJson<ResponseAuthenticate>(response);
        
            output = new UserCredentials(username, password, tokens.access, tokens.refresh);
        }
        catch (WebException e) {
            var responseDetailed = e.Response as HttpWebResponse;
            int httpStatus = (int)responseDetailed.StatusCode;

            // Bad request
            if (httpStatus == 400) {
                Debug.Log(responseDetailed.StatusDescription);
            }
            
            Debug.Log(username + " | " + e.Message);
            
            // Returns user without tokens in exception instance
            output = new UserCredentials(username, password, null, null);
        }

        return output;
    }
}
