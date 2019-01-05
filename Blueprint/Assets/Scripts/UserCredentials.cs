using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UserCredentials {
    private string username;
    private string password;
    
    // Server tokens
    private string access;
    private string refresh;

    public UserCredentials(string username, string password)
    {
        this.username = username;
        this.password = password;
    }

    public UserCredentials(string username, string password, string access, string refresh) {
        this.username = username;
        this.password = password;
        this.access = access;
        this.refresh = refresh;
    }
    
    // Getters
    public string getUsername()
    {
        return username;
    }
    
    public string getPassword()
    {
        return password;
    }
    
    public string getAccessToken()
    {
        return access;
    }
    
    public string getRefreshToken()
    {
        return refresh;
    }
    
}
