using Model.State;
using Model.Action;
using Model.Redux;
using UnityEngine;

namespace Controller {
    public class BlueprintUIController : MonoBehaviour, Subscriber<UIState> {

        private Canvas blueprintCanvas;
        private Canvas cursorCanvas;

        void Start() {
            blueprintCanvas = GetComponent<Canvas> ();
            blueprintCanvas.enabled = false;
            cursorCanvas = GameObject.FindGameObjectWithTag("Cursor")
                .GetComponent<Canvas>();
            GameManager.Instance().uiStore.Subscribe(this);
        }

        void Update() {
            if (Input.GetKeyDown(KeyMapping.Blueprint)) {
                if (!blueprintCanvas.enabled) {
                    GameManager.Instance().uiStore.Dispatch(new OpenBlueprintUI());
                } else {
                    GameManager.Instance().uiStore.Dispatch(new CloseUI());
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
        
        public void StateDidUpdate(UIState state) {
            if (state.Selected == UIState.OpenUI.Blueprint) {
                PauseGame();
            } else if (state.Selected == UIState.OpenUI.Playing) {
                ContinueGame();
            }
        }
    }
}
