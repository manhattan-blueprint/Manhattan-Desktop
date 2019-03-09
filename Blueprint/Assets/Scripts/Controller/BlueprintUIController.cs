using System.Collections;
using System.Collections.Generic;
using Model;
using Model.Action;
using Model.Redux;
using Model.State;
using UnityEngine;

namespace Controller {
    public class BlueprintUIController : MonoBehaviour, Subscriber<GameState> {

        private Canvas blueprintCanvas;
        private Canvas cursorCanvas;

        void Start() {
            blueprintCanvas = GetComponent<Canvas> ();
            blueprintCanvas.enabled = false;
            cursorCanvas = GameObject.FindGameObjectWithTag("Cursor")
                .GetComponent<Canvas>();
            GameManager.Instance().store.Subscribe(this);
        }

        void Update() {
            if (Input.GetKeyDown(KeyMapping.Blueprint)) {
                if (!blueprintCanvas.enabled) {
                    GameManager.Instance().store.Dispatch(new OpenBlueprintUI());
                } else {
                    GameManager.Instance().store.Dispatch(new CloseUI());
                }
            }
        }

        private void PauseGame() {
            Time.timeScale = 0;
            blueprintCanvas.enabled = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            cursorCanvas.enabled = false;
        }

        private void ContinueGame() {
            Time.timeScale = 1;
            blueprintCanvas.enabled = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            cursorCanvas.enabled = true;
        }

        public void StateDidUpdate(GameState state) {
            if (state.uiState.Selected == UIState.OpenUI.Blueprint) {
                PauseGame();
            } else if (state.uiState.Selected == UIState.OpenUI.Login) {
                GameManager.Instance().store.Unsubscribe(this);
            } else if (state.uiState.Selected == UIState.OpenUI.Playing) {
                ContinueGame();
            }
        }
    }
}
