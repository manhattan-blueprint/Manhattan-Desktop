using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Attached to player camera and controls camera movement */
namespace Controller {
    public class PlayerLookController : MonoBehaviour {
        [SerializeField] private Transform playerBody;
        private const string MouseXInputName = "Mouse X";
        private const string MouseYInputName = "Mouse Y";
        private const float mouseSensitivity = 150;
        private float xAxisClamp = 0;
        public bool active;

        private void Awake() {
            LockCursor();
            active = true;
        }

        private void LockCursor() {
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update() {
            if (active) CameraRotation();
        }

        private void CameraRotation() {
            float mouseX = Input.GetAxis(MouseXInputName) * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis(MouseYInputName) * mouseSensitivity * Time.deltaTime;

            xAxisClamp += mouseY;

            if (xAxisClamp > 90.0f) {
                xAxisClamp = 90.0f;
                mouseY = 0.0f;
                ClampAxisRotationToValue(270.0f);
            } else if (xAxisClamp < -90.0f) {
                xAxisClamp = -90.0f;
                mouseY = 0.0f;
                ClampAxisRotationToValue(90.0f);
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
