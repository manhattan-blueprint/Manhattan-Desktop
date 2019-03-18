using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;
using Model.Action;
using Model.Redux;
using Model.State;

namespace View {
    public class Highlight : MonoBehaviour, Subscriber<GameState>{
        [SerializeField] private Color highlightColor;
        [SerializeField] private bool holdable;
        private Color initialColor;
        private Renderer rend;
        private bool paused;

        void Start () {
            rend = GetComponent<Renderer>();
            GameManager.Instance().store.Subscribe(this);
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

        public void StateDidUpdate(GameState state) {
            switch (state.uiState.Selected) {
              case UIState.OpenUI.Playing:
                  paused = false;
                  break;
              case UIState.OpenUI.Login:
                  GameManager.Instance().store.Unsubscribe(this);
                  break;
              default:
                  resetColor();
                  break;
              }
        }
    }
}
