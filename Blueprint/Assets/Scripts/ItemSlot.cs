using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Behaviour for the UI item slots
public class ItemSlot : MonoBehaviour {

	public int id;
	public Inventory inventory;
	public ClickObject items;
	private Vector3 position;
	private Vector3 facing;
	private int itemId;

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
            itemId = inventory.GetItems()[id].id;
			GameObject.Destroy(child.gameObject);
			inventory.GetItems()[id] = null;
			Debug.Log("Item ID is: " + itemId);
			switch (itemId)
			{
				case (0):
					Transform cube = Instantiate(items.cube, new Vector3(position.x + facing.x , position.y + facing.y, position.z + facing.z), Quaternion.identity);
					cube.name = "Cube";
					break;
				case (1):
					Transform cubeLarge = Instantiate(items.cubeLarge, new Vector3(position.x + facing.x , position.y + facing.y, position.z + facing.z), Quaternion.identity);
					cubeLarge.name = "Cube Large";
					break;
				case (2):
					Transform capsule = Instantiate(items.capsule, new Vector3(position.x + facing.x , position.y + facing.y, position.z + facing.z), Quaternion.identity);
					capsule.name = "Capsule";
					break;
				default:
					Debug.Log("Damn you Stormcloaks. Skyrim was fine until you came along. Empire was nice and lazy. If they hadn't been looking for you, I could've stolen that horse and been half way to Hammerfell. You there. You and me -- we should be here. It's these Stormcloaks the Empire wants.!");

                    break;
			}	
		}
	}
}
 