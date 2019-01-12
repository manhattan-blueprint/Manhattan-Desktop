using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controller {
    public class PlayerLookController : MonoBehaviour {
        [SerializeField] private Transform playerBody;
        private string mouseXInputName = "Mouse X";
        private string mouseYInputName = "Mouse Y";
        private float mouseSensitivity = 150;
        private float xAxisClamp = 0;
        private bool paused = false; 
        
        private void Awake() {
            LockCursor();
        }
            
        private void LockCursor() {
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update() {
            CameraRotation();
        }

        private void CameraRotation() {
            float mouseX = Input.GetAxis(mouseXInputName) * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis(mouseYInputName) * mouseSensitivity * Time.deltaTime;

            xAxisClamp += mouseY;

            if (xAxisClamp > 90.0f) {
                xAxisClamp = 90.0f;
                mouseY = 0.0f;
                ClampAxisRotationToValue (270.0f);
            } else if (xAxisClamp < -90.0f) {
                xAxisClamp = -90.0f;
                mouseY = 0.0f;
                ClampAxisRotationToValue (90.0f);
            }

            transform.Rotate(Vector3.left * mouseY);
            playerBody.Rotate(Vector3.up * mouseX);
        }

        private void ClampAxisRotationToValue(float value) {
            Vector3 eulerRotation = transform.eulerAngles;
            eulerRotation.x = value;
            transform.eulerAngles = eulerRotation;
        }
    }
}