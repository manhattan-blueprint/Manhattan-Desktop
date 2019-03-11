using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using UnityEditor;
using System.Threading.Tasks;
using Controller;
using Model;
using Model.Action;
using Model.Redux;
using Model.State;
using Service;
using Service.Response;

public class MainMenu : MonoBehaviour, Subscriber<GameState> {
    [SerializeField] private InputField usernameLoginInput;
    [SerializeField] private InputField passwordLoginInput;
    [SerializeField] private InputField usernameSignupInput;
    [SerializeField] private InputField passwordSignupInput;
    [SerializeField] private Button loginButton;
    [SerializeField] private Button signupButton;
    [SerializeField] private Text   infoMessage;
    private int maxUsernameLength;
    private BlueprintAPI api;
    private bool splashScreenShowing;
    private GameObject background;

    void Start() {
        background = GameObject.Find("HexVideoHolder");

        maxUsernameLength = 16;
        api = BlueprintAPI.WithBaseUrl("http://smithwjv.ddns.net");

        GameObject errorObject = GameObject.Find("InfoMessage");
        infoMessage = errorObject.GetComponent<Text>();
        infoMessage.color = Color.blue;
        infoMessage.text = "";
        usernameLoginInput.Select();
        GameManager.Instance().store.Subscribe(this);
    }


    public void StateDidUpdate(GameState state) {
        if (state.uiState.Selected == UIState.OpenUI.Playing) {
            SceneManager.LoadScene(SceneMapping.World);
            GameManager.Instance().store.Unsubscribe(this);
        }
    }

    void Update() {
        if (splashScreenShowing) {
            if (Input.GetKeyDown(KeyCode.Tab)) {
                HideSplashScreen();
            }
        }
        else {
            if (Input.GetKeyDown(KeyCode.Tab)) {
                if (usernameLoginInput.isFocused) {
                    passwordLoginInput.Select();
                } else if (passwordLoginInput.isFocused) {
                    loginButton.Select();
                } else if (usernameSignupInput.isFocused) {
                    passwordSignupInput.Select();
                } else if (passwordSignupInput.isFocused) {
                    signupButton.Select();
                } else if (usernameLoginInput.IsActive()) {
                    usernameLoginInput.Select();
                } else if (usernameSignupInput.IsActive()) {
                    usernameSignupInput.Select();
                }
            } else if (Input.GetKeyDown(KeyCode.Return)) {
                if (passwordLoginInput.IsActive()) {
                    OnLoginClick();
                } else if (passwordSignupInput.IsActive()) {
                    OnRegisterClick();
                }
            }
        }
    }

    public void OnLoginClick() {
        SetInfoMessage("");
        string usernameLoginText = usernameLoginInput.text;
        string passwordLoginText = passwordLoginInput.text;

        // Validate user input
        if (string.IsNullOrWhiteSpace(usernameLoginText) ||
            usernameLoginText.Length > maxUsernameLength) {
            SetErrorMessage("Invalid username, it must have between 1 and 16 characters.");
            return;
        } else if (string.IsNullOrWhiteSpace(passwordLoginText))  {
            SetErrorMessage("Please enter a non-empty password.");
            return;
        }

        UserCredentials userCredentials = new UserCredentials(usernameLoginText, passwordLoginText);
        UserCredentials returnUser;

        Task.Run( async () => {
            Task<APIResult<UserCredentials, JsonError>> fetchingResponse = api.AsyncAuthenticateUser(userCredentials);
            // TODO Add a visual cue ( using setInfoMessage(“Connecting . . . “) )
            //      to indicate to the user that the app is waiting on a response form the server.

            try {
                APIResult<UserCredentials, JsonError> response = await fetchingResponse;
                returnUser = response.GetSuccess();
                GameManager.Instance().SetUserCredentials(returnUser);
                if (response.isSuccess()) {
                    // Launch Blueprint
                        GameManager.Instance().store.Dispatch(
                            new OpenPlayingUI());
                } else {
                    SetErrorMessage(response.GetError().error);
                }
            } catch (Exception e) {
                SetErrorMessage(e.Message);
            }
        }).GetAwaiter().GetResult();
    }

    public void OnRegisterClick() {
        SetInfoMessage("");
        string usernameSignupText = usernameSignupInput.text;
        string passwordSignupText = passwordSignupInput.text;

        // Validate user input
        if (string.IsNullOrWhiteSpace(usernameSignupText) ||
            usernameSignupText.Length > maxUsernameLength) {
            SetErrorMessage("Invalid username, it must have between 1 and 16 characters.");
            return;
        } else if (string.IsNullOrWhiteSpace(passwordSignupText)) {
            SetErrorMessage("Please enter a non-empty password.");
            return;
        }

        UserCredentials returnUser;

        Task.Run(async () => {
            Task<APIResult<UserCredentials, JsonError>> fetchingResponse = api.AsyncRegisterUser(usernameSignupText, passwordSignupText);
            // TODO Add a visual cue ( using setInfoMessage(“Connecting . . . “) )
            //      to indicate to the user that the app is waiting on a response form the server.

            try {
                APIResult<UserCredentials, JsonError> response = await fetchingResponse;
                returnUser = response.GetSuccess();
                if (response.isSuccess()) {
                    // Launch Blueprint
                    SceneManager.LoadScene(SceneMapping.World);
                } else {
                    SetErrorMessage(response.GetError().error);
                }
            }
            catch (Exception e) {
                SetErrorMessage(e.Message);
            }
        }).GetAwaiter().GetResult();
    }

    public void OnSignupClick() {
        SetInfoMessage("");
        usernameSignupInput.Select();
    }

    public void OnBackClick() {
        SetInfoMessage("");
        usernameLoginInput.Select();
    }

    private void SetErrorMessage(string msg) {
        this.infoMessage.color = Color.red;
        this.infoMessage.text = msg;
    }

    private void SetInfoMessage(string msg) {
        this.infoMessage.color = Color.blue;
        this.infoMessage.text = msg;
    }

    private void HideSplashScreen() {
        splashScreenShowing = false;
    }
}
