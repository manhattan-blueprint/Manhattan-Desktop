using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuShow : MonoBehaviour {

	private Canvas CanvasObject;

	void Start() {
	    CanvasObject = GetComponent<Canvas> ();
	    CanvasObject.enabled = false;
    }

	void Update() {
		if (Input.GetKeyDown(KeyCode.E)) {
			if (!CanvasObject.enabled) {
				PauseGame();
			} else {
				ContinueGame();
			}
		}
    }

	private void PauseGame() {
		Time.timeScale = 0;
		CanvasObject.enabled = true;
		Cursor.lockState = CursorLockMode.None;
	}

	private void ContinueGame() {
		Time.timeScale = 1;
		CanvasObject.enabled = false;
		Cursor.lockState = CursorLockMode.Locked;
	}
}
