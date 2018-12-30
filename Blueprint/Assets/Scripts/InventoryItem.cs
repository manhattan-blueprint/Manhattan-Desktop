using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class InventoryItem : MonoBehaviour {

    public int id;
    private int quantity;
    public Sprite icon;

    public InventoryItem(int id, int quantity) {
        this.id = id;
        this.quantity = quantity;
    }

    public int GetQuantity() {
        return quantity;
    }

    public Boolean Equals(Object obj) {
        Boolean result = false;
        if (obj.GetType() == typeof(InventoryItem)) {
            InventoryItem other = (InventoryItem) obj;
            result = this.id.Equals(other.id) && this.quantity == other.quantity;
        }
        return result;
    }
}
