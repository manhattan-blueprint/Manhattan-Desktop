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
                itemId = inventory.GetItems()[id].id;
                int quantity = inventory.GetItems()[id].GetQuantity();
                Debug.Log("Quantity before drop:" + quantity);
                inventory.GetItems()[id].SetQuantity(quantity - 1);
                
                if (inventory.GetItems()[id].GetQuantity() == 0) {
                    Destroy(child.gameObject);
                    empty = true;
                } else {
                    GameObject.Find("InventoryItemSlot " + id + "(Clone)").GetComponentInChildren<Text>().text = 
                    inventory.GetItems()[id].type + " (" + inventory.GetItems()[id].GetQuantity() + ")";
                    empty = false;
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
                    case (3):
                        InventoryItem invMac = inventory.GetItems()[id];
                        invMac.transform.position = new Vector3(position.x + facing.x , position.y + facing.y, position.z + facing.z);
                        invMac.transform.gameObject.SetActive(true);
                        if (init) {
                            Transform machinery = Instantiate(items.machinery, new Vector3(position.x + facing.x , position.y + facing.y, position.z + facing.z), Quaternion.identity);
                            machinery.name = "Machinery";
                        }
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
 
