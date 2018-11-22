using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;
using NUnit.Framework;
using NUnit.Framework.Api;

public class RestHandler_Tests {

    [Test]
    public void TestPerformGET()
    {
        var go = new GameObject();
        var resthandler = go.AddComponent<RestHandler>();

        var response = resthandler.performGET("http://jsonplaceholder.typicode.com/posts/1");
        
        JSONplaceholder inv = new JSONplaceholder();
        inv = JsonUtility.FromJson<JSONplaceholder>(response);

        Assert.That(inv.userId, Is.EqualTo(1));
        Assert.That(inv.id, Is.EqualTo(1));
        Assert.That(inv.title, Is.EqualTo("sunt aut facere repellat provident occaecati excepturi optio reprehenderit"));
        Assert.That(inv.body, Is.EqualTo("quia et suscipit\nsuscipit recusandae consequuntur expedita et " +
                                         "cum\nreprehenderit molestiae ut ut quas totam\nnostrum rerum est " +
                                         "autem sunt rem eveniet architecto"));
    }
}

public class JSONplaceholder
{
    public int userId;
    public int id;
    public string title;
    public string body;
}
