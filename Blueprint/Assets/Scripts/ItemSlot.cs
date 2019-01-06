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
    private bool pickUp;
    private bool empty;

    void Start() {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        items = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ClickObject>(); 
    }


    public void DropItem() {
        foreach (Transform child in transform) {
            if (child.gameObject.CompareTag("Inventory")) {
                Transform cameraTransform = Camera.main.gameObject.transform;
                position = cameraTransform.position;
                facing = cameraTransform.forward * 2;
                itemId = inventory.GetItems()[id].GetId();
                int quantity = inventory.GetItems()[id].GetQuantity();
                inventory.GetItems()[id].SetQuantity(quantity - 1);
                
                if (inventory.GetItems()[id].GetQuantity() == 0) {
                    Destroy(child.gameObject);
                    empty = true;
                } else {
                    GameObject.Find("InventoryItemSlot " + id + "(Clone)").GetComponentInChildren<Text>().text = 
                    inventory.GetItems()[id].GetItemType() + " (" + inventory.GetItems()[id].GetQuantity() + ")";
                    empty = false;
                }
               
                
                switch (itemId) {
                    case (0):
                        Transform cube = Instantiate(items.cube, new Vector3(position.x + facing.x , position.y + facing.y, position.z + facing.z), Quaternion.identity);
                        break;
                    case (1):
                        Transform cubeLarge = Instantiate(items.cubeLarge, new Vector3(position.x + facing.x , position.y + facing.y, position.z + facing.z), Quaternion.identity);
                        break;
                    case (2):
                        Transform capsule = Instantiate(items.capsule, new Vector3(position.x + facing.x , position.y + facing.y, position.z + facing.z), Quaternion.identity);
                        break;
                    case (3):
                        Transform machinery = Instantiate(items.machinery, new Vector3(position.x + facing.x , position.y + facing.y, position.z + facing.z), Quaternion.identity);
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
 
