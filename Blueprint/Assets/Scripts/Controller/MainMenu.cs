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
using Utils;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour, Subscriber<UIState> {
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
    private string infoMessageText;
    private bool isMessageErrorStyle;
    private VisibleMenu visibleMenu;
    private bool toLaunch;
    private ScreenProportions sp;
    private ManhattanAnimation animationManager;

    // Used to prevent the user from interacting during animation to prevent unexpected errors.
    private bool animating;

    private enum VisibleMenu {
        SplashScreen,
        Login,
        Register
    }

    void Start() {
        SetMessageClear();
        visibleMenu = VisibleMenu.SplashScreen;
        isMessageErrorStyle = false;
        toLaunch = false;

        sp = this.gameObject.AddComponent<ScreenProportions>();

        // Make "Press space..." button flash opacity.
        animationManager = this.gameObject.AddComponent<ManhattanAnimation>();
        animationManager.StartAppearanceAnimation(pressSpace.gameObject,
            Anim.OscillateAlpha, 0.6f, true, 0.3f);

        maxUsernameLength = 16;
        api = BlueprintAPI.WithBaseUrl("http://smithwjv.ddns.net");

        GameManager.Instance().uiStore.Subscribe(this);
    }

    public void StateDidUpdate(UIState state) {
        if (state.Selected == UIState.OpenUI.Playing) {
            GameManager.Instance().uiStore.Unsubscribe(this);
            SceneManager.LoadScene(SceneMapping.World);
        }
    }

    void Update() {
        if (animating) return;

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
                    } else if (loginPasswordInput.isFocused) {
                        loginLoginButton.Select();
                    } else if (loginLoginButton.IsActive()) {
                        loginRegisterButton.Select();
                    } else if (loginRegisterButton.IsActive()) {
                        loginUsernameInput.Select();
                    }
                }
                else if (Input.GetKeyDown(KeyCode.Return)) {
                    if (loginPasswordInput.IsActive()) {
                        OnLoginClick();
                    } else if (registerPasswordInput.IsActive()) {
                        OnRegisterClick();
                    }
                } else if (Input.GetKeyDown(KeyCode.Escape)) {
                    ToSplashScreen();
                }
                infoMessage.gameObject.transform.position =
                    loginLoginButton.gameObject.transform.position - new Vector3(0, Screen.height/10, 0);
                break;

            case VisibleMenu.Register:
                if (Input.GetKeyDown(KeyCode.Tab)) {
                    if (registerUsernameInput.isFocused) {
                        registerPasswordInput.Select();
                    } else if (registerPasswordInput.isFocused) {
                        registerRegisterButton.Select();
                    } else if (registerRegisterButton.IsActive()) {
                        registerBackButton.Select();
                    } else if (registerBackButton.IsActive()) {
                        registerUsernameInput.Select();
                    }
                } else if (Input.GetKeyDown(KeyCode.Escape)) {
                    ToLoginMenu();
                }
                infoMessage.gameObject.transform.position =
                    registerRegisterButton.gameObject.transform.position - new Vector3(0, Screen.height/10, 0);
                break;
        }

        // As the API call is running on a separate thread and Unity is not
        // technically thread safe, the actual game object has to be modified
        // here rather than doing it straight from the separate thread.
        if (isMessageErrorStyle) {
            infoMessage.color = Color.red;
            infoMessage.text = infoMessageText;
        } else {
            infoMessage.color = Color.blue;
            infoMessage.text = infoMessageText;
        }

        if (toLaunch) {
            toLaunch = false;
            GameManager.Instance().StartGame();
        }
    }

    // API call from login menu.
    public void OnLoginClick() {
        if (animating) return;
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
                if (String.Equals(e.Message, "Password invalid"))
                    SetMessageError("Incorrect password");
            }
        }).GetAwaiter().GetResult();
    }

    // API call from the register menu.
    public void OnRegisterClick() {
        if (animating) return;
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
            SetMessageInfo("Loading . . .");

            try {
                APIResult<UserCredentials, JsonError> response = await fetchingResponse;
                returnUser = response.GetSuccess();
                if (response.isSuccess()) {
                    toLaunch = true;
                } else {
                    SetMessageError(response.GetError().error);
                }
            } catch (Exception e) {
                SetMessageError(e.Message);
                if (String.Equals(e.Message, "Password invalid"))
                    SetMessageError("Password must contain between 5 and 24\ncharacters, have at least one upper and\nlower case letter, and at least one number");
            }
        }).GetAwaiter().GetResult();
    }

    // Splash screen is only accessible from the login menu.
    public void ToSplashScreen() {
        if (animating) return;
        ShowMenu(splashScreen);
        HideMenu(loginMenu);

        // Make Blueprint logo fly out.
        animationManager.StartMovementAnimation(blueprintLogo.gameObject,
            Anim.MoveToDecelerate, sp.ToV(new Vector3(0.0f, -0.2f, 0.0f)), 0.4f, false);
        animationManager.StartAppearanceAnimation(blueprintLogo.gameObject,
            Anim.Grow, 0.6f, false, (4.0f/3.0f), 0.0f);

        visibleMenu = VisibleMenu.SplashScreen;

        // Prevents error where register still selected after changing screen.
        loginUsernameInput.Select();
    }

    // Login menu is accessible from either the splash screen or the register
    // menu.
    public void ToLoginMenu() {
        if (animating) return;
        switch(visibleMenu) {
            case VisibleMenu.SplashScreen:
                HideMenu(splashScreen);
                loginUsernameInput.Select();
                visibleMenu = VisibleMenu.Login;

                // Make Blueprint logo fly in.
                animationManager.StartMovementAnimation(blueprintLogo.gameObject,
                    Anim.MoveToDecelerate, sp.ToV(new Vector3(0.0f, 0.2f, 0.0f)), 0.4f, false);
                animationManager.StartAppearanceAnimation(blueprintLogo.gameObject,
                    Anim.Grow, 0.6f, false, (3.0f/4.0f));
                break;

            case VisibleMenu.Register:
                HideMenu(registerMenu);
                loginUsernameInput.Select();
                visibleMenu = VisibleMenu.Login;
                break;
        }
        ShowMenu(loginMenu);
    }

    // Register menu is only accessible from the login menu.
    public void ToRegister() {
        if (animating) return;
        ShowMenu(registerMenu);
        HideMenu(loginMenu);
        registerUsernameInput.Select();
        visibleMenu = VisibleMenu.Register;
    }

    // Make an object fade out and fly downwards.
    private void HideMenu(GameObject obj) {
        SetMessageClear();
        obj.GetComponent<CanvasGroup>().interactable = false;
        triggerAnimating();
        animationManager.StartAppearanceAnimation(obj.gameObject, Anim.Dissappear, 0.3f, false, 0.0f, 0.0f);
        animationManager.StartMovementAnimation(obj.gameObject, Anim.MoveToDecelerate, sp.ToV(new Vector3(0.0f, -0.9f, 0.0f)), 0.4f, false);
    }

    // Make an object fade in and fly upwards.
    private void ShowMenu(GameObject obj) {
        SetMessageClear();
        obj.GetComponent<CanvasGroup>().interactable = true;
        triggerAnimating();
        animationManager.StartAppearanceAnimation(obj.gameObject, Anim.Appear, 0.3f, false, 0.0f, 0.2f);
        animationManager.StartMovementAnimation(obj.gameObject, Anim.MoveToDecelerate, sp.ToV(new Vector3(0.0f, 0.9f, 0.0f)), 0.4f, false);
    }

    private void triggerAnimating() {
        animating = true;
        Invoke("resetAnimating", 1.0f);
    }

    private void resetAnimating() {
        animating = false;
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
