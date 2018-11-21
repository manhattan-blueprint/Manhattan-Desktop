using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {

	Transform hand;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void onMouseDown() {
		this.transform.position = hand.position;
	}

	void onMouseUp() {
		this.transform.parent = null;
	}
}
