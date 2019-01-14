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
    private const string defaultBaseUrl        = "http://smithwjv.ddns.net";
    
    // Enum
    public enum httpResponseCode {
        ok = 200,
        badRequest = 400,
        unauthorised = 401
    };

    private BlueprintAPI(string baseUrl) {
        this.rs = new RestHandler(baseUrl);
    }

    public static BlueprintAPI WithBaseUrl(string baseUrl) {
        return new BlueprintAPI(baseUrl);
    }
    
    public static BlueprintAPI DefaultCredentials() {
        return new BlueprintAPI(defaultBaseUrl);
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

    public async Task<ResponseAuthenticate> AsyncRefreshTokens(UserCredentials user) {
        // Prepare JSON payload & local variables
        string json = JsonUtility.ToJson(new RefreshPayload(user.getRefreshToken()));
        
        // Fetch
        string response = await rs.PerformAsyncPost(refreshEndpoint, json);
        
        // Extract tokens from JSON
        ResponseAuthenticate tokens = JsonUtility.FromJson<ResponseAuthenticate>(response);
        
        return tokens;
    }
    
    public async Task<ResponseGetInventory> AsyncGetInventory(UserCredentials user) {
        string response = null;
        
        // Fetch
        try {
            response = await rs.PerformAsyncGet(inventoryEndpoint, user.getAccessToken());
        }
        catch (WebException e) when (RetrieveHTTPCode(e) == (int)httpResponseCode.unauthorised) {
            // if access token doesn't match a user, refresh tokens and retry
            ResponseAuthenticate refreshedTokens = await AsyncRefreshTokens(user);

            response = await rs.PerformAsyncGet(inventoryEndpoint, refreshedTokens.access);
        }

        // Serialize JSON
        ResponseGetInventory inventory = JsonUtility.FromJson<ResponseGetInventory>(response);

        return inventory;
    }
    
    public async Task<string> AsyncAddToInventory(UserCredentials user, ResponseGetInventory items) {
        string response = null;
        
        // Prepare JSON payload
        string json = JsonUtility.ToJson(items);
        
        // Send payload
        try {
            response = await rs.PerformAsyncPost(inventoryEndpoint, json, user.getAccessToken());
        }
        catch (WebException e) when (RetrieveHTTPCode(e) == (int) httpResponseCode.unauthorised) {
            // if access token doesn't match a user, refresh tokens and retry
            ResponseAuthenticate refreshedTokens = await AsyncRefreshTokens(user);
            
            response = await rs.PerformAsyncPost(inventoryEndpoint, json, refreshedTokens.access);
        }

        return json;
    }
    
    public async Task<string> AsyncDeleteInventory(UserCredentials user) {
        string response = null;
        
        // Perform request
        try {
            response = await rs.PerformAsyncDelete(inventoryEndpoint, user.getAccessToken());
        }
        catch (WebException e) when (RetrieveHTTPCode(e) == (int)httpResponseCode.unauthorised) {
            // if access token doesn't match a user, refresh tokens and retry
            ResponseAuthenticate refreshedTokens = await AsyncRefreshTokens(user);
            
            response = await rs.PerformAsyncDelete(inventoryEndpoint, refreshedTokens.access);
        }

        return response;
    }

    private class RefreshPayload {
        public string refresh;

        public RefreshPayload(string refresh) {
            this.refresh = refresh;
        }
    }
}