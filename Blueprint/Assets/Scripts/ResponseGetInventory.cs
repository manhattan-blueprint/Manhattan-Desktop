using System;
using System.Collections.Generic;

[Serializable]
public class ResponseGetInventory {
    public List<InventoryEntry> items;

    public ResponseGetInventory(List<InventoryEntry> items) {
        this.items = items;
    }
}

