using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour {

	private Inventory inventory;
	
	public void Start() {
		inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
	}

	public void DropItem() {
		Debug.Log("Hello");
		foreach (Transform child in transform) {
			GameObject.Destroy(child.gameObject);

		}
	}
}
 