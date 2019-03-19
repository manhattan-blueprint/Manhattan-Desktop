using System.Collections.Generic;
using System.Collections;
using Controller;
using Model.Action;
using UnityEngine;
using System.Linq;

namespace Model.State {
    public class InventoryState {
        public Dictionary<int, List<HexLocation>> inventoryContents;
        public int inventorySize = 0;

        public InventoryState() {
            inventoryContents = new Dictionary<int, List<HexLocation>>(); 
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