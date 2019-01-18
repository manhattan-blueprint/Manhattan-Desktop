using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using UnityEditor;
using System.Threading.Tasks;
using Service;
using Service.Response;

public class MainMenu : MonoBehaviour {
    [SerializeField] private InputField usernameLoginInput;
    [SerializeField] private InputField passwordLoginInput;
    [SerializeField] private InputField usernameSignupInput;
    [SerializeField] private InputField passwordSignupInput;
    [SerializeField] private Button loginButton;
    [SerializeField] private Button signupButton;
    [SerializeField] private Text   infoMessage;
    private int maxUsernameLength;
    private BlueprintAPI api;

    void Start() {
        maxUsernameLength = 16;
        api = new BlueprintAPI("http://smithwjv.ddns.net");

        GameObject errorObject = GameObject.Find("InfoMessage");
        infoMessage = errorObject.GetComponent<Text>();
        infoMessage.color = Color.blue;
        infoMessage.text = "";
        usernameLoginInput.Select();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            if        (usernameLoginInput.isFocused) {
                passwordLoginInput.Select();
            } else if (passwordLoginInput.isFocused) {
                loginButton.Select();
            } else if (usernameSignupInput.isFocused) {
                passwordSignupInput.Select();
            } else if (passwordSignupInput.isFocused) {
                signupButton.Select();
            } else {
                if (usernameLoginInput.IsActive()) {
                    usernameLoginInput.Select();
                } else if (usernameSignupInput.IsActive()) {
                    usernameSignupInput.Select();
                }
            }
        }
    }

    public void onLoginClick() {
        setInfoMessage("");
        string usernameLoginText = usernameLoginInput.text;
        string passwordLoginText = passwordLoginInput.text;

        // Validate user input
        if (string.IsNullOrWhiteSpace(usernameLoginText) ||
            usernameLoginText.Length > maxUsernameLength) {
            setErrorMessage("Invalid username, it must have between 1 and 16 characters.");
            return;
        } else if (string.IsNullOrWhiteSpace(passwordLoginText))  {
            setErrorMessage("Please enter a non-empty password.");
            return;
        }

        UserCredentials userCredentials = new UserCredentials(usernameLoginText, passwordLoginText);
        UserCredentials returnUser;

        Task.Run( async () => {
            Task<UserCredentials> fetchingResponse = api.AsyncAuthenticateUser(userCredentials);
            // TODO Add a visual cue ( using setInfoMessage(“Connecting . . . “) ) 
            //      to indicate to the user that the app is waiting on a response form the server.

            try {
                returnUser = await fetchingResponse;
                setInfoMessage("Login Successful!");
                // Launch Blueprint
                SceneManager.LoadScene("World");
            } catch (Exception e) {
                setErrorMessage(e.Message);
            }
        }).GetAwaiter().GetResult();
    }

    public void onRegisterClick() {
        setInfoMessage("");
        string usernameSignupText = usernameSignupInput.text;
        string passwordSignupText = passwordSignupInput.text;

        // Validate user input
        if (string.IsNullOrWhiteSpace(usernameSignupText) ||
            usernameSignupText.Length > maxUsernameLength) {
            setErrorMessage("Invalid username, it must have between 1 and 16 characters.");
            return;
        } else if (string.IsNullOrWhiteSpace(passwordSignupText)) {
            setErrorMessage("Please enter a non-empty password.");
            return;
        }

        UserCredentials returnUser;

        Task.Run(async () => {
            Task<UserCredentials> fetchingResponse = api.AsyncRegisterUser(usernameSignupText, passwordSignupText);
            // TODO Add a visual cue ( using setInfoMessage(“Connecting . . . “) ) 
            //      to indicate to the user that the app is waiting on a response form the server.

            try {
                returnUser = await fetchingResponse;
                setInfoMessage("Registration Successful!");
            }
            catch (Exception e) {
                setErrorMessage(e.Message);
            }
        }).GetAwaiter().GetResult();
    }

    public void onSignupClick() {
        setInfoMessage("");
        usernameSignupInput.Select();
    }

    public void onBackClick() {
        setInfoMessage("");
        usernameLoginInput.Select();
    }

    private void setErrorMessage(string msg) {
        this.infoMessage.color = Color.red;
        this.infoMessage.text = msg;
    }

    private void setInfoMessage(string msg) {
        this.infoMessage.color = Color.blue;
        this.infoMessage.text = msg;
    }
}
