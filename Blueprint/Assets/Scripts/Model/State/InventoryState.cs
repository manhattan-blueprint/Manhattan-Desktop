using System.Collections.Generic;
using System.Collections;
using Controller;
using Model.Action;
using UnityEngine;
using System.Linq;

namespace Model {
    public class InventoryState {
        public class HeldItem {
            public readonly int itemID;
            public readonly HexLocation location;

            public HeldItem(int itemID, HexLocation location) {
                this.itemID = itemID;
                this.location = location;
            }
        }
        
        public Dictionary<int, List<HexLocation>> inventoryContents;
        public int inventorySize = 0;
        public Optional<HeldItem> heldItem;

        public InventoryState() {
            inventoryContents = new Dictionary<int ,List<HexLocation>>(); 
        }
    }
}

public class HexLocation {
    public int hexID;
    public int quantity;

    public HexLocation(int hexID, int quantity) {
        this.hexID = hexID;
        this.quantity = quantity;
    }
}