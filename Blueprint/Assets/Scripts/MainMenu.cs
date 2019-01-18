using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using UnityEditor;
using System.Threading.Tasks;

public class MainMenu : MonoBehaviour {
    public InputField usernameLoginInput;
    public InputField passwordLoginInput;
    public InputField usernameSignupInput;
    public InputField passwordSignupInput;
    public Button loginButton;
    public Button signupButton;
    public string usernameLoginText;
    public string passwordLoginText;
    public string usernameSignupText;
    public string passwordSignupText;
    public int maxUsernameLength;
    public BlueprintAPI api;
    Text errorMsg;

    void Start() {
        maxUsernameLength = 16;
        api = new BlueprintAPI("http://smithwjv.ddns.net");

        GameObject errorObject = GameObject.Find("ErrorMsg");
        errorMsg = errorObject.GetComponent<Text>();
        errorMsg.text = "";
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Return)) {
            onLoginClick();
        }
    }

    public void onLoginClick() {
        setError("");
        usernameLoginText = usernameLoginInput.text;
        passwordLoginText = passwordLoginInput.text;

        // Validate user input
        if (string.IsNullOrWhiteSpace(usernameLoginText) ||
            usernameLoginText.Length > maxUsernameLength) {
            setError("Invalid username, it must have between 1 and 16 characters.");
            return;
        } else if (string.IsNullOrWhiteSpace(passwordLoginText))  {
            setError("Please enter a non-empty password.");
            return;
        }

        UserCredentials userCredentials = new UserCredentials(usernameLoginText, passwordLoginText);
        UserCredentials returnUser;

        Task.Run( async () => {
            Task<UserCredentials> fetchingResponse = api.AsyncAuthenticateUser(userCredentials);

            try {
                returnUser = await fetchingResponse;
                // Launch Blueprint
                SceneManager.LoadScene("World");
            } catch (Exception e) {
                setError(e.Message);
            }
        }).GetAwaiter().GetResult();
    }

    public void onRegisterClick() {
        setError("");
        usernameSignupText = usernameSignupInput.text;
        passwordSignupText = passwordSignupInput.text;

        // Validate user input
        if (string.IsNullOrWhiteSpace(usernameSignupText) ||
            usernameSignupText.Length > maxUsernameLength) {
            setError("Invalid username, it must have between 1 and 16 characters.");
            return;
        } else if (string.IsNullOrWhiteSpace(passwordSignupText)) {
            setError("Please enter a non-empty password.");
            return;
        }

        UserCredentials returnUser;

        Task.Run(async () => {
            Task<UserCredentials> fetchingResponse = api.AsyncRegisterUser(usernameSignupText, passwordSignupText);

            try {
                returnUser = await fetchingResponse;
            }
            catch (Exception e) {
                setError(e.Message);
            }
        }).GetAwaiter().GetResult();
    }

    public void onSignupClick() {
        setError("");
    }

    public void onBackClick() {
        setError("");
    }
    private void setError(string error) {
        errorMsg.text = error;
    }
}
