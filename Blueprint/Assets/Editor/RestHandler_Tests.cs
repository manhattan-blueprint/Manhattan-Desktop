using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Security.Authentication;
using UnityEngine;
using NUnit.Framework;
using NUnit.Framework.Api;

public class RestHandler_Tests {
    private string baseUrl = "http://smithwjv.ddns.net";
    
    [Test]
    public void TestPerformGET() {
        var rest_handler = new RestHandler("http://jsonplaceholder.typicode.com");

        var response = rest_handler.performGET("/posts/1");

        JSONplaceholder inv = new JSONplaceholder();
        inv = JsonUtility.FromJson<JSONplaceholder>(response);

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
        var rest_handler = new RestHandler("http://jsonplaceholder.typicode.com");

        //setup
        var data = new JSONpost();
        data.value = "hello";
        var payload = JsonUtility.ToJson(data);

        //response
        var response = rest_handler.performPOST("/posts", payload);
        var response_json = JsonUtility.FromJson<JSONpost>(response);

        Assert.That(response_json.value, Is.EqualTo("hello"));
    }

    //DEPRECATED
    /* [Test]
    public void TestAuthenticateUser_1() {
        var rest_handler = new RestHandler("http://jsonplaceholder.typicode.com");
        
        UserCredentials user = new UserCredentials("adam", "test");
        UserCredentials return_user = rest_handler.AuthenticateUser(user, "/posts");
       
        Assert.That(return_user.getUsername(), Is.EqualTo("adam"));
        Assert.That(return_user.getPassword(), Is.EqualTo("test"));
        Assert.That(return_user.getAccessToken(), Is.EqualTo(null));
        Assert.That(return_user.getRefreshToken(), Is.EqualTo(null));
    } */

    [Test]
    public void TestAuthenticateUser_2() {
        var rest_handler = new RestHandler(baseUrl);
        
        UserCredentials user = new UserCredentials("adam", "test");
        UserCredentials return_user = rest_handler.AuthenticateUser(user);
       
        Assert.That(return_user.getUsername(), Is.EqualTo("adam"));
        Assert.That(return_user.getPassword(), Is.EqualTo("test"));    
        Assert.IsNotNull(return_user.getAccessToken());
        Assert.IsNotNull(return_user.getRefreshToken());
    }
    
    [Test]
    public void TestAuthenticateUser_3() {
        var rest_handler = new RestHandler(baseUrl);
        
        UserCredentials user = new UserCredentials("adam", "test123");
        UserCredentials return_user = rest_handler.AuthenticateUser(user);
       
        Assert.That(return_user.getUsername(), Is.EqualTo("adam"));
        Assert.That(return_user.getPassword(), Is.EqualTo("test123"));    
        
        //null in error case
        Assert.IsNull(return_user.getAccessToken());
        Assert.IsNull(return_user.getRefreshToken());
    }
    
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

    [Test]
    public void TestRegisterUser_2() {
        var rest_handler = new RestHandler(baseUrl);

        try {
            UserCredentials return_user = rest_handler.RegisterUser("adam", "failure");

            //if execution reaches here, no exception has been thrown
            Assert.Fail();
        }
        catch (InvalidCredentialException e) {
            
        }
    }
}


//TEST CLASSES

public class JSONplaceholder {
    public int userId;
    public int id;
    public string title;
    public string body;
}

public class JSONpost {
    public string value;
}