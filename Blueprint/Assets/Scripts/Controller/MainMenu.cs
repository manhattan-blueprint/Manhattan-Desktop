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
    [SerializeField] private Text infoMessage;

    [SerializeField] private GameObject splashScreen;
    [SerializeField] private Text blueprintLogo;
    [SerializeField] private Text pressSpace;

    [SerializeField] private GameObject loginMenu;
    [SerializeField] private InputField loginUsernameInput;
    [SerializeField] private InputField loginPasswordInput;
    [SerializeField] private Button loginLoginButton;
    [SerializeField] private Button loginRegisterButton;

    [SerializeField] private GameObject registerMenu;
    [SerializeField] private InputField registerUsernameInput;
    [SerializeField] private InputField registerPasswordInput;
    [SerializeField] private Button registerRegisterButton;
    [SerializeField] private Button registerBackButton;

    private int maxUsernameLength;
    private BlueprintAPI api;
    private VisibleMenu visibleMenu;

    private bool isMessageErrorStyle;
    private bool toLaunch;
    private string infoMessageText;

    private enum VisibleMenu {
        SplashScreen,
        Login,
        Register
    }

    void Start() {
        loginMenu.GetComponent<CanvasGroup>().alpha = 0.0f;
        loginMenu.GetComponent<CanvasGroup>().interactable = false;
        registerMenu.GetComponent<CanvasGroup>().alpha = 0.0f;
        registerMenu.GetComponent<CanvasGroup>().interactable = false;
        SetMessageClear();
        visibleMenu = VisibleMenu.SplashScreen;
        isMessageErrorStyle = false;
        toLaunch = false;

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
        switch (visibleMenu) {
            case VisibleMenu.SplashScreen:
                if (Input.GetKeyDown(KeyCode.Space)) {
                    ToLoginMenu();
                }
                break;

            case VisibleMenu.Login:
                if (Input.GetKeyDown(KeyCode.Tab)) {
                    // Cyclic movement through menu using Tab
                    if (loginUsernameInput.isFocused) {
                        loginPasswordInput.Select();
                    }
                    else if (loginPasswordInput.isFocused) {
                        loginLoginButton.Select();
                    }
                    else if (loginLoginButton.IsActive()) {
                        loginRegisterButton.Select();
                    }
                    else if (loginRegisterButton.IsActive()) {
                        loginUsernameInput.Select();
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Return)) {
                    if (loginPasswordInput.IsActive()) {
                        OnLoginClick();
                    }
                    else if (registerPasswordInput.IsActive()) {
                        OnRegisterClick();
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Escape)) {
                    ToSplashScreen();
                }
                break;

            case VisibleMenu.Register:
                if (Input.GetKeyDown(KeyCode.Tab)) {
                    if (registerUsernameInput.isFocused) {
                        registerPasswordInput.Select();
                    }
                    else if (registerPasswordInput.isFocused) {
                        registerRegisterButton.Select();
                    }
                    else if (registerRegisterButton.IsActive()) {
                        registerBackButton.Select();
                    }
                    else if (registerBackButton.IsActive()) {
                        registerUsernameInput.Select();
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Escape)) {
                    ToLoginMenu();
                }
                break;
        }

        // As the API call is running on a separate thread and Unity is not
        // technically thread safe, the actual game object has to be modified
        // here rather than doing it straight from the separate thread.
        if (isMessageErrorStyle) {
            infoMessage.color = Color.red;
            infoMessage.text = infoMessageText;
        }
        else {
            infoMessage.color = Color.blue;
            infoMessage.text = infoMessageText;
        }

        if (toLaunch) {
            toLaunch = false;
            GameManager.Instance().store.Dispatch(
                new OpenPlayingUI());
        }
    }

    // API call from login menu.
    public void OnLoginClick() {
        SetMessageClear();
        string loginUsernameText = loginUsernameInput.text;
        string loginPasswordText = loginPasswordInput.text;

        // Validate user input
        if (string.IsNullOrWhiteSpace(loginUsernameText) ||
            loginUsernameText.Length > maxUsernameLength) {
            SetMessageError("Invalid username\nMust be between 1 and 16 characters");
            isMessageErrorStyle = true;
            return;
        } else if (string.IsNullOrWhiteSpace(loginPasswordText))  {
            SetMessageError("Please enter a non-empty password");
            return;
        }

        UserCredentials userCredentials = new UserCredentials(loginUsernameText, loginPasswordText);
        UserCredentials returnUser;

        Task.Run( async () => {
            Task<APIResult<UserCredentials, JsonError>> fetchingResponse = api.AsyncAuthenticateUser(userCredentials);
            SetMessageInfo("Loading . . .");
            Debug.Log("Loading");

            try {
                APIResult<UserCredentials, JsonError> response = await fetchingResponse;
                returnUser = response.GetSuccess();
                GameManager.Instance().SetUserCredentials(returnUser);
                if (response.isSuccess()) {
                    toLaunch = true;
                } else {
                    SetMessageError(response.GetError().error);
                }
            } catch (Exception e) {
                SetMessageError(e.Message);
            }
        }).GetAwaiter().GetResult();
    }

    // API call from the register menu.
    public void OnRegisterClick() {
        SetMessageClear();
        string registerUsernameText = registerUsernameInput.text;
        string registerPasswordText = registerPasswordInput.text;

        // Validate user input
        if (string.IsNullOrWhiteSpace(registerUsernameText) ||
            registerUsernameText.Length > maxUsernameLength) {
            SetMessageError("Invalid username\nMust have between 1 and 16 characters");
            return;
        } else if (string.IsNullOrWhiteSpace(registerPasswordText)) {
            SetMessageError("Please enter a non-empty password");
            return;
        }

        UserCredentials returnUser;

        Task.Run(async () => {
            Task<APIResult<UserCredentials, JsonError>> fetchingResponse = api.AsyncRegisterUser(registerUsernameText, registerPasswordText);
            SetMessageInfo("Connecting . . .");

            try {
                APIResult<UserCredentials, JsonError> response = await fetchingResponse;
                returnUser = response.GetSuccess();
                if (response.isSuccess()) {
                    toLaunch = true;
                } else {
                    SetMessageError(response.GetError().error);
                }
            }
            catch (Exception e) {
                SetMessageError(e.Message);
            }
        }).GetAwaiter().GetResult();
    }

    // Splash screen is only accessible from the login menu.
    public void ToSplashScreen() {

        // Prevents error where register still selected after changing screen.
        loginUsernameInput.Select();

        SetMessageClear();
        splashScreen.GetComponent<CanvasGroup>().alpha = 1.0f;
        splashScreen.GetComponent<CanvasGroup>().interactable = true;
        loginMenu.GetComponent<CanvasGroup>().alpha = 0.0f;
        loginMenu.GetComponent<CanvasGroup>().interactable = false;
        blueprintLogo.transform.position -= new Vector3(0.0f, 150.0f, 0.0f);
        blueprintLogo.fontSize = 180;
        visibleMenu = VisibleMenu.SplashScreen;
    }

    // Login menu is accessible from either the splash screen or the register
    // menu.
    public void ToLoginMenu() {
        SetMessageClear();
        switch(visibleMenu) {
            case VisibleMenu.SplashScreen:
                loginMenu.GetComponent<CanvasGroup>().alpha = 1.0f;
                loginMenu.GetComponent<CanvasGroup>().interactable = true;
                splashScreen.GetComponent<CanvasGroup>().alpha = 0.0f;
                splashScreen.GetComponent<CanvasGroup>().interactable = false;
                blueprintLogo.transform.position += new Vector3(0.0f, 150.0f, 0.0f);
                blueprintLogo.fontSize = 100;
                loginUsernameInput.Select();
                visibleMenu = VisibleMenu.Login;
                break;

            case VisibleMenu.Register:
                loginMenu.GetComponent<CanvasGroup>().alpha = 1.0f;
                loginMenu.GetComponent<CanvasGroup>().interactable = true;
                registerMenu.GetComponent<CanvasGroup>().alpha = 0.0f;
                registerMenu.GetComponent<CanvasGroup>().interactable = false;
                visibleMenu = VisibleMenu.Login;
                loginUsernameInput.Select();
                break;

            default:
                break;
        }

        // Have to do this to stop one layer blocking the other.
        registerMenu.transform.position -= new Vector3(0.0f, 1000.0f, 0.0f);
    }

    // Register menu is only accessible from the login menu.
    public void ToRegister() {

        // Prevents error where register still selected after changing screen.
        loginUsernameInput.Select();

        SetMessageClear();
        registerMenu.GetComponent<CanvasGroup>().alpha = 1.0f;
        registerMenu.GetComponent<CanvasGroup>().interactable = true;
        loginMenu.GetComponent<CanvasGroup>().alpha = 0.0f;
        loginMenu.GetComponent<CanvasGroup>().interactable = false;
        registerUsernameInput.Select();
        visibleMenu = VisibleMenu.Register;

        // Have to do this to stop one layer blocking the other.
        registerMenu.transform.position += new Vector3(0.0f, 1000.0f, 0.0f);
    }

    private void SetMessageInfo(string msg) {
        isMessageErrorStyle = false;
        infoMessageText = msg;
    }

    private void SetMessageError(string msg) {
        isMessageErrorStyle = true;
        infoMessageText = msg;
    }

    private void SetMessageClear() {
        infoMessageText = "";
    }
}
