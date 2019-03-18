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

        void Start() {
            blueprintCanvas = GetComponent<Canvas> ();
            blueprintCanvas.enabled = false;
            GameManager.Instance().store.Subscribe(this);

            // HexTile = Resources.Load("inventory_slot", typeof(Sprite)) as Sprite;
            // borderSprite = Resources.Load("slot_border", typeof(Sprite)) as Sprite;

        }

        void Update() {
            if (Input.GetKeyDown(KeyMapping.Blueprint)) {
                if (!blueprintCanvas.enabled) {
                    GameManager.Instance().store.Dispatch(new OpenBlueprintUI());
                } else {
                    GameManager.Instance().store.Dispatch(new CloseUI());
                }
            } else if (Input.GetKeyDown(KeyMapping.Escape) && blueprintCanvas.enabled) {
                GameManager.Instance().store.Dispatch(new CloseUI());
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
            } else if (state.uiState.Selected == UIState.OpenUI.Playing) {
                ContinueGame();
            }
        }
    }
}
