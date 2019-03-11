using System.Collections;
using System.Collections.Generic;
using Model;
using Model.Action;
using Model.Redux;
using Model.State;
using UnityEngine;
using UnityEngine.SceneManagement;

/* Attached to Inventory, listens for key press to show/hide panel */
namespace Controller {
    public class MenuController : MonoBehaviour, Subscriber<GameState> {
        private Canvas inventoryCanvas;
        private Canvas cursorCanvas;
        private Canvas pauseCanvas;
        private Canvas logoutCanvas;
        private Canvas exitCanvas;
        private Canvas blueprintCanvas;
        private bool multiCanvas;

        void Start() {
            inventoryCanvas = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Canvas>();
            cursorCanvas = GameObject.FindGameObjectWithTag("Cursor").GetComponent<Canvas>();
            pauseCanvas = GameObject.FindGameObjectWithTag("Pause").GetComponent<Canvas>();
            exitCanvas = GameObject.FindGameObjectWithTag("Exit").GetComponent<Canvas>();
            logoutCanvas = GameObject.FindGameObjectWithTag("Logout").GetComponent<Canvas>();
            blueprintCanvas = GameObject.FindGameObjectWithTag("Blueprint").GetComponent<Canvas>();

            inventoryCanvas.enabled = false;
            blueprintCanvas.enabled = false;
            pauseCanvas.enabled = false;
            logoutCanvas.enabled = false;
            exitCanvas.enabled = false;

            multiCanvas = false;

            GameManager.Instance().store.Subscribe(this);
        }

        void Update() {
            if (Input.GetKeyDown(KeyMapping.Inventory)) {
                if (!inventoryCanvas.enabled) {
                    GameManager.Instance().store.Dispatch(new OpenInventoryUI());
                } else if (inventoryCanvas.enabled && !multiCanvas){
                    GameManager.Instance().store.Dispatch(new CloseUI());
                }
            } else if (Input.GetKeyDown(KeyMapping.Pause)) {
                if (!pauseCanvas.enabled) {
                    GameManager.Instance().store.Dispatch(new OpenSettingsUI());
                } else {
                    GameManager.Instance().store.Dispatch(new CloseUI());
                }
            }  else if (Input.GetKeyDown(KeyMapping.Blueprint)) {
                if (!blueprintCanvas.enabled) {
                    GameManager.Instance().store.Dispatch(new OpenBlueprintUI());
                } else if (blueprintCanvas.enabled && !multiCanvas){
                    GameManager.Instance().store.Dispatch(new CloseUI());
                }
            }
        }

        private void OpenInventory() {
            Time.timeScale = 0;
            inventoryCanvas.enabled = true;
            pauseCanvas.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            cursorCanvas.enabled = false;
            GameObject.Find("HeldItem").transform.SetParent(GameObject.Find("InventoryUICanvas").transform);
        }

        private void OpenBlueprint() {
            Time.timeScale = 0;
            blueprintCanvas.enabled = true;
            pauseCanvas.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            cursorCanvas.enabled = false;
            GameObject.Find("HeldItem").transform.SetParent(GameObject.Find("BlueprintUICanvas").transform);
        }

        // Playing state
        private void ContinueGame() {
            Time.timeScale = 1;
            inventoryCanvas.enabled = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            pauseCanvas.enabled = false;
            blueprintCanvas.enabled = false;
            cursorCanvas.enabled = true;
            GameObject.Find("HeldItemCanvas").GetComponent<Canvas>().enabled = true;
            GameObject.Find("HeldItem").transform.SetParent(GameObject.Find("HeldItemCanvas").transform);
        }

        // Logout button from the pause menu
        public void LogoutButton() {
            GameManager.Instance().store.Dispatch(new Logout());
        }

        // Are you sure you would like to log out?
        private void LogoutPrompt() {
            pauseCanvas.enabled = false;
            logoutCanvas.enabled = true;
        }

        public void Logout() {
            GameManager.Instance().store.Dispatch(new OpenLoginUI());
        }

        public void LogoutCancel() {
            GameManager.Instance().store.Dispatch(new CloseUI());
        }

        // Exit button from the pause menu
        public void ExitButton() {
            GameManager.Instance().store.Dispatch(new Exit());
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
            GameManager.Instance().store.Dispatch(new CloseUI());
        }

        private void PauseGame() {
            Time.timeScale = 0;
            pauseCanvas.enabled = true;
            exitCanvas.enabled = false;
            logoutCanvas.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            cursorCanvas.enabled = false;
            GameObject.Find("HeldItemCanvas").GetComponent<Canvas>().enabled = false;
        }

        public void StateDidUpdate(GameState state) {
            switch (state.uiState.Selected) {
              case UIState.OpenUI.Inventory:
                  multiCanvas = false;
                  OpenInventory();
                  break;
              case UIState.OpenUI.Playing:
                  ContinueGame();
                  break;
              case UIState.OpenUI.Blueprint:
                  multiCanvas = false;
                  OpenBlueprint();
                  break;
              case UIState.OpenUI.Pause:
                  multiCanvas = false;
                  PauseGame();
                  break;
              case UIState.OpenUI.InvPause:
              case UIState.OpenUI.BluePause:
              case UIState.OpenUI.MachPause:
                  multiCanvas = true;
                  PauseGame();
                  break;
              case UIState.OpenUI.Logout:
                  multiCanvas = false;
                  LogoutPrompt();
                  break;
              case UIState.OpenUI.InvLogout:
              case UIState.OpenUI.BlueLogout:
              case UIState.OpenUI.MachLogout:
                  multiCanvas = true;
                  LogoutPrompt();
                  break;
              case UIState.OpenUI.InvExit:
              case UIState.OpenUI.BlueExit:
              case UIState.OpenUI.MachExit:
                  multiCanvas = true;
                  ExitPrompt();
                  break;
              case UIState.OpenUI.Login:
                  SceneManager.LoadScene(SceneMapping.MainMenu);
                  GameManager.Instance().store.Unsubscribe(this);
                  break;
              case UIState.OpenUI.Exit:
                  multiCanvas = false;
                  ExitPrompt();
                  break;
              default:
                  throw new System.Exception("Not in expected state.");
            }
        }
    }
}
