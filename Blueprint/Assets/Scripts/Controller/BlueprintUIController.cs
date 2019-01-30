using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controller {
    public class BlueprintUIController : MonoBehaviour {

        private Canvas blueprintCanvas;
        private Canvas cursorCanvas;

        void Start() {
            blueprintCanvas = GetComponent<Canvas> ();
            blueprintCanvas.enabled = false;
            cursorCanvas = GameObject.FindGameObjectWithTag("Cursor")
                .GetComponent<Canvas>();
        }

        void Update() {
            if (Input.GetKeyDown(KeyMapping.Blueprint)) {
                if (!blueprintCanvas.enabled) {
                    Time.timeScale = 0;
                    blueprintCanvas.enabled = true;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    cursorCanvas.enabled = false;
                } else {
                    Time.timeScale = 1;
                    blueprintCanvas.enabled = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    cursorCanvas.enabled = true;
                }
            }
        }
    }
}
