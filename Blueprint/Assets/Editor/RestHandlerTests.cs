using System.Security.Authentication;
using UnityEngine;
using NUnit.Framework;
using Random = System.Random;

public class RestHandlerTests {
    private string baseUrl = "http://smithwjv.ddns.net";
    
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

    [Test]
    public void TestPerformPOST() {
        var restHandler = new RestHandler("http://jsonplaceholder.typicode.com");

        //setup
        var data = new JsonPost();
        data.value = "hello";
        var payload = JsonUtility.ToJson(data);

        //response
        var response = restHandler.PerformPOST("/posts", payload);
        var responseJson = JsonUtility.FromJson<JsonPost>(response);

        Assert.That(responseJson.value, Is.EqualTo("hello"));
    }

    [Test]
    public void TestValidAuthenticateUser() {
        var blueprintApi = new BlueprintAPI(baseUrl);
        
        UserCredentials user = new UserCredentials("adam", "test");
        UserCredentials returnUser = blueprintApi.AuthenticateUser(user);
       
        // Check returned user is correct and contains access tokens
        Assert.That(returnUser.getUsername(), Is.EqualTo("adam"));
        Assert.That(returnUser.getPassword(), Is.EqualTo("test"));    
        Assert.IsNotNull(returnUser.getAccessToken());
        Assert.IsNotNull(returnUser.getRefreshToken());
    }
    
    [Test]
    public void TestInvalidAuthenticateUser() {
        var blueprintApi = new BlueprintAPI(baseUrl);
        
        UserCredentials user = new UserCredentials("adam", "test123");
        UserCredentials returnUser = blueprintApi.AuthenticateUser(user);
       
        Assert.That(returnUser.getUsername(), Is.EqualTo("adam"));
        Assert.That(returnUser.getPassword(), Is.EqualTo("test123"));    
        
        // Null as it's an error case
        Assert.IsNull(returnUser.getAccessToken());
        Assert.IsNull(returnUser.getRefreshToken());
    }

    [Test]
    public void TestRegisterUserLowercaseOnlyPassword() {
        var blueprintApi = new BlueprintAPI(baseUrl);

        try {
            UserCredentials returnUser = blueprintApi.RegisterUser("adam", "failure");

            // If execution reaches here, no exception has been thrown
            // Failure case
            Assert.Fail();
        }
        catch (InvalidCredentialException e) {
            // Exception correctly thrown
            // Pass case
        }
    }
    
    [Test]
    public void TestRegisterUserValidPassword() {
        var blueprintApi = new BlueprintAPI(baseUrl);
        Random random = new Random();

        try {
            UserCredentials returnUser = blueprintApi.RegisterUser("adam" + random.Next(10000), "Failure123");
        }
        catch (InvalidCredentialException e) {
            // Password is valid, failure case when exception is thrown
            Assert.Fail();
        }
    }
    
    [Test]
    public void TestRegisterUserNoLowercasePassword() {
        var blueprintApi = new BlueprintAPI(baseUrl);
        Random random = new Random();

        try {
            UserCredentials returnUser = blueprintApi.RegisterUser("adam" + random.Next(10000), "FAILURE123");
            
            // If execution reaches here, no exception has been thrown
            // Failure case
            Assert.Fail();
        }
        catch (InvalidCredentialException e) {
            // Exception correctly thrown
            // Pass case
        }
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
    
    //TEST CLASSES
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


