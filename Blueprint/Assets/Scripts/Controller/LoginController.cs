using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using Controller;
using Model.Action;
using Model.Redux;
using Model.State;
using Service;
using Service.Response;
using Utils;

public class LoginController: MonoBehaviour, Subscriber<UIState> {
    [SerializeField] private Text infoMessage;
    [SerializeField] private GameObject fadeIn;

    [SerializeField] private GameObject splashScreen;
    [SerializeField] private SVGImage blueprintLogo;
    [SerializeField] private Text pressSpace;

    [SerializeField] private GameObject loginMenu;
    [SerializeField] private InputField loginUsernameInput;
    [SerializeField] private InputField loginPasswordInput;
    [SerializeField] private Button loginLoginButton;
    [SerializeField] private Button loginRegisterButton;
    [SerializeField] private Text loginRegisterText;

    [SerializeField] private GameObject registerMenu;
    [SerializeField] private InputField registerUsernameInput;
    [SerializeField] private InputField registerPasswordInput;
    [SerializeField] private Button registerRegisterButton;
    [SerializeField] private Button registerBackButton;
    [SerializeField] private Text registerLoginText;

    private int maxUsernameLength = 16;
    private string infoMessageText;
    private bool isMessageErrorStyle;
    private VisibleMenu visibleMenu;
    private bool toLaunch;
    private ScreenProportions sp;
    private ManhattanAnimation animationManager;
    private string passwordRegex = "(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{5,16}";

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
        animating = false;
        Time.timeScale = 1;

        sp = this.gameObject.AddComponent<ScreenProportions>();

        // Make "Press space..." button flash opacity.
        animationManager = this.gameObject.AddComponent<ManhattanAnimation>();
        animationManager.StartAppearanceAnimation(pressSpace.gameObject,
            Anim.OscillateAlpha, 0.6f, true, 0.3f);

        // Fade whole menu fade in
        animationManager.StartAppearanceAnimation(fadeIn.gameObject,
            Anim.Disappear, 2.0f, true, 1.0f, 0.5f);

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

        if (toLaunch) {
            toLaunch = false;
            GameManager.Instance().uiStore.Dispatch(new OpenPlayingUI());
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
    }

    private void PreloadGame() {
        AccessToken accessToken = GameManager.Instance().GetAccessToken();
        // Fetch desktop state
        StartCoroutine(BlueprintAPI.GetGameState(accessToken, desktopResult => {
            if (!desktopResult.isSuccess()) {
                SetMessageError("Could not load saved game: " + desktopResult.GetError());
                return;
            }

            // Fetch inventory
            StartCoroutine(BlueprintAPI.GetInventory(accessToken, inventoryResult => {
                if (!inventoryResult.isSuccess()) {
                    SetMessageError("Could not fetch inventory: " + inventoryResult.GetError());
                    return;
                }

                // Delete inventory
                StartCoroutine(BlueprintAPI.DeleteInventory(accessToken, deleteResult => {
                    if (!deleteResult.isSuccess()) {
                        SetMessageError("Could not remove inventory: " + deleteResult.GetError());
                        return;
                    }
                    
                    // Fetch schema
                    StartCoroutine(BlueprintAPI.GetSchema(schemaResult => {
                        if (!schemaResult.isSuccess()) {
                            SetMessageError("Could not fetch schema: " + schemaResult.GetError());
                            return;
                        }
                        
                        // Fetch completed blueprints
                        StartCoroutine(BlueprintAPI.GetCompletedBlueprints(accessToken, blueprintsResult => {
                            if (!blueprintsResult.isSuccess()) {
                                SetMessageError("Could not fetch completed blueprints: " + blueprintsResult.GetError());
                                return;
                            }
                            
                            GameManager.Instance()
                                .ConfigureGame(schemaResult.GetSuccess(), desktopResult.GetSuccess(), inventoryResult.GetSuccess().items);
                            GameManager.Instance().completedBlueprints = blueprintsResult.GetSuccess().blueprints;
                            toLaunch = true;
                        }));
                    }));
                }));
            }));
        }));
    }

    public void OnLoginClick() {
        if (animating) return;
        SetMessageClear();
        string loginUsernameText = loginUsernameInput.text;
        string loginPasswordText = loginPasswordInput.text;

        // Validate username
        if (string.IsNullOrWhiteSpace(loginUsernameText) ||
            loginUsernameText.Length > maxUsernameLength) {
            SetMessageError("Invalid username\nMust be between 1 and 16 characters");
            return;
        } 
        
        // Validate password
        if (string.IsNullOrWhiteSpace(loginPasswordText))  {
            SetMessageError("Please enter a non-empty password");
            return;
        }
        
        loginLoginButton.gameObject.SetActive(false);
        loginRegisterButton.gameObject.SetActive(false);
        loginRegisterText.gameObject.SetActive(false);
        StartCoroutine(BlueprintAPI.Login(loginUsernameText, loginPasswordText, result => {
            if (result.isSuccess()) {
                GameManager.Instance().SetAccessToken(result.GetSuccess());
                PreloadGame();
            } else {
                SetMessageError(result.GetError());
                loginLoginButton.gameObject.SetActive(true);
                loginRegisterButton.gameObject.SetActive(true);
                loginRegisterText.gameObject.SetActive(true);
            }
        }));
    }

    public void OnRegisterClick() {
        if (animating) return;
        SetMessageClear();
        string registerUsernameText = registerUsernameInput.text;
        string registerPasswordText = registerPasswordInput.text;

        // Validate username
        if (string.IsNullOrWhiteSpace(registerUsernameText) ||
            registerUsernameText.Length > maxUsernameLength) {
            SetMessageError("Invalid username\nMust have between 1 and 16 characters");
            return;
        } 
        
        if (string.IsNullOrWhiteSpace(registerPasswordText) || !new Regex(passwordRegex).IsMatch(registerPasswordText)) {
            SetMessageError("Your password must be between 5 and 16 characters, with at least 1 number, 1 uppercase and 1 lowercase letter.");
            return;
        }

        registerRegisterButton.gameObject.SetActive(false);
        registerBackButton.gameObject.SetActive(false);
        registerLoginText.gameObject.SetActive(false);
        StartCoroutine(BlueprintAPI.Register(registerUsernameText, registerPasswordText, result => {
            if (result.isSuccess()) {
                GameManager.Instance().SetAccessToken(result.GetSuccess());
                PreloadGame();
            } else {
                SetMessageError(result.GetError());
                registerRegisterButton.gameObject.SetActive(true);
                registerBackButton.gameObject.SetActive(true);
                registerLoginText.gameObject.SetActive(true);
            }
        }));
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

    // Login menu is accessible from either the splash screen or the register menu.
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
        animationManager.StartAppearanceAnimation(obj.gameObject, Anim.Disappear, 0.3f, false, 0.0f, 0.0f);
        animationManager.StartMovementAnimation(obj.gameObject, Anim.MoveToDecelerate, sp.ToV(new Vector3(0.0f, -0.9f, 0.0f)), 0.4f);
    }

    // Make an object fade in and fly upwards.
    private void ShowMenu(GameObject obj) {
        SetMessageClear();
        obj.GetComponent<CanvasGroup>().interactable = true;
        triggerAnimating();
        animationManager.StartAppearanceAnimation(obj.gameObject, Anim.Appear, 0.3f, false, 0.0f, 0.2f);
        animationManager.StartMovementAnimation(obj.gameObject, Anim.MoveToDecelerate, sp.ToV(new Vector3(0.0f, 0.9f, 0.0f)), 0.4f);
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
