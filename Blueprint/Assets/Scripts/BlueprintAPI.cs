using System.Net;
using System.Security.Authentication;
using UnityEngine;

public class BlueprintAPI {
    private string baseUrl;
    private RestHandler rs;

    // Const
    private const string authenticateEndpoint = ":8000/api/v1/authenticate";
    private const string registerEndpoint     = ":8000/api/v1/authenticate/register";
    
    // Enum
    private enum httpResponseCode {
        ok = 200,
        badRequest = 400,
        unauthorised = 401
    };

    public BlueprintAPI(string baseUrl) {
        this.baseUrl = baseUrl;
        this.rs = new RestHandler(baseUrl);
    }

    public UserCredentials AuthenticateUser(UserCredentials user) {
        // Check validity of password
        if (!rs.checkPasswordValid(user.getPassword())) {
            throw new InvalidCredentialException("Password invalid");
        }
        
        // Check validity of username
        if (user.getUsername().Length >= 16) {
            throw new InvalidCredentialException("Username invalid, must have less than 16 characters");
        }
        
        // Prepare JSON payload & local variables
        string json = JsonUtility.ToJson(new PayloadAuthenticate(user));
        UserCredentials output = null;

        // Attempt user authentication
        try {
            string response = rs.PerformPOST(authenticateEndpoint, json);
            ResponseAuthenticate tokens = JsonUtility.FromJson<ResponseAuthenticate>(response);

            output =
                new UserCredentials(user.getUsername(), user.getPassword(), tokens.access, tokens.refresh);
        }
        catch (WebException e) {
            var responseDetailed = e.Response as HttpWebResponse;
            int httpStatus = (int)responseDetailed.StatusCode;

            switch (httpStatus) {
                case 400:
                    throw new InvalidCredentialException("Invalid username or password");
                    break;
                case 401:
                    throw new InvalidCredentialException("The credentials provided do not match any user");
                    break;
            }
            
            // Return user without tokens in Exception instance
            output = new UserCredentials(user.getUsername(), user.getPassword(), null, null);
        }
        
        return output;
    }
    
    public UserCredentials RegisterUser(string username, string password) {
        // Check validity of password
        if (!rs.checkPasswordValid(password)) {
            throw new InvalidCredentialException("Password invalid");
        }
        
        // Check validity of username
        if (username.Length >= 16) {
            throw new InvalidCredentialException("Username invalid, must have less than 16 characters");
        }
        
        // Prepare JSON payload & local variables
        string json = JsonUtility.ToJson(new PayloadAuthenticate(username, password));
        UserCredentials output = null;
        
        // Attempt user registration
        try {
            string response = rs.PerformPOST(registerEndpoint, json);
            ResponseAuthenticate tokens = JsonUtility.FromJson<ResponseAuthenticate>(response);
        
            output = new UserCredentials(username, password, tokens.access, tokens.refresh);
        }
        catch (WebException e) {
            var responseDetailed = e.Response as HttpWebResponse;
            int httpStatus = (int)responseDetailed.StatusCode;

            // Bad request
            if (httpStatus == (int)httpResponseCode.badRequest) {
                throw new InvalidCredentialException("Username already exists");
            }
            
            // Returns user without tokens in exception instance
            output = new UserCredentials(username, password, null, null);
        }

        return output;
    }
}
