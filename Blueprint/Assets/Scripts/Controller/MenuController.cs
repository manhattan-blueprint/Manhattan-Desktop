using Model.Action;
using Model.Redux;
using Model.State;
using UnityEngine;
using UnityEngine.SceneManagement;

/* Attached to Inventory, listens for key press to show/hide panel */
namespace Controller {
    public class MenuController : MonoBehaviour, Subscriber<UIState> {
        private Canvas inventoryCanvas;
        private Canvas heldCanvas;
        private Canvas cursorCanvas;
        private Canvas pauseCanvas;
        private Canvas logoutCanvas;
        private Canvas exitCanvas;
        private Canvas blueprintCanvas;
        private Canvas machineCanvas;
        private Canvas machineInventoryCanvas;
        private bool multiCanvas;

        void Start() {
            inventoryCanvas = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Canvas>();
            heldCanvas = GameObject.FindGameObjectWithTag("Held").GetComponent<Canvas>();
            cursorCanvas = GameObject.FindGameObjectWithTag("Cursor").GetComponent<Canvas>();
            pauseCanvas = GameObject.FindGameObjectWithTag("Pause").GetComponent<Canvas>();
            exitCanvas = GameObject.FindGameObjectWithTag("Exit").GetComponent<Canvas>();
            logoutCanvas = GameObject.FindGameObjectWithTag("Logout").GetComponent<Canvas>();
            blueprintCanvas = GameObject.FindGameObjectWithTag("Blueprint").GetComponent<Canvas>();
            machineCanvas = GameObject.FindGameObjectWithTag("Machine").GetComponent<Canvas>();
            machineInventoryCanvas = GameObject.FindGameObjectWithTag("MachineInventory").GetComponent<Canvas>();

            inventoryCanvas.enabled = false;
            blueprintCanvas.enabled = false;
            pauseCanvas.enabled = false;
            logoutCanvas.enabled = false;
            exitCanvas.enabled = false;
            machineCanvas.enabled = false;

            multiCanvas = false;
            
            // TODO: remove when finished testing
            GameManager.Instance().inventoryStore.Dispatch(new AddItemToInventory(11, 3, "Furnace"));
            GameManager.Instance().inventoryStore.Dispatch(new AddItemToInventory(4, 1, "Iron ore"));
            GameManager.Instance().inventoryStore.Dispatch(new AddItemToInventory(12, 1, "Charcoal"));

            GameManager.Instance().uiStore.Subscribe(this);
        }

        void Update() {
            if (Input.GetKeyDown(KeyMapping.Inventory)) {
                if (!inventoryCanvas.enabled) {
                    GameManager.Instance().uiStore.Dispatch(new OpenInventoryUI());
                } else if (inventoryCanvas.enabled && !multiCanvas){
                    GameManager.Instance().uiStore.Dispatch(new CloseUI());
                }
            } else if (Input.GetKeyDown(KeyMapping.Pause)) {
                if (machineCanvas.enabled || inventoryCanvas.enabled || blueprintCanvas.enabled) {
                    GameManager.Instance().uiStore.Dispatch(new CloseUI());
                } else if (!pauseCanvas.enabled) {
                    GameManager.Instance().uiStore.Dispatch(new OpenSettingsUI());
                } else {
                    GameManager.Instance().uiStore.Dispatch(new CloseUI());
                }
            } else if (Input.GetKeyDown(KeyMapping.Blueprint)) {
                if (!blueprintCanvas.enabled) {
                    GameManager.Instance().uiStore.Dispatch(new OpenBlueprintUI());
                } else if (blueprintCanvas.enabled && !multiCanvas){
                    GameManager.Instance().uiStore.Dispatch(new CloseUI());
                }
            } 
        }

        private void OpenInventory() {
            Time.timeScale = 0;
            inventoryCanvas.enabled = true;
            pauseCanvas.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            cursorCanvas.enabled = false;
            heldCanvas.enabled = false;
        }

