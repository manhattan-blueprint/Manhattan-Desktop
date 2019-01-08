using System;
using System.Collections.Generic;

[Serializable]
public class ResponseGetInventory {
    public List<InventoryEntry> items;

    public ResponseGetInventory(List<InventoryEntry> items) {
        this.items = items;
    }
}

[Serializable]
public class InventoryEntry {
    public int item_id;
    public int quantity;

    public InventoryEntry(int itemId, int quantity) {
        item_id = itemId;
        this.quantity = quantity;
    }
}

