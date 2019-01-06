using System.Net;
using System.Security.Authentication;
using UnityEngine;
using System.Threading.Tasks;

public class BlueprintAPI {
    private RestHandler rs;

    // Const
    private const string authenticateEndpoint = ":8000/api/v1/authenticate";
    private const string registerEndpoint     = ":8000/api/v1/authenticate/register";
    
    // Enum
    public enum httpResponseCode {
        ok = 200,
        badRequest = 400,
        unauthorised = 401
    };

    // Constructor
    public BlueprintAPI(string baseUrl) {
        this.rs = new RestHandler(baseUrl);
    }

    public int RetrieveHTTPCode(WebException e) {
        var responseDetailed = e.Response as HttpWebResponse;
        int httpStatus = (int)responseDetailed.StatusCode;
        return httpStatus;
    }

    private void validateUsernamePassword(string username, string password) {
        // Check validity of password
        if (!rs.CheckPasswordValid(password)) {
            throw new InvalidCredentialException("Password invalid");
        }
        
        // Check validity of username
        if (username.Length >= 16) {
            throw new InvalidCredentialException("Username invalid, must have less than 16 characters");
        }
    }
    
    public async Task<UserCredentials> AsyncAuthenticateUser(UserCredentials user) {
        validateUsernamePassword(user.getUsername(), user.getPassword());
        
        // Prepare JSON payload & local variables
        string json = JsonUtility.ToJson(new PayloadAuthenticate(user));
        
        // Fetch
        string response = await rs.PerformAsyncPost(authenticateEndpoint, json);
        
        // Extract tokens from JSON
        ResponseAuthenticate tokens = JsonUtility.FromJson<ResponseAuthenticate>(response);
        
        // Form and return UserCredentials object
        return new UserCredentials(user.getUsername(), user.getPassword(), tokens.access, tokens.refresh);
    }
    
    
    public async Task<UserCredentials> AsyncRegisterUser(string username, string password) {
        validateUsernamePassword(username, password);
        
        // Prepare JSON payload & local variables
        string json = JsonUtility.ToJson(new PayloadAuthenticate(username, password));
        
        // Fetch
        string response = await rs.PerformAsyncPost(registerEndpoint, json);
        
        // Extract tokens from JSON
        ResponseAuthenticate tokens = JsonUtility.FromJson<ResponseAuthenticate>(response);
        
        // Form and return UserCredentials object
        return new UserCredentials(username, password, tokens.access, tokens.refresh);
    }
    
    //TODO: Inventory, GetAll
    //TODO: Inventory, Add
    //TODO: Inventory, Delete
    
    //TODO: Resources, Get
    //^ Probably not necessary
    
}