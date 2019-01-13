using System;
using System.Net;
using System.Security.Authentication;
using UnityEngine;
using System.Threading.Tasks;

public class BlueprintAPI {
    private RestHandler rs;

    // Const
    private const string authenticateEndpoint  = ":8000/api/v1/authenticate";
    private const string registerEndpoint      = ":8000/api/v1/authenticate/register";
    private const string refreshEndpoint       = ":8000/api/v1/authenticate/refresh";
    private const string inventoryEndpoint     = ":8001/api/v1/inventory";
    private const string itemSchemaEndpoint    = ":8000/api/v1/item-schema";
    
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

    public async Task<ResponseAuthenticate> AsyncRefreshTokens(string refreshToken) {
        // Prepare JSON payload & local variables
        string json = JsonUtility.ToJson(new RefreshPayload(refreshToken));
        
        // Fetch
        string response = await rs.PerformAsyncPost(refreshEndpoint, json);
        
        // Extract tokens from JSON
        ResponseAuthenticate tokens = JsonUtility.FromJson<ResponseAuthenticate>(response);
        
        return tokens;
    }
    
    public async Task<ResponseGetInventory> AsyncGetInventory(string accessToken) {
        // Fetch
        string response = await rs.PerformAsyncGet(inventoryEndpoint, accessToken);

        // Serialize JSON
        ResponseGetInventory inventory = JsonUtility.FromJson<ResponseGetInventory>(response);

        return inventory;
    }
    
    public async Task<string> AsyncAddToInventory(string accessToken, ResponseGetInventory items) {
        // Prepare JSON payload
        string json = JsonUtility.ToJson(items);
        
        // Send payload
        string response = await rs.PerformAsyncPost(inventoryEndpoint, json, accessToken);

        return json;
    }
    
    public async Task<string> AsyncDeleteInventory(string accessToken) {
        // Perform request
        string response = await rs.PerformAsyncDelete(inventoryEndpoint, accessToken);

        return response;
    }

    public async Task<string> AsyncGetItemSchema() {
       //Peform request
        string response = await rs.PerformAsyncGet(itemSchemaEndpoint);

        return response;
    }

    private class RefreshPayload {
        public string refresh;

        public RefreshPayload(string refresh) {
            this.refresh = refresh;
        }
    }
}