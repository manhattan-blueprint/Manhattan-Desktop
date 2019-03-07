using System.Collections;
using System.Collections.Generic;
using Model;
using Model.Action;
using Model.Redux;
using Model.State;
using Model.Action;
using UnityEngine;

/* Attached to Inventory, listens for key press to show/hide panel */
namespace Controller {
    public class MenuController : MonoBehaviour, Subscriber<GameState> {
        private Canvas inventoryCanvas;
        private Canvas cursorCanvas;
        private GameManager gameManager;

        void Start() {
            inventoryCanvas = GetComponent<Canvas> ();
            inventoryCanvas.enabled = false;
            cursorCanvas = GameObject.FindGameObjectWithTag("Cursor").GetComponent<Canvas>();
            GameManager.Instance().store.Subscribe(this);
            gameManager = GameManager.Instance();
        }

        void Update() {
            if (Input.GetKeyDown(KeyMapping.Inventory)) {
                if (!inventoryCanvas.enabled) {
                    GameManager.Instance().store.Dispatch(new OpenInventoryUI());
                    PauseGame();
                } else {
                    GameManager.Instance().store.Dispatch(new CloseUI());
                }
            }
        }

        private void PauseGame() {
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
            cursorCanvas.enabled = true;
            GameObject.Find("HeldItem").transform.SetParent(GameObject.Find("HeldItemCanvas").transform);
        }

        public void StateDidUpdate(GameState state) {
            if (state.uiState.Selected == UIState.OpenUI.Inventory) {
                PauseGame();
            } else if (state.uiState.Selected == UIState.OpenUI.Playing) {
                ContinueGame();
            } else {
                throw new System.Exception("Not in expected state.");
            }
        }
    }
}