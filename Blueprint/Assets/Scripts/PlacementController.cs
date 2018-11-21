using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementController : MonoBehaviour {


	public GameObject item;
	public GameObject tempParent;
	public Transform guide;
	private bool moved;


	// Use this for initialisation
	void Start() {
		item.GetComponent<Rigidbody>().useGravity = true;
	}

	// Update is called once per frame
	void Update () {

	}

	void OnMouseDown() {
		float dist = Vector3.Distance(item.transform.position, guide.transform.position);
		Debug.Log(dist);
		if (dist < 2) {
			moved = true;
			item.GetComponent<Rigidbody>().useGravity = false;
			item.GetComponent<Rigidbody>().isKinematic = true;
			item.transform.position = guide.transform.position;
			item.transform.rotation = guide.transform.rotation;
			item.transform.parent = tempParent.transform;
		}
	}

	void OnMouseUp() {
		if (moved) {
			item.GetComponent<Rigidbody>().useGravity = true;
			item.GetComponent<Rigidbody>().isKinematic = false;
			item.transform.parent = null;
			item.transform.position = guide.transform.position;
			moved = !moved;
		}
	}

}
