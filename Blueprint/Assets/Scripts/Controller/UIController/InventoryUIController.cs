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
    public class InventoryUIController : MonoBehaviour, Subscriber<GameState> {
        private Canvas inventoryCanvas;
        private Canvas cursorCanvas;
        private bool paused;

        void Start() {
            inventoryCanvas = GetComponent<Canvas> ();
            inventoryCanvas.enabled = false;
            cursorCanvas = GameObject.FindGameObjectWithTag("Cursor").GetComponent<Canvas>();
            GameManager.Instance().store.Subscribe(this);
            paused = false;
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
        }

        private void ContinueGame() {
            Time.timeScale = 1;
            inventoryCanvas.enabled = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            cursorCanvas.enabled = true;
        }

        public void StateDidUpdate(GameState state) {
            switch (state.uiState.Selected) {
                case UIState.OpenUI.Playing:
                    if (paused)
                        ContinueGame();
                    break;

                case UIState.OpenUI.InvPause:
                    if (!paused)
                        PauseGame();
                    break;

                case UIState.OpenUI.BluePause:
                    if (!paused)
                        PauseGame();
                    break;

                case UIState.OpenUI.MachPause:
                    if (!paused)
                        PauseGame();
                    break;

                default:
                    throw new System.Exception("Not in expected state.");
                    break;
            }
        }
    }
}
