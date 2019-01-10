using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using System;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using UnityEditor;


using System.Threading.Tasks;
using System.Net;
using System.Security.Authentication;

public class Login : MonoBehaviour {
    public InputField usernameInput;
    public InputField passwordInput;
    public Button loginButton;
    public string usernameText;
    public string passwordText;
    public int maxUsernameLength;
    public BlueprintAPI api;

    // Start is called before the first frame update
    void Start() {
        maxUsernameLength = 16;
        api = new BlueprintAPI("http://smithwjv.ddns.net");
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Return)) {
            onLoginClick();
        }
    }

    public void onLoginClick() {
        usernameText = usernameInput.text;
        passwordText = passwordInput.text;

        // Validate user input
        if (string.IsNullOrWhiteSpace(usernameText) ||
            usernameText.Length > maxUsernameLength) {
            EditorUtility.DisplayDialog("Invalid username",
                "The username must have between 1 and 16 charactes.", "OK");
            return;
        } else if (string.IsNullOrWhiteSpace(passwordText))  {
            EditorUtility.DisplayDialog("Invalid password",
                "Please enter a non empty password.", "OK");
            return;
        }

        UserCredentials userCredentials = new UserCredentials(usernameText, passwordText);
        UserCredentials returnUser;

        Task.Run( async () => {
            Task<UserCredentials> fetchingResponse = api.AsyncAuthenticateUser(userCredentials);

            try {
                returnUser = await fetchingResponse;
                // Launch Blueprint
                SceneManager.LoadScene("World");
            } catch (Exception e) {
                EditorUtility.DisplayDialog("Login failed!", e.ToString(), "OK");
            }
        }).GetAwaiter().GetResult();
    }
        
}