        private void OpenBlueprint() {
            Time.timeScale = 0;
            blueprintCanvas.enabled = true;
            pauseCanvas.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            cursorCanvas.enabled = false;
            heldCanvas.enabled = false;
        }
        
        private void OpenMachine() {
            Time.timeScale = 0;
            machineCanvas.enabled = true;
            machineInventoryCanvas.enabled = true;
            pauseCanvas.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            cursorCanvas.enabled = false;
            heldCanvas.enabled = false;
        }

        // Playing state
        private void ContinueGame() {
            Time.timeScale = 1;
            inventoryCanvas.enabled = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            pauseCanvas.enabled = false;
            blueprintCanvas.enabled = false;
            machineCanvas.enabled = false;
            machineInventoryCanvas.enabled = false;
            cursorCanvas.enabled = true;
            heldCanvas.enabled = true;
        }

        // Logout button from the pause menu
        public void LogoutButton() {
            GameManager.Instance().uiStore.Dispatch(new Logout());
        }

        // Are you sure you would like to log out?
        private void LogoutPrompt() {
            pauseCanvas.enabled = false;
            logoutCanvas.enabled = true;
        }

        public void Logout() {
            GameManager.Instance().uiStore.Dispatch(new OpenLoginUI());
        }

        public void LogoutCancel() {
            GameManager.Instance().uiStore.Dispatch(new CloseUI());
        }

        // Exit button from the pause menu
        public void ExitButton() {
            GameManager.Instance().uiStore.Dispatch(new Exit());
        }

        // Are you sure you would like to quit?
        private void ExitPrompt() {
            pauseCanvas.enabled = false;
            exitCanvas.enabled = true;
        }

        public void Exit() {
            Application.Quit();
        }

        public void ExitCancel() {
            GameManager.Instance().uiStore.Dispatch(new CloseUI());
        }

        private void PauseGame() {
            Time.timeScale = 0;
            pauseCanvas.enabled = true;
            exitCanvas.enabled = false;
            logoutCanvas.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            cursorCanvas.enabled = false;
            heldCanvas.enabled = false;
        }

        // TODO: REFACTOR NOW WE DONT ALLOW MULTIPLE CANVAS
        public void StateDidUpdate(UIState state) {
            switch (state.Selected) {
              case UIState.OpenUI.Inventory:
                  multiCanvas = false;
                  OpenInventory();
                  break;
              case UIState.OpenUI.Playing:
                  ContinueGame();
                  break;
              case UIState.OpenUI.Blueprint:
                  multiCanvas = false;
                  OpenBlueprint();
                  break;
              case UIState.OpenUI.Machine:
                  multiCanvas = false;
                  OpenMachine();
                  break;
              case UIState.OpenUI.Pause:
                  multiCanvas = false;
                  PauseGame();
                  break;
              case UIState.OpenUI.InvPause:
              case UIState.OpenUI.BluePause:
              case UIState.OpenUI.MachPause:
                  multiCanvas = true;
                  PauseGame();
                  break;
              case UIState.OpenUI.Logout:
                  multiCanvas = false;
                  LogoutPrompt();
                  break;
              case UIState.OpenUI.InvLogout:
              case UIState.OpenUI.BlueLogout:
              case UIState.OpenUI.MachLogout:
                  multiCanvas = true;
                  LogoutPrompt();
                  break;
              case UIState.OpenUI.InvExit:
              case UIState.OpenUI.BlueExit:
              case UIState.OpenUI.MachExit:
                  multiCanvas = true;
                  ExitPrompt();
                  break;
              case UIState.OpenUI.Login:
                  SceneManager.LoadScene(SceneMapping.MainMenu);
                  GameManager.Instance().uiStore.Unsubscribe(this);
                  break;
              case UIState.OpenUI.Exit:
                  multiCanvas = false;
                  ExitPrompt();
                  break;
              default:
                  throw new System.Exception("Not in expected state.");
            }
        }
    }
}
