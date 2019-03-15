using System.Collections;
using System.Collections.Generic;
using Controller;
using Model;
using Model.Redux;
using Model.State;
using UnityEngine;

public class HeldItemController : MonoBehaviour, Subscriber<GameState> {
    public InventoryItem heldItem;
    private List<InventoryItem> placeableItems = new List<InventoryItem>();
    private InventoryController inventoryController;
    private HeldItemSlotController[] heldItemSlots;
    
    void Start() {
        inventoryController = GameObject.Find("InventoryUICanvas").GetComponent<InventoryController>();
        heldItemSlots = gameObject.GetComponentsInChildren<HeldItemSlotController>();
    }

    public void StateDidUpdate(GameState gameState) {
        List<InventoryItem> temp = new List<InventoryItem>();
        
        foreach (KeyValuePair<int, List<HexLocation>> element in gameState.inventoryState.inventoryContents) {
            foreach(HexLocation loc in element.Value) {
                // If can be placed 
                if (inventoryController.GetItemType(element.Key) == 2) {
                    InventoryItem item = new InventoryItem(inventoryController.GetItemName(element.Key), element.Key, loc.quantity);
                    temp.Add(item);
                } 
            } 
        }

        placeableItems = temp;
    }
}
