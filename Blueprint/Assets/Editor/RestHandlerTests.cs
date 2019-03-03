using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Authentication;
using System.Threading.Tasks;
using UnityEngine;
using NUnit.Framework;
using Random = System.Random;
using Service;
using Service.Response;

public class RestHandlerTests {
    private string baseUrl = "http://smithwjv.ddns.net";
    private UserCredentials validUser = new UserCredentials("testplayer", "Player123");
    private UserCredentials validDev = new UserCredentials("testdev", "Dev123");
    private UserCredentials validLec = new UserCredentials("testlecturer", "Lecturer123");
    
    // GETs data from jsonplaceholder, asserts it is correct
    [Test]
    public void TestPerformGET() {
        RestHandler restHandler = new RestHandler("http://jsonplaceholder.typicode.com");

        string response = restHandler.PerformGET("/posts/1");

        JsonPlaceholder inv = JsonUtility.FromJson<JsonPlaceholder>(response);

        Assert.That(inv.userId, Is.EqualTo(1));
        Assert.That(inv.id, Is.EqualTo(1));
        Assert.That(inv.title,
            Is.EqualTo("sunt aut facere repellat provident occaecati excepturi optio reprehenderit"));
        Assert.That(inv.body, Is.EqualTo("quia et suscipit\nsuscipit recusandae consequuntur expedita et " +
                                         "cum\nreprehenderit molestiae ut ut quas totam\nnostrum rerum est " +
                                         "autem sunt rem eveniet architecto"));
    }

    // POSTs data to jsonplaceholder, checks that response is correct
    [Test]
    public void TestPerformAsyncPOST() {
        RestHandler restHandler = new RestHandler("http://jsonplaceholder.typicode.com");

        // Setup
        JsonPost data = new JsonPost();
        data.value = "hello";
        string payload = JsonUtility.ToJson(data);
        JsonPost responseJson= new JsonPost();

        // Response
        Task.Run(async () => {
            string response = await restHandler.PerformAsyncPost("/posts", payload);
            responseJson = JsonUtility.FromJson<JsonPost>(response);
        }).GetAwaiter().GetResult();

        Assert.That(responseJson.value, Is.EqualTo("hello"));
    }
   
    // Authenticates user with valid credentials, who is present in the db
    // Is an example of HTTP error handling   
    [Test]
    public void TestValidAuthenticateUser() {
        BlueprintAPI blueprintApi = BlueprintAPI.WithBaseUrl(baseUrl);
        UserCredentials user = validUser;
        UserCredentials returnUser = null; 
        
        Task.Run(async () => {
            Task<APIResult<UserCredentials, JsonError>> fetchingResponse = blueprintApi.AsyncAuthenticateUser(user);
            
            APIResult<UserCredentials, JsonError> response = await fetchingResponse;

            if (response.isSuccess()) {
                // Success case
                returnUser = response.GetSuccess();
            } else {
                //error case
            }

        }).GetAwaiter().GetResult();
        
        // Check returned user is correct and contains access tokens
        Assert.That(returnUser.GetUsername(), Is.EqualTo(validUser.GetUsername()));
        Assert.That(returnUser.GetPassword(), Is.EqualTo(validUser.GetPassword()));    
        Assert.IsNotNull(returnUser.GetAccessToken());
        Assert.IsNotNull(returnUser.GetRefreshToken());
    }
    
    // Attempts to authenticate user with invalid password
    // Will catch InvalidCredentialException thrown by AsyncAuthenticateUser
    [Test]
    public void TestInvalidAuthenticateUser() {
        BlueprintAPI blueprintApi = BlueprintAPI.WithBaseUrl(baseUrl);
        UserCredentials user = new UserCredentials("test_invalid", "Invalid123");
        
        Task.Run(async () => {
            Task<APIResult<UserCredentials, JsonError>> fetchingResponse = blueprintApi.AsyncAuthenticateUser(user);
            
            APIResult<UserCredentials, JsonError> response = await fetchingResponse;

            if (!response.isSuccess()) {
                // Error case
                JsonError error = response.GetError();
                
                Assert.That(error.error, Is.EqualTo("The credentials provided do not match any user"));
            } else {
                Assert.Fail();
            }

        }).GetAwaiter().GetResult();
    }

