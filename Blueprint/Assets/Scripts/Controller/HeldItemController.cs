using System.Collections;
using System.Collections.Generic;
using Controller;
using Model;
using Model.Redux;
using Model.State;
using UnityEngine;

public class HeldItemController : MonoBehaviour {
    public InventoryItem heldItem;
    private List<InventoryItem> placeableItems = new List<InventoryItem>();
    private InventoryController inventoryController;
    
    void Start() {
        inventoryController = GameObject.Find("InventoryUICanvas").GetComponent<InventoryController>();
    }

    // TODO: implement switching of held item (i.e. use of scroll wheel)
}
