using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementController : MonoBehaviour {


	[SerializeField] private GameObject placeableObjectPrefab;
	[SerializeField] private KeyCode newObjectHotkey = KeyCode.Q;

	private GameObject currentPlaceableObject;

	private float mouseWheelRotation;
	
	// Update is called once per frame
	void Update () {
		HandleNewObjectHotKey();

		if (currentPlaceableObject != null) {
			MoveCurrentObjectToMouse();

			ReleaseIfClicked();
		}
	}

	private void HandleNewObjectHotKey() {
		if (Input.GetKeyDown(newObjectHotkey)) {
			if (currentPlaceableObject != null) {
				Destroy(currentPlaceableObject);
			} else {
				currentPlaceableObject = Instantiate(placeableObjectPrefab);
			}
		}
	}


	private void MoveCurrentObjectToMouse() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		RaycastHit hitInfo;

		if(Physics.Raycast(ray, out hitInfo)) {
			currentPlaceableObject.transform.position = Camera.main.gameObject.transform.position + new Vector3(0,0,3);
			currentPlaceableObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
		}
	}

	private void ReleaseIfClicked() {
		if (Input.GetMouseButtonDown(0)) {
			currentPlaceableObject = null;
		}
	}

}
