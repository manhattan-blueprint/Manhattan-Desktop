using System;
using System.Diagnostics;
using System.Collections;
using Model;
using Model.Action;
using Model.Redux;
using Model.State;
using Service;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

/* Attached to Inventory, listens for key press to show/hide panel */
namespace Controller {
    public class MenuController : MonoBehaviour, Subscriber<UIState> {
        public bool gameOver;
        private Canvas inventoryCanvas;
        private Canvas heldCanvas;
        private Canvas cursorCanvas;
        private Canvas pauseCanvas;
        private Canvas logoutCanvas;
        private Canvas exitCanvas;
        private Canvas blueprintCanvas;
        private Canvas bindingsCanvas;
        private Canvas gateCanvas;
        private Canvas machineCanvas;
        private Canvas machineInventoryCanvas;
        private Canvas goalCanvas;
        private Image cursor;
        private SVGImage rmb;
        private const int rightButton = 1;

        void Start() {
            inventoryCanvas = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Canvas>();
            heldCanvas = GameObject.FindGameObjectWithTag("Held").GetComponent<Canvas>();
            cursorCanvas = GameObject.FindGameObjectWithTag("Cursor").GetComponent<Canvas>();
            pauseCanvas = GameObject.FindGameObjectWithTag("Pause").GetComponent<Canvas>();
            exitCanvas = GameObject.FindGameObjectWithTag("Exit").GetComponent<Canvas>();
            logoutCanvas = GameObject.FindGameObjectWithTag("Logout").GetComponent<Canvas>();
            blueprintCanvas = GameObject.FindGameObjectWithTag("Blueprint").GetComponent<Canvas>();
            bindingsCanvas = GameObject.FindGameObjectWithTag("Bindings").GetComponent<Canvas>();
            gateCanvas = GameObject.FindGameObjectWithTag("Gate").GetComponent<Canvas>();
            machineCanvas = GameObject.FindGameObjectWithTag("Machine").GetComponent<Canvas>();
            goalCanvas = GameObject.FindGameObjectWithTag("Goal").GetComponent<Canvas>();
            machineInventoryCanvas = GameObject.FindGameObjectWithTag("MachineInventory").GetComponent<Canvas>();
            cursor = GameObject.Find("Cursor Image").GetComponent<Image>();
            rmb = GameObject.Find("RMB Image").GetComponent<SVGImage>();

            inventoryCanvas.enabled = false;
            blueprintCanvas.enabled = false;
            gateCanvas.enabled = false;
            pauseCanvas.enabled = false;
            logoutCanvas.enabled = false;
            exitCanvas.enabled = false;
            bindingsCanvas.enabled = false;
            machineCanvas.enabled = false;
            goalCanvas.enabled = false;

            gameOver = false;
            rmb.enabled = false;

            GameManager.Instance().uiStore.Subscribe(this);
        }

        void Update() {
            if (gameOver) {
                return;
            }

            if (Input.GetKeyDown(KeyMapping.Inventory)) {
                if (!inventoryCanvas.enabled) {
                    GameManager.Instance().uiStore.Dispatch(new OpenInventoryUI());
                } else if (inventoryCanvas.enabled) {
                    GameManager.Instance().uiStore.Dispatch(new CloseUI());
                }
            } else if (Input.GetKeyDown(KeyMapping.Pause)) {
                if (machineCanvas.enabled || inventoryCanvas.enabled || blueprintCanvas.enabled || bindingsCanvas.enabled || gateCanvas.enabled || goalCanvas.enabled) {
                    GameManager.Instance().uiStore.Dispatch(new CloseUI());
                } else if (!pauseCanvas.enabled) {
                    GameManager.Instance().uiStore.Dispatch(new OpenSettingsUI());
                } else {
                    GameManager.Instance().uiStore.Dispatch(new CloseUI());
                }
            } else if (Input.GetKeyDown(KeyMapping.Blueprint)) {
                if (!blueprintCanvas.enabled) {
                    GameManager.Instance().uiStore.Dispatch(new OpenBlueprintUI());
                } else if (blueprintCanvas.enabled) {
                    GameManager.Instance().uiStore.Dispatch(new CloseUI());
                }
            } else if (Input.GetKeyDown(KeyMapping.Bindings)) {
                if (!bindingsCanvas.enabled) {
                    GameManager.Instance().uiStore.Dispatch(new OpenBindingsUI());
                }
            } else if (Input.GetMouseButtonDown(rightButton)) {
                if (rmb.enabled) {
                    GameManager.Instance().uiStore.Dispatch(new OpenGateUI());
                }
              }

            if (Input.GetKeyUp(KeyMapping.Bindings)) {
                if (bindingsCanvas.enabled) {
                    GameManager.Instance().uiStore.Dispatch(new CloseUI());
                }
            }
        }

        public void GameOver() {
            gameOver = true;
            GameManager.Instance().uiStore.Dispatch(new CloseUI());
            heldCanvas.enabled = false;
            cursorCanvas.enabled = false;
            pauseCanvas.enabled = false;
            GameObject.Find("Player").GetComponent<PlayerMoveController>().enabled = false;
            GameObject.Find("PlayerCamera").GetComponent<PlayerLookController>().enabled = false;
            Invoke("ToMainMenu", 30.0f);
        }

        private void ToMainMenu() {
            GameManager.Instance().uiStore.Dispatch(new OpenLoginUI());
        }

