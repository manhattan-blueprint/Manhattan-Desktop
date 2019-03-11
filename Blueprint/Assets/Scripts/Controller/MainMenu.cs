using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
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
    [SerializeField] private GameObject loginMenu;
    [SerializeField] private GameObject registerMenu;
    [SerializeField] private GameObject splashScreen;
    [SerializeField] private InputField usernameLoginInput;
    [SerializeField] private InputField passwordLoginInput;
    [SerializeField] private InputField usernameRegisterInput;
    [SerializeField] private InputField passwordRegisterInput;
    [SerializeField] private Button loginButton;
    [SerializeField] private Button registerButton;
    [SerializeField] private Text registerMsg;
    [SerializeField] private Text blueprintLogo;
    [SerializeField] private Text pressSpace;
    private int maxUsernameLength;
    private BlueprintAPI api;
    private MenuState menuState;

    private enum MenuState {
        SplashScreen,
        Login,
        Register
    }

    void Start() {
        loginMenu.GetComponent<CanvasGroup>().alpha = 0.0f;
        loginMenu.GetComponent<CanvasGroup>().interactable = false;
        registerMenu.GetComponent<CanvasGroup>().alpha = 0.0f;
        registerMenu.GetComponent<CanvasGroup>().interactable = false;
        menuState = MenuState.SplashScreen;

        maxUsernameLength = 16;
        api = BlueprintAPI.WithBaseUrl("http://smithwjv.ddns.net");

        GameManager.Instance().store.Subscribe(this);
    }

    public void StateDidUpdate(GameState state) {
        if (state.uiState.Selected == UIState.OpenUI.Playing) {
            SceneManager.LoadScene(SceneMapping.World);
            GameManager.Instance().store.Unsubscribe(this);
        }
    }

    void Update() {
        if (menuState == MenuState.SplashScreen) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                ToLoginMenu();
            }
        }
        else {
            if (Input.GetKeyDown(KeyCode.Tab)) {
                if (usernameLoginInput.isFocused) {
                    passwordLoginInput.Select();
                } else if (passwordLoginInput.isFocused) {
                    loginButton.Select();
                } else if (usernameRegisterInput.isFocused) {
                    passwordRegisterInput.Select();
                } else if (passwordRegisterInput.isFocused) {
                    registerButton.Select();
                } else if (usernameLoginInput.IsActive()) {
                    usernameLoginInput.Select();
                } else if (usernameRegisterInput.IsActive()) {
                    usernameRegisterInput.Select();
                }
            } else if (Input.GetKeyDown(KeyCode.Return)) {
                if (passwordLoginInput.IsActive()) {
                    OnLoginClick();
                } else if (passwordRegisterInput.IsActive()) {
                    OnRegisterClick();
                }
            }
        }
    }

    public void OnLoginClick() {
        SetRegisterMsg("");
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
            // TODO Add a visual cue ( using setRegisterMsg(“Connecting . . . “) )
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
        SetRegisterMsg("");
        string usernameRegisterText = usernameRegisterInput.text;
        string passwordRegisterText = passwordRegisterInput.text;

        // Validate user input
        if (string.IsNullOrWhiteSpace(usernameRegisterText) ||
            usernameRegisterText.Length > maxUsernameLength) {
            SetErrorMessage("Invalid username, it must have between 1 and 16 characters.");
            return;
        } else if (string.IsNullOrWhiteSpace(passwordRegisterText)) {
            SetErrorMessage("Please enter a non-empty password.");
            return;
        }

        UserCredentials returnUser;

        Task.Run(async () => {
            Task<APIResult<UserCredentials, JsonError>> fetchingResponse = api.AsyncRegisterUser(usernameRegisterText, passwordRegisterText);
            // TODO Add a visual cue ( using setRegisterMsg(“Connecting . . . “) )
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

    private void SetErrorMessage(string msg) {
        registerMsg.color = Color.red;
        registerMsg.text = msg;
    }

    private void SetRegisterMsg(string msg) {
        registerMsg.color = Color.blue;
        registerMsg.text = msg;
    }

    private void ToLoginMenu() {
        switch (menuState) {
            case MenuState.SplashScreen:
                loginMenu.GetComponent<CanvasGroup>().alpha = 1.0f;
                loginMenu.GetComponent<CanvasGroup>().interactable = true;
                splashScreen.GetComponent<CanvasGroup>().alpha = 0.0f;
                splashScreen.GetComponent<CanvasGroup>().interactable = false;
                blueprintLogo.transform.position += new Vector3(0.0f, 100.0f, 0.0f);
                usernameLoginInput.Select();
                break;
        }
    }

    private void ToRegister() {
        switch (menuState) {
            case MenuState.SplashScreen:
                registerMenu.GetComponent<CanvasGroup>().alpha = 1.0f;
                registerMenu.GetComponent<CanvasGroup>().interactable = true;
                loginMenu.GetComponent<CanvasGroup>().alpha = 0.0f;
                loginMenu.GetComponent<CanvasGroup>().interactable = false;
                usernameRegisterInput.Select();
                break;
        }
    }
}
