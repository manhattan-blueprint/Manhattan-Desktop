using System.Collections;
using System.Collections.Generic;
using Model;
using Model.Action;
using Model.Redux;
using Model.State;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
using UnityEngine.SceneManagement;

/* Attached to Inventory, listens for key press to show/hide panel */
namespace Controller {
    public class MenuController : MonoBehaviour, Subscriber<GameState> {
        private Canvas inventoryCanvas;
        private Canvas cursorCanvas;
        private Canvas pauseCanvas;
        private Canvas exitCanvas;

        void Start() {
            inventoryCanvas = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Canvas>();
            inventoryCanvas.enabled = false;
            cursorCanvas = GameObject.FindGameObjectWithTag("Cursor").GetComponent<Canvas>();
            pauseCanvas = GameObject.FindGameObjectWithTag("Pause").GetComponent<Canvas>();
            exitCanvas = GameObject.FindGameObjectWithTag("Exit").GetComponent<Canvas>();
            pauseCanvas.enabled = false;
            exitCanvas.enabled = false;
            GameManager.Instance().store.Subscribe(this);
        }

        void Update() {
            if (Input.GetKeyDown(KeyMapping.Inventory)) {
                if (!inventoryCanvas.enabled) {
                    GameManager.Instance().store.Dispatch(new OpenInventoryUI());
                } else {
                    GameManager.Instance().store.Dispatch(new CloseUI());
                }
            } else if (Input.GetKeyDown(KeyMapping.Pause)) {
                if (!pauseCanvas.enabled) {
                    GameManager.Instance().store.Dispatch(new OpenSettingsUI());
                } else {
                    GameManager.Instance().store.Dispatch(new CloseUI());
                }
            }
        }


        private void OpenInventory() {
            Time.timeScale = 0;
            inventoryCanvas.enabled = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            cursorCanvas.enabled = false;
            GameObject.Find("HeldItem").transform.SetParent(GameObject.Find("InventoryUICanvas").transform);
        }

        private void ContinueGame() {
            Time.timeScale = 1;
            inventoryCanvas.enabled = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            pauseCanvas.enabled = false;
            cursorCanvas.enabled = true;
            GameObject.Find("HeldItemCanvas").GetComponent<Canvas>().enabled = true;
            GameObject.Find("HeldItem").transform.SetParent(GameObject.Find("HeldItemCanvas").transform);
        }

        public void Logout() {
            GameManager.Instance().store.Dispatch(new Logout());
        }

        public void Exit() {
            Application.Quit();
        }

        public void ExitButton() {
            GameManager.Instance().store.Dispatch(new Exit());
        }

        private void ExitPrompt() {
            pauseCanvas.enabled = false;
            exitCanvas.enabled = true;
        }

        public void ExitCancel() {
            GameManager.Instance().store.Dispatch(new OpenSettingsUI());
        }

        private void PauseGame() {
            Time.timeScale = 0;
            pauseCanvas.enabled = true;
            exitCanvas.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            cursorCanvas.enabled = false;
            GameObject.Find("HeldItemCanvas").GetComponent<Canvas>().enabled = false;
        }

        public void StateDidUpdate(GameState state) {
            if (state.uiState.Selected == UIState.OpenUI.Inventory) {
                OpenInventory();
            } else if (state.uiState.Selected == UIState.OpenUI.Playing) {
                ContinueGame();
            } else if (state.uiState.Selected == UIState.OpenUI.Pause) {
                PauseGame();
            } else if (state.uiState.Selected == UIState.OpenUI.Login) {
                SceneManager.LoadScene(SceneMapping.MainMenu);
                GameManager.Instance().store.Unsubscribe(this);
            } else if (state.uiState.Selected == UIState.OpenUI.Exit) {
                ExitPrompt();
            } else {
                throw new System.Exception("Not in expected state.");
            }
        }
    }
}