        private void OpenInventory() {
            Time.timeScale = 0;
            inventoryCanvas.enabled = true;
            pauseCanvas.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            cursorCanvas.enabled = false;
            heldCanvas.enabled = false;
        }

        private void OpenBlueprint() {
            Time.timeScale = 0;
            blueprintCanvas.enabled = true;
            pauseCanvas.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            cursorCanvas.enabled = false;
            heldCanvas.enabled = false;
        }

        private void OpenMachine() {
            Time.timeScale = 0;
            machineCanvas.enabled = true;
            machineInventoryCanvas.enabled = true;
            pauseCanvas.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            cursorCanvas.enabled = false;
            heldCanvas.enabled = false;
        }

        private void OpenGoal() {
            Time.timeScale = 0;
            goalCanvas.enabled = true;
            machineInventoryCanvas.enabled = true;
            pauseCanvas.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            cursorCanvas.enabled = false;
            heldCanvas.enabled = false;
        }

        private void OpenBindings() {
            Time.timeScale = 0;
            bindingsCanvas.enabled = true;
            pauseCanvas.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            cursorCanvas.enabled = false;
            heldCanvas.enabled = false;
        }

        private void OpenGate() {
            gateCanvas.enabled = true;
        }

        // Playing state
        private void ContinueGame() {
            Time.timeScale = 1;
            inventoryCanvas.enabled = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            pauseCanvas.enabled = false;
            blueprintCanvas.enabled = false;
            bindingsCanvas.enabled = false;
            gateCanvas.enabled = false;
            machineCanvas.enabled = false;
            goalCanvas.enabled = false;
            machineInventoryCanvas.enabled = false;
            cursorCanvas.enabled = true;
            heldCanvas.enabled = true;
            rmb.enabled = false;
            cursor.enabled = true;
        }

        // Logout button from the pause menu
        public void LogoutButton() {
            GameManager.Instance().uiStore.Dispatch(new Logout());
        }

        // Are you sure you would like to log out?
        private void LogoutPrompt() {
            pauseCanvas.enabled = false;
            logoutCanvas.enabled = true;
        }

        public void Logout() {
            GameManager.Instance().uiStore.Dispatch(new OpenLoginUI());
        }

        public void LogoutCancel() {
            GameManager.Instance().uiStore.Dispatch(new CloseUI());
        }

        // Exit button from the pause menu
        public void ExitButton() {
            GameManager.Instance().uiStore.Dispatch(new Exit());
        }

        // Are you sure you would like to quit?
        private void ExitPrompt() {
            pauseCanvas.enabled = false;
            exitCanvas.enabled = true;
        }

        public void Exit() {
            Application.Quit();
        }

        public void ExitCancel() {
            GameManager.Instance().uiStore.Dispatch(new CloseUI());
        }

        private void EnableMouse() {
            gateCanvas.enabled = false;
            cursor.enabled = false;
            rmb.enabled = true;
        }

        private void PauseGame() {
            Time.timeScale = 0;
            pauseCanvas.enabled = true;
            exitCanvas.enabled = false;
            logoutCanvas.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            cursorCanvas.enabled = false;
            heldCanvas.enabled = false;
        }

        public void StateDidUpdate(UIState state) {
            switch (state.Selected) {
                case UIState.OpenUI.Inventory:
                    OpenInventory();
                    break;
                case UIState.OpenUI.Playing:
                    ContinueGame();
                    break;
                case UIState.OpenUI.Blueprint:
                    OpenBlueprint();
                    break;
                case UIState.OpenUI.Bindings:
                    OpenBindings();
                    break;
                case UIState.OpenUI.Gate:
                    OpenGate();
                    break;
                case UIState.OpenUI.Mouse:
                    EnableMouse();
                    break;
                case UIState.OpenUI.Machine:
                    OpenMachine();
                    break;
                case UIState.OpenUI.Goal:
                    OpenGoal();
                    break;
                case UIState.OpenUI.Pause:
                    PauseGame();
                    break;
                case UIState.OpenUI.Logout:
                    LogoutPrompt();
                    break;
                case UIState.OpenUI.Login:
                    GameState logoutGameState = new GameState(GameManager.Instance().mapStore.GetState(),
                        GameManager.Instance().heldItemStore.GetState(),
                        GameManager.Instance().inventoryStore.GetState(),
                        GameManager.Instance().machineStore.GetState());

                    StartCoroutine(BlueprintAPI.SaveGameState(GameManager.Instance().GetAccessToken(), logoutGameState, result => {
                        if (result.isSuccess()) {
                            // Reset timescale so welcome/login UI works when the main menu scene is reloaded
                            Time.timeScale = 1;
                            GameManager.Instance().ResetGame();
                            SceneManager.LoadScene(SceneMapping.MainMenu);
                        } else {
                            // TODO: Handle failure via UI?
                            throw new Exception("Couldn't save game " + result.GetError());
                        }
                    }));
                    break;
                case UIState.OpenUI.Exit:
                    GameState exitGameState = new GameState(GameManager.Instance().mapStore.GetState(),
                        GameManager.Instance().heldItemStore.GetState(),
                        GameManager.Instance().inventoryStore.GetState(),
                        GameManager.Instance().machineStore.GetState());

                    StartCoroutine(BlueprintAPI.SaveGameState(GameManager.Instance().GetAccessToken(), exitGameState, result => {
                        if (result.isSuccess()) {
                            ExitPrompt();
                        } else {
                            // TODO: Handle failure via UI?
                        }
                    }));

                    break;
                default:
                    throw new Exception("Not in expected state.");
            }
        }
    }
}