    // Attempts to register user with invalid password
    // Will catch InvalidCredentialException thrown by AsyncRegisterUser
    [Test]
    public void TestRegisterUserLowercaseOnlyPassword() {
        BlueprintAPI blueprintApi = BlueprintAPI.WithBaseUrl(baseUrl);
        Random random = new Random();

        Task.Run(async () => {

            try {
                Task<APIResult<UserCredentials, JsonError>> fetchingResponse = 
                    blueprintApi.AsyncRegisterUser("adam" + random.Next(10000), "failure");
                
                APIResult<UserCredentials, JsonError> returnUser = await fetchingResponse;

                // Failure case
                // If here, exception has not been thrown
                Assert.Fail();
            } catch (InvalidCredentialException e) {
                // Pass case
                // Exception correctly thrown
            }

        }).GetAwaiter().GetResult();
    }
    
    // Attempts to register user with a valid password
    // Asserts return values are correct
    [Test]
    public void TestRegisterUserValidPassword() {       
        BlueprintAPI blueprintApi = BlueprintAPI.WithBaseUrl(baseUrl);
        Random random = new Random();
        UserCredentials returnUser = null;
        string username = "adam" + random.Next(10000);

        Task.Run(async () => {
            Task<APIResult<UserCredentials, JsonError>> fetchingResponse = blueprintApi.AsyncRegisterUser(username, "Failure123");
            APIResult<UserCredentials, JsonError> response = await fetchingResponse;

            if (response.isSuccess()) {
                // Success case
                returnUser = response.GetSuccess();
            } else {
                // Failure case
                Assert.Fail();
            }

        }).GetAwaiter().GetResult();
        
        // Check returned user is correct and contains access tokens
        Assert.That(returnUser.GetUsername(), Is.EqualTo(username));
        Assert.That(returnUser.GetPassword(), Is.EqualTo("Failure123"));    
        Assert.IsNotNull(returnUser.GetAccessToken());
        Assert.IsNotNull(returnUser.GetRefreshToken());
    }
     
    // Attempts to register user with invalid password
    // Will catch InvalidCredentialException thrown by AsyncRegisterUser
    [Test]
    public void TestRegisterUserNoLowercasePassword() {
        BlueprintAPI blueprintApi = BlueprintAPI.WithBaseUrl(baseUrl);
        Random random = new Random();
        string username = "adam" + random.Next(10000);
        
        Task.Run(async () => {
            try {
                Task<APIResult<UserCredentials, JsonError>> fetchingResponse = 
                    blueprintApi.AsyncRegisterUser(username, "FAILURE123");
                APIResult<UserCredentials, JsonError> response = await fetchingResponse;    
                
                // Failure case
                // If here, exception has not been thrown
                Assert.Fail();
            } catch (InvalidCredentialException e) {
                // Pass case
                // Exception correctly thrown
            }

        }).GetAwaiter().GetResult();
    }

    // Attempts to refresh tokens
    // Asserts returned tokens are not null
    [Test]
    public void TestRefreshTokens() {
        BlueprintAPI blueprintApi = BlueprintAPI.WithBaseUrl(baseUrl);      
        UserCredentials user = null;
        
        // Authenticate user to gain tokens
        Task.Run(async () => {             
            APIResult<UserCredentials, JsonError> response = await blueprintApi.AsyncAuthenticateUser(validUser);
            user = response.GetSuccess();
        }).GetAwaiter().GetResult();
        
        // Refresh tokens
        Task.Run(async () => {
            APIResult<ResponseAuthenticate, JsonError> response = await blueprintApi.AsyncRefreshTokens(user);

            if (response.isSuccess()) {
                // Success case
                Assert.IsNotNull(response.GetSuccess().refresh);
                Assert.IsNotNull(response.GetSuccess().access);
            } else {
                // Failure case
                Assert.Fail();
            }
        }).GetAwaiter().GetResult();
    }

