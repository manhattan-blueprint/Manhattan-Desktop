using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Attached to each slot in the inventory grid */
namespace Controller {
    public class InventorySlotController : MonoBehaviour {
        public int id;
        private InventoryController inventory;
        private MouseController items;
        private Vector3 position;
        private Vector3 facing;
        private int itemId;
        private bool pickUp;
        private bool empty;
        private Transform cameraTransform;

        void Start() {
            inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryController>();
            items = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MouseController>(); 
            cameraTransform = Camera.main.gameObject.transform;
        }

        // Loops through the child elements of the item slot to find the associated text/sprite and delete it from the menu 
        public void DropItem() {
            foreach (Transform child in transform) {
                if (child.gameObject.CompareTag("Inventory")) {
                    // Set up drop position
                    position = cameraTransform.position;
                    facing = cameraTransform.forward * 2;
                    
                    // Get ID and quantity of item to be dropped
                    itemId = inventory.GetItems()[id].GetId();
                    inventory.GetItems()[id].quantity -= 1;
                    
                    if (inventory.GetItems()[id].quantity == 0) {
                        Destroy(child.gameObject);
                        empty = true;
                    } else {
                        GameObject.Find("InventoryItemSlot " + id + "(Clone)").GetComponentInChildren<Text>().text = 
                        inventory.GetItems()[id].GetItemType() + " (" + inventory.GetItems()[id].quantity + ")";
                        empty = false;
                    }
                
                    switch (itemId) {
                        case (0):
                            //Transform cube = Instantiate(items.cube, new Vector3(position.x + facing.x , position.y + facing.y, position.z + facing.z), Quaternion.identity);
                            break;
                        case (1):
                            //Transform cubeLarge = Instantiate(items.cubeLarge, new Vector3(position.x + facing.x , position.y + facing.y, position.z + facing.z), Quaternion.identity);
                            break;
                        case (2):
                            //Transform capsule = Instantiate(items.capsule, new Vector3(position.x + facing.x , position.y + facing.y, position.z + facing.z), Quaternion.identity);
                            break;
                        case (3):
                            //Transform machinery = Instantiate(items.machinery, new Vector3(position.x + facing.x , position.y + facing.y, position.z + facing.z), Quaternion.identity);
                            break;
                        default:
                            Debug.Log("Item ID does not exist.");
                            break;
                    }
                    if (empty) {
                        inventory.GetItems()[id] = null;
                    }
                }
            }
        }
    }
}    
