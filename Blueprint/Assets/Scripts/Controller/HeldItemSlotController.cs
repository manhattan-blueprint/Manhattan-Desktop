using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;
using UnityEngine.UI;

public class HeldItemSlotController : MonoBehaviour {
    private AssetManager assetManager;
    private InventoryItem itemInSlot;
    // TODO: move nullItem to a global location
    private InventoryItem nullItem = new InventoryItem("", 0, 0);
    
    void Start() {
        assetManager = AssetManager.Instance();
    }

    void Update() {
        
    }

    public void setSlotItem(InventoryItem item) {
        itemInSlot = item;
        updateFields();
    }

    private void updateFields() {
        Image image = transform.GetComponentsInChildren<Image>()[1];
        Text text = transform.GetComponentInChildren<Text>();
        
        if (itemInSlot.GetId() != nullItem.GetId()) {
            image.sprite = assetManager.GetItemSprite(itemInSlot.GetId());
            text.text = itemInSlot.GetQuantity().ToString();
                
            image.enabled = true;
            text.enabled = true;
        } else {
            image.enabled = false;
            text.enabled = false;
        }
    }
}
