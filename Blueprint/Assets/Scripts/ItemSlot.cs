using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour {

	public int id;
	public Inventory inventory;
	public ClickObject items;
	private Vector3 position;
	private Vector3 facing;
	private string itemName;

	void Start() {
		inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
		items = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ClickObject>(); 
	}

	public void DropItem() {
		foreach (Transform child in transform) {
			Debug.Log("Dropping item:" + inventory.GetItems()[id].name);
			position = Camera.main.gameObject.transform.position;
			facing = Camera.main.gameObject.transform.forward * 2;
			Debug.Log("Forward vector: " + position);
			itemName = inventory.GetItems()[id].name.Replace("(Clone)","").Trim();
			GameObject.Destroy(child.gameObject);
			inventory.GetItems()[id] = null;
			Debug.Log("Item name is: " + itemName);
			switch (itemName)
			{
				case ("Cube"):
					Transform cube = Instantiate(items.cube, new Vector3(position.x + facing.x , position.y + facing.y, position.z + facing.z), Quaternion.identity);
					cube.name = "Cube";
					break;
				case ("Cube Large"):
					Transform cubeLarge = Instantiate(items.cubeLarge, new Vector3(position.x + facing.x , position.y + facing.y, position.z + facing.z), Quaternion.identity);
					cubeLarge.name = "Cube Large";
					break;
				case ("Capsule"):
					Transform capsule = Instantiate(items.capsule, new Vector3(position.x + facing.x , position.y + facing.y, position.z + facing.z), Quaternion.identity);
					capsule.name = "Capsule";
					break;
				default:
					Debug.Log("You're a wizard Harry!");
					break;
			}	
		}
	}
}
 