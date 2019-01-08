using System.Collections.Generic;
using System.Net;
using System.Security.Authentication;
using System.Threading.Tasks;
using UnityEngine;
using NUnit.Framework;
using Random = System.Random;

public class RestHandlerTests {
    private string baseUrl = "http://smithwjv.ddns.net";
    
    // GETs data from jsonplaceholder, asserts it is correct
    [Test]
    public void TestPerformGET() {
        var restHandler = new RestHandler("http://jsonplaceholder.typicode.com");

        var response = restHandler.PerformGET("/posts/1");

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
        var restHandler = new RestHandler("http://jsonplaceholder.typicode.com");

        // Setup
        var data = new JsonPost();
        data.value = "hello";
        var payload = JsonUtility.ToJson(data);
        var responseJson= new JsonPost();

        // Response
        Task.Run(async () => {
            var response = await restHandler.PerformAsyncPost("/posts", payload);
            responseJson = JsonUtility.FromJson<JsonPost>(response);
        }).GetAwaiter().GetResult();

        Assert.That(responseJson.value, Is.EqualTo("hello"));
    }
   
    // Authenticates user with valid credentials, who is present in the db
    // Is an example of HTTP error handling
    [Test]
    public void TestValidAuthenticateUser() {
        var blueprintApi = new BlueprintAPI(baseUrl);
        UserCredentials user = new UserCredentials("adam1", "Test123");
        UserCredentials returnUser = null;
        
        Task.Run(async () => {
            // Example asynchronous call
            Task<UserCredentials> fetchingResponse = blueprintApi.AsyncAuthenticateUser(user);
            
            // Can perform other tasks here while awaiting response
            
            try {
                returnUser = await fetchingResponse;

                // Success logic
            }
            catch (WebException e) {
                // Failure logic
                
                // Example of error handling
                switch (blueprintApi.RetrieveHTTPCode(e)) {
                    case (int)BlueprintAPI.httpResponseCode.unauthorised:
                        throw new InvalidCredentialException("The credentials provided do not match any user");
                        break;
                }
            }  
        }).GetAwaiter().GetResult();
        
        // Check returned user is correct and contains access tokens
        Assert.That(returnUser.getUsername(), Is.EqualTo("adam1"));
        Assert.That(returnUser.getPassword(), Is.EqualTo("Test123"));    
        Assert.IsNotNull(returnUser.getAccessToken());
        Assert.IsNotNull(returnUser.getRefreshToken());
    }
    
    // Attempts to authenticate user with invalid password
    // Will catch InvalidCredentialException thrown by AsyncAuthenticateUser
    [Test]
    public void TestInvalidAuthenticateUser() {
        var blueprintApi = new BlueprintAPI(baseUrl);
        UserCredentials user = new UserCredentials("adam", "test123");
        UserCredentials returnUser = null;
        
        Task.Run(async () => {
            Task<UserCredentials> fetchingResponse = blueprintApi.AsyncAuthenticateUser(user);

            try {
                returnUser = await fetchingResponse;

                // Failure case
                // If here, exception has not been thrown
                Assert.Fail();
            }
            catch (InvalidCredentialException e) {
                // Pass case
                // Exception correctly thrown
            }

        }).GetAwaiter().GetResult();
    }

    // Attempts to register user with invalid password
    // Will catch InvalidCredentialException thrown by AsyncRegisterUser
    [Test]
    public void TestRegisterUserLowercaseOnlyPassword() {
        var blueprintApi = new BlueprintAPI(baseUrl);
        Random random = new Random();

        Task.Run(async () => {
            Task<UserCredentials> fetchingResponse = 
                blueprintApi.AsyncRegisterUser("adam" + random.Next(10000), "failure");

            try {
                UserCredentials returnUser = await fetchingResponse;

                // Failure case
                // If here, exception has not been thrown
                Assert.Fail();
            }
            catch (InvalidCredentialException e) {
                // Pass case
                // Exception correctly thrown
            }

        }).GetAwaiter().GetResult();
    }
    
    // Attempts to register user with a valid password
    // Asserts return values are correct
    [Test]
    public void TestRegisterUserValidPassword() {       
        var blueprintApi = new BlueprintAPI(baseUrl);
        Random random = new Random();
        UserCredentials returnUser = null;
        string username = "adam" + random.Next(10000);

        Task.Run(async () => {
            Task<UserCredentials> fetchingResponse = blueprintApi.AsyncRegisterUser(username, "Failure123");

            try {
                returnUser = await fetchingResponse;
            }
            catch (InvalidCredentialException e) {
                // Failure case
                // Exception incorrectly thrown
                Assert.Fail();
            }

        }).GetAwaiter().GetResult();
        
        // Check returned user is correct and contains access tokens
        Assert.That(returnUser.getUsername(), Is.EqualTo(username));
        Assert.That(returnUser.getPassword(), Is.EqualTo("Failure123"));    
        Assert.IsNotNull(returnUser.getAccessToken());
        Assert.IsNotNull(returnUser.getRefreshToken());
    }
    
