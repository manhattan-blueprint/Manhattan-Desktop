using System.Collections;
using System.Collections.Generic;
using Model;
using Model.Action;
using Model.Redux;
using Model.State;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

/* Attached to Inventory, listens for key press to show/hide panel */
namespace Controller {
    public class MenuController : MonoBehaviour, Subscriber<GameState> {
        private Canvas inventoryCanvas;
        private Canvas cursorCanvas;
        private Canvas pauseCanvas;

        void Start() {
            inventoryCanvas = GetComponent<Canvas> ();
            inventoryCanvas.enabled = false;
            cursorCanvas = GameObject.FindGameObjectWithTag("Cursor").GetComponent<Canvas>();
            pauseCanvas = GameObject.FindGameObjectWithTag("Pause").GetComponent<Canvas>();
            pauseCanvas.enabled = false;
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

        private void BlurScreen() {
          GameObject.Find("PlayerCamera").GetComponent<Blur>().enabled = true;
        }

        private void UnblurScreen() {
          GameObject.Find("PlayerCamera").GetComponent<Blur>().enabled = false;
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
            UnblurScreen();
        }

        private void PauseGame() {
            Time.timeScale = 0;
            pauseCanvas.enabled = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            cursorCanvas.enabled = false;
            GameObject.Find("HeldItemCanvas").GetComponent<Canvas>().enabled = false;
            BlurScreen();
        }

        public void StateDidUpdate(GameState state) {
            if (state.uiState.Selected == UIState.OpenUI.Inventory) {
                OpenInventory();
            } else if (state.uiState.Selected == UIState.OpenUI.Playing) {
                ContinueGame();
            } else if (state.uiState.Selected == UIState.OpenUI.Pause) {
                PauseGame();
            } else {
                throw new System.Exception("Not in expected state.");
            }
        }
    }
}
