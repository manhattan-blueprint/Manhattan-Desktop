using UnityEngine;
using Model.State;
using Model.Redux;

namespace View {
    public class Highlight : MonoBehaviour, Subscriber<UIState>{
        [SerializeField] private Color highlightColor;
        [SerializeField] private bool holdable;
        private Color initialColor;
        private Renderer rend;
        private bool paused;

        void Start () {
            rend = GetComponent<Renderer>();
            GameManager.Instance().uiStore.Subscribe(this);
            paused = false;
            initialColor = rend.material.color;
        }

        public Color GetColor() {
          return this.initialColor;
        }

        void OnMouseEnter() {
            if (!paused) {
              rend.material.color = highlightColor;
            }
        }

        private void OnMouseDown() {
            if (!paused) {
              if (!holdable) return;
              rend.material.color = Color.yellow;
            }
        }

        void OnMouseExit() {
            if (!paused) {
              rend.material.color = initialColor;
            }
        }

        void resetColor() {
          paused = true;
          rend.material.color = initialColor;
        }

        public void StateDidUpdate(UIState state) {
            if (state.Selected == UIState.OpenUI.Inventory) {
                resetColor();
            } else if (state.Selected == UIState.OpenUI.Playing) {
                paused = false;
            } else if (state.Selected == UIState.OpenUI.Pause) {
                resetColor();
            } else if (state.Selected == UIState.OpenUI.Exit) {
                resetColor();
            } else if (state.Selected == UIState.OpenUI.Login) {
                GameManager.Instance().uiStore.Unsubscribe(this);
            } else {
                throw new System.Exception("I haven't handled this case yet.");
            }
        }
    }
}
