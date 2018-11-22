using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserCredentials
{
    private string _username;
    private string _password;
    
    //server tokens
    private string _access;
    private string _refresh;

    public UserCredentials(string username, string password)
    {
        _username = username;
        _password = password;
    }

    //setters
    public void setUsername(string username)
    {
        _username = username;
    }

    public void setPassword(string password)
    {
        _password = password;
    }

    public void setAccessToken(string access)
    {
        _access = access;
    }

    public void setRefreshToken(string refresh)
    {
        _refresh = refresh;
    }
    
    //getters
    public string getUsername()
    {
        return _username;
    }
    
    public string getPassword()
    {
        return _password;
    }
    
    public string getAccessToken()
    {
        return _access;
    }
    
    public string getRefreshToken()
    {
        return _refresh;
    }
}
