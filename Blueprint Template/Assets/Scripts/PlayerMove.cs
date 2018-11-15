using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {
	[SerializeField] private string horizontalInputName;
	[SerializeField] private string verticalInputName;
	[SerializeField] private float movementSpeed;

	private CharacterController charController;

	[SerializeField] private AnimationCurve jumpFalloff;
	[SerializeField] private float jumpMultiplier;
	[SerializeField] private KeyCode jumpKey;
	private bool isJumping;

	[SerializeField] private KeyCode placeKey;

	// public GameObject hexMap;
	// GenerateHex genHex;

	void start() {
		// genHex = hexMap.AddComponent<GenerateHex> ();
	}

	private void Awake() {
		charController = GetComponent<CharacterController> ();
	}

	private void Update() {
		PlayerMovement ();
	}

	private void PlayerMovement() {
		float horizInput = Input.GetAxis (horizontalInputName) * movementSpeed;
		float vertInput = Input.GetAxis (verticalInputName) * movementSpeed;

		Vector3 forwardMovement = transform.forward * vertInput;
		Vector3 rightMovement = transform.right * horizInput;

		// applies delta time so don't need to multiply above
		charController.SimpleMove (forwardMovement + rightMovement);

		JumpInput ();
		placeInput();
	}

	private void JumpInput() {
		if (Input.GetKeyDown (jumpKey)) {
			isJumping = true;
			Debug.Log("jump key pressed");
			StartCoroutine (JumpEvent ());
		}
	}

	private IEnumerator JumpEvent()
	{
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

	private void placeInput() {
		if (Input.GetKeyDown (placeKey)) {
			StartCoroutine (placeEvent ());
		}
	}

	// When player presses the "place object" key
	private IEnumerator placeEvent() {
		// Debug.Log(charController.transform.position);
		Vector3 cPos = charController.transform.position;
		Debug.Log("Placing object near" + cPos);
		// genHex.PlayerPlace(cPos[0], cPos[1], cPos[2], Quaternion.Euler(0, 0, 0));
		yield return null;
	}
}
