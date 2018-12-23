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
            Debug.Log(inventory.GetItems()[id]);
            GameObject.Destroy(child.gameObject);
            inventory.GetItems()[id] = null;
            switch (itemId) {
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
                    Debug.Log("The man who passes the sentence should swing the sword.");
                    break;
            }   
        }
    }
}
 