    // Obtains an access token, adds an item to the inventory, then retrieves the inventory
    // Asserts contains are as expected
    [Test]
    public void TestGetInventory() {
        BlueprintAPI blueprintApi = BlueprintAPI.WithBaseUrl(baseUrl);
        UserCredentials user = null;
        ResponseGetInventory finalInventory = null;
        
        // Authenticate user to gain access token
        Task.Run(async () => {             
            APIResult<UserCredentials, JsonError> response = await blueprintApi.AsyncAuthenticateUser(validUser);
            user = response.GetSuccess();
        }).GetAwaiter().GetResult();


        // Add item to test user inventory
        Task.Run(async () => {
            List<InventoryEntry> entries = new List<InventoryEntry>();
            entries.Add(new InventoryEntry(1, 1));
            ResponseGetInventory inventory = new ResponseGetInventory(entries);

            APIResult<Boolean, JsonError> response = await blueprintApi.AsyncAddToInventory(user, inventory);
        }).GetAwaiter().GetResult();

        // Retrieve inventory of new user
        Task.Run(async () => {            
            APIResult<ResponseGetInventory, JsonError> finalInventoryResponse = await blueprintApi.AsyncGetInventory(user);
            finalInventory = finalInventoryResponse.GetSuccess();
        }).GetAwaiter().GetResult();
        
        Assert.That(finalInventory.items[0].item_id, Is.EqualTo(1));
        Assert.That(finalInventory.items[0].item_id, Is.GreaterThanOrEqualTo(1));
    }

    // Obtains an access token, adds item to user inventory
    // Fails in the case of an exception
    [Test]
    public void TestAddInventoryItem() {
        BlueprintAPI blueprintApi = BlueprintAPI.WithBaseUrl(baseUrl);
        UserCredentials user = null;

        // Authenticate user to gain access token
        Task.Run(async () => {
            APIResult<UserCredentials, JsonError> response = await blueprintApi.AsyncAuthenticateUser(validUser);
            user = response.GetSuccess();
        }).GetAwaiter().GetResult();

        // Add item to test user inventory
        Task.Run(async () => {
            try {
                List<InventoryEntry> entries = new List<InventoryEntry>();
                entries.Add(new InventoryEntry(1, 1));
                ResponseGetInventory inventory = new ResponseGetInventory(entries);

                APIResult<Boolean, JsonError> response = await blueprintApi.AsyncAddToInventory(user, inventory);
            } catch (WebException e) {
                // Exception throw, failure case
                Assert.Fail();
            }
        }).GetAwaiter().GetResult();
    }

    // Obtains an access token, adds item to inventory, deletes inventory contents
    // Fails in the case of an exception
    [Test]
    public void TestDeleteInventory() {
        BlueprintAPI blueprintApi = BlueprintAPI.WithBaseUrl(baseUrl);
        UserCredentials user = null;
        ResponseGetInventory finalInventory = null;
        
        // Authenticate user to gain access token
        Task.Run(async () => {             
            APIResult<UserCredentials, JsonError> response = await blueprintApi.AsyncAuthenticateUser(validUser);
            user = response.GetSuccess();
        }).GetAwaiter().GetResult();

        // Add item to test user inventory
        Task.Run(async () => {
            List<InventoryEntry> entries = new List<InventoryEntry>();
            entries.Add(new InventoryEntry(1, 1));

            ResponseGetInventory inventory = new ResponseGetInventory(entries);
            APIResult<Boolean, JsonError> response = await blueprintApi.AsyncAddToInventory(user, inventory);
        }).GetAwaiter().GetResult();
        
        // Delete inventory and assert on response
        Task.Run(async () => {
            try {
                APIResult<Boolean, JsonError> response = await blueprintApi.AsyncDeleteInventory(user);
                
                // Success case
            } catch (WebException e) {
                // Failure case
                Assert.Fail();
            }    
        }).GetAwaiter().GetResult();
    }
  
    [Test]
    public void TestBlueprintApiDefaultCredentialsConstructor() {
        BlueprintAPI blueprintApi = BlueprintAPI.DefaultCredentials();
        UserCredentials user = null;
        
        // Authenticate user
        Task.Run(async () => {             
            APIResult<UserCredentials, JsonError> response = await blueprintApi.AsyncAuthenticateUser(validUser);
            user = response.GetSuccess();
        }).GetAwaiter().GetResult();
        
        Assert.That(user.GetUsername(), Is.EqualTo(validUser.GetUsername()));
    }

