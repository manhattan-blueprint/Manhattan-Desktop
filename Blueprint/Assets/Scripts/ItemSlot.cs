using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour {

	public int id;
	public Inventory inventory;

	void Start() {
		inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
	}

	public void DropItem() {
		foreach (Transform child in transform) {
			Debug.Log("Dropping item:" + child.gameObject.name);
			GameObject.Destroy(child.gameObject);
			inventory.GetItems()[id] = null;
		}
	}
}
 