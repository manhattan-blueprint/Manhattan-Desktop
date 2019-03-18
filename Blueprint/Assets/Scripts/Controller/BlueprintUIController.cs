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

            Hide();
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

        private void Show() {
            blueprintCanvas.enabled = true;
        }

        private void Hide() {
            blueprintCanvas.enabled = false;
        }

        public void StateDidUpdate(GameState state) {
            if (state.uiState.Selected == UIState.OpenUI.Blueprint) {
                Show();
            } else if (state.uiState.Selected == UIState.OpenUI.Playing) {
                Hide();
            }
        }
    }
}