    // Attempts to register user with invalid password
    // Will catch InvalidCredentialException thrown by AsyncRegisterUser
    [Test]
    public void TestRegisterUserNoLowercasePassword() {
        var blueprintApi = new BlueprintAPI(baseUrl);
        Random random = new Random();
        string username = "adam" + random.Next(10000);
        
        Task.Run(async () => {
            Task<UserCredentials> fetchingResponse = 
                blueprintApi.AsyncRegisterUser(username, "FAILURE123");

            try {
                UserCredentials returnUser = await fetchingResponse;
                
                // Failure case
                // If here, exception has not been thrown
                Assert.Fail();
            }
            catch (InvalidCredentialException e) {
                // Pass case
                // Exception correctly thrown
            }

        }).GetAwaiter().GetResult();
    }

    [Test]
    public void TestRefreshTokens() {
        var blueprintApi = new BlueprintAPI(baseUrl);      
        UserCredentials user = null;
        
        // Authenticate user to gain tokens
        Task.Run(async () => {             
            user = await blueprintApi.AsyncAuthenticateUser(new UserCredentials("adam1", "Test123"));
        }).GetAwaiter().GetResult();
        
        // Refresh tokens
        Task.Run(async () => {
            try {
                ResponseAuthenticate response = await blueprintApi.AsyncRefreshTokens(user.getRefreshToken());

                Assert.IsNotNull(response.refresh);
                Assert.IsNotNull(response.access);
            }
            catch (WebException e) {
                // Exception thrown, failure case
                Debug.Log(e.Message);
                Assert.Fail();
            }
        }).GetAwaiter().GetResult();
    }

    // Obtains an access token, adds an item to the inventory, then retrieves the inventory
    // Asserts contains are as expected
    [Test]
    public void TestGetInventory() {
        var blueprintApi = new BlueprintAPI(baseUrl);
        UserCredentials user = null;
        ResponseGetInventory finalInventory = null;
        
        // Authenticate user to gain access token
        Task.Run(async () => {             
            user = await blueprintApi.AsyncAuthenticateUser(new UserCredentials("adam1", "Test123"));
        }).GetAwaiter().GetResult();

        // Add item to test user inventory
        Task.Run(async () => {
            List<InventoryEntry> entries = new List<InventoryEntry>();
            entries.Add(new InventoryEntry(1, 1));
            ResponseGetInventory inventory = new ResponseGetInventory(entries);

            string response = await blueprintApi.AsyncAddToInventory(user.getAccessToken(), inventory);
        }).GetAwaiter().GetResult();

        // Retrieve inventory of new user
        Task.Run(async () => {
            
            finalInventory = await blueprintApi.AsyncGetInventory(user.getAccessToken());
        }).GetAwaiter().GetResult();
        
        Assert.That(finalInventory.items[0].item_id, Is.EqualTo(1));
        Assert.That(finalInventory.items[0].item_id, Is.GreaterThanOrEqualTo(1));
    }

    // Obtains an access token, adds item to user inventory
    // Fails in the case of an exception
    [Test]
    public void TestAddInventoryItem() {
        var blueprintApi = new BlueprintAPI(baseUrl);
        UserCredentials user = null;

        // Authenticate user to gain access token
        Task.Run(async () => {
            user = await blueprintApi.AsyncAuthenticateUser(new UserCredentials("adam1", "Test123"));
        }).GetAwaiter().GetResult();

        // Add item to test user inventory
        Task.Run(async () => {
            try {
                List<InventoryEntry> entries = new List<InventoryEntry>();
                entries.Add(new InventoryEntry(1, 1));
                ResponseGetInventory inventory = new ResponseGetInventory(entries);

                string response = await blueprintApi.AsyncAddToInventory(user.getAccessToken(), inventory);
            }
            catch (WebException e) {
                // Exception throw, failure case
                Assert.Fail();
            }
        }).GetAwaiter().GetResult();
    }

    // Obtains an access token, adds item to inventory, deletes inventory contents
    // Fails in the case of an exception
    [Test]
    public void TestDeleteInventory() {
        var blueprintApi = new BlueprintAPI(baseUrl);
        UserCredentials user = null;
        ResponseGetInventory finalInventory = null;
        
        // Authenticate user to gain access token
        Task.Run(async () => {             
            user = await blueprintApi.AsyncAuthenticateUser(new UserCredentials("adam1", "Test123"));
        }).GetAwaiter().GetResult();

        // Add item to test user inventory
        Task.Run(async () => {
            List<InventoryEntry> entries = new List<InventoryEntry>();
            entries.Add(new InventoryEntry(1, 1));
            ResponseGetInventory inventory = new ResponseGetInventory(entries);

            string response = await blueprintApi.AsyncAddToInventory(user.getAccessToken(), inventory);
        }).GetAwaiter().GetResult();
        
        // Delete inventory and assert on response
        Task.Run(async () => {
            try {
                string response = await blueprintApi.AsyncDeleteInventory(user.getAccessToken());
                
                // Success case
            }
            catch (WebException e) {
                // Failure case
                Assert.Fail();
            }    
        }).GetAwaiter().GetResult();
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