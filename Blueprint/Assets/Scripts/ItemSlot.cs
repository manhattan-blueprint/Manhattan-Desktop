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
            position = Camera.main.gameObject.transform.position;
            facing = Camera.main.gameObject.transform.forward * 2;
            itemId = inventory.GetItems()[id].id;
			if (child.gameObject.name == "InventoryButton(Clone)") {
                GameObject.Destroy(child.gameObject);
			}
			bool init = false;
            switch (itemId) {
                case (0):
					InventoryItem invCub = inventory.GetItems()[id];
					invCub.transform.position = new Vector3(position.x + facing.x , position.y + facing.y, position.z + facing.z);
                    invCub.transform.gameObject.SetActive(true);
					if (init) {
                        Transform cube = Instantiate(items.cube, new Vector3(position.x + facing.x , position.y + facing.y, position.z + facing.z), Quaternion.identity);
                        cube.name = "Cube";
					}
                    break;
                case (1):
					InventoryItem invCube = inventory.GetItems()[id];
					invCube.transform.position = new Vector3(position.x + facing.x , position.y + facing.y, position.z + facing.z);
                    invCube.transform.gameObject.SetActive(true);
					if (init) {
                        Transform cubeLarge = Instantiate(items.cubeLarge, new Vector3(position.x + facing.x , position.y + facing.y, position.z + facing.z), Quaternion.identity);
                        cubeLarge.name = "Cube Large";
					}	
                    break;
                case (2):
					InventoryItem invCap = inventory.GetItems()[id];
					invCap.transform.position = new Vector3(position.x + facing.x , position.y + facing.y, position.z + facing.z);
                    invCap.transform.gameObject.SetActive(true);
					if (init) {
                        Transform capsule = Instantiate(items.capsule, new Vector3(position.x + facing.x , position.y + facing.y, position.z + facing.z), Quaternion.identity);
                        capsule.name = "Capsule";
                    }
                    break;
                default:
                    Debug.Log("Item ID does not exist.");
                    break;
            }   
        }
        inventory.GetItems()[id] = null;
    }
}
 
