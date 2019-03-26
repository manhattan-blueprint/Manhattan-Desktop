﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Attached to player and controls movement around map */
namespace Controller {
    public class PlayerMoveController : MonoBehaviour {
        [SerializeField] private AnimationCurve jumpFalloff;
        private const string horizontalInputName = "Horizontal";
        private const string verticalInputName = "Vertical";
        private float movementSpeed = 6.0f;
        private const float jumpMultiplier = 6.0f;

        public bool active;

        private CharacterController charController;
        private bool isJumping;

        private void Awake() {
            charController = GetComponent<CharacterController>();
            active = true;
        }

        private void Update() {
            if (active) PlayerMovement();
        }

        private void PlayerMovement() {
            float horizInput = Input.GetAxis(horizontalInputName) * movementSpeed;
            float vertInput = Input.GetAxis(verticalInputName) * movementSpeed;

            Vector3 forwardMovement = transform.forward * vertInput;
            Vector3 rightMovement = transform.right * horizInput;

            if (Input.GetKey(KeyMapping.Sprint)) {
              movementSpeed = 9.5f;
            } else {
              movementSpeed = 6.0f;
            }

            // applies delta time so don't need to multiply above
            charController.SimpleMove(forwardMovement + rightMovement);

            JumpInput ();
        }

        private void JumpInput() {
            if (Input.GetKeyDown(KeyMapping.Jump) && !isJumping) {
                isJumping = true;
                StartCoroutine(JumpEvent());
            }
        }

        private IEnumerator JumpEvent() {
            charController.slopeLimit = 90.0f;
            float timeInAir = 0.0f;

            do {
                float jumpForce = jumpFalloff.Evaluate(timeInAir);
                // move as jumping is more complex than WASD
                charController.Move(Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime);
                timeInAir += Time.deltaTime;
                yield return null;
            } while(!charController.isGrounded &&
                charController.collisionFlags != CollisionFlags.Above);

            charController.slopeLimit = 45.0f;
            isJumping = false;
        }
    }
}