    // Authenticates the test player account, asserts correct account_type returned
    [Test]
    public void TestAccountTypePlayer() {
        BlueprintAPI blueprintApi = BlueprintAPI.WithBaseUrl(baseUrl);
        UserCredentials user = validUser;
        UserCredentials returnUser = null; 
        
        Task.Run(async () => {
            Task<APIResult<UserCredentials, JsonError>> fetchingResponse = blueprintApi.AsyncAuthenticateUser(user);
            
            APIResult<UserCredentials, JsonError> response = await fetchingResponse;

            if (response.isSuccess()) {
                // Success case
                returnUser = response.GetSuccess();
            } else {
                //error case
            }

        }).GetAwaiter().GetResult();
        
        // Check returned user is correct and contains access tokens
        Assert.That(returnUser.GetUsername(), Is.EqualTo(validUser.GetUsername()));
        Assert.That(returnUser.GetPassword(), Is.EqualTo(validUser.GetPassword()));    
        Assert.IsNotNull(returnUser.GetAccessToken());
        Assert.IsNotNull(returnUser.GetRefreshToken());
        Assert.That(returnUser.GetAccountType(), Is.EqualTo("player"));
    }
    
    // Authenticates the test developer account, asserts correct account_type returned
    [Test]
    public void TestAccountTypeDeveloper() {
        BlueprintAPI blueprintApi = BlueprintAPI.WithBaseUrl(baseUrl);
        UserCredentials user = validDev;
        UserCredentials returnUser = null; 
        
        Task.Run(async () => {
            Task<APIResult<UserCredentials, JsonError>> fetchingResponse = blueprintApi.AsyncAuthenticateUser(user);
            
            APIResult<UserCredentials, JsonError> response = await fetchingResponse;

            if (response.isSuccess()) {
                // Success case
                returnUser = response.GetSuccess();
            } else {
                //error case
            }

        }).GetAwaiter().GetResult();
        
        // Check returned user is correct and contains access tokens
        Assert.That(returnUser.GetUsername(), Is.EqualTo(validDev.GetUsername()));
        Assert.That(returnUser.GetPassword(), Is.EqualTo(validDev.GetPassword()));    
        Assert.IsNotNull(returnUser.GetAccessToken());
        Assert.IsNotNull(returnUser.GetRefreshToken());
        Assert.That(returnUser.GetAccountType(), Is.EqualTo("developer"));
    }

    // Authenticates the test lecturer account, asserts correct account_type returned
    [Test]
    public void TestAccountTypeLecturer() {
        BlueprintAPI blueprintApi = BlueprintAPI.WithBaseUrl(baseUrl);
        UserCredentials user = validLec;
        UserCredentials returnUser = null; 
        
        Task.Run(async () => {
            Task<APIResult<UserCredentials, JsonError>> fetchingResponse = blueprintApi.AsyncAuthenticateUser(user);
            
            APIResult<UserCredentials, JsonError> response = await fetchingResponse;

            if (response.isSuccess()) {
                // Success case
                returnUser = response.GetSuccess();
            } else {
                //error case
            }

        }).GetAwaiter().GetResult();
        
        // Check returned user is correct and contains access tokens
        Assert.That(returnUser.GetUsername(), Is.EqualTo(validLec.GetUsername()));
        Assert.That(returnUser.GetPassword(), Is.EqualTo(validLec.GetPassword()));    
        Assert.IsNotNull(returnUser.GetAccessToken());
        Assert.IsNotNull(returnUser.GetRefreshToken());
        Assert.That(returnUser.GetAccountType(), Is.EqualTo("lecturer"));
    }
    
    // Blocked by user removal functionality
    /*[Test]
    public void TestRegisterUser_1() {
        var rest_handler = new RestHandler(baseUrl);
        
        UserCredentials return_user = rest_handler.RegisterUser("adam", "test");
        //Debug.Log(return_user.getUsername() + return_user.getPassword() + return_user.getAccessToken() + return_user.getRefreshToken());
       
        Assert.That(return_user.getUsername(), Is.EqualTo("adam"));
        Assert.That(return_user.getPassword(), Is.EqualTo("test"));    
        Assert.IsNotNull(return_user.getAccessToken());
        Assert.IsNotNull(return_user.getRefreshToken());
    }*/
    
    // TEST CLASSES
    private class JsonPlaceholder {
        public int userId;
        public int id;
        public string title;
        public string body;
    }

    private class JsonPost {
        public string value;
    }
}
