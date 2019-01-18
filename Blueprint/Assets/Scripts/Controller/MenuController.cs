using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Attached to Inventory, listens for key press to show/hide panel */
namespace Controller {
    public class MenuController : MonoBehaviour {
        private Canvas inventoryCanvas;

        void Start() {
            inventoryCanvas = GetComponent<Canvas> ();
            inventoryCanvas.enabled = false;
        }

        void Update() {
            if (Input.GetKeyDown(KeyMapping.Inventory)) {
                if (!inventoryCanvas.enabled) {
                    PauseGame();
                } else {
                    ContinueGame();
                }
            }
        }

        private void PauseGame() {
            Time.timeScale = 0;
            inventoryCanvas.enabled = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void ContinueGame() {
            Time.timeScale = 1;
            inventoryCanvas.enabled = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}