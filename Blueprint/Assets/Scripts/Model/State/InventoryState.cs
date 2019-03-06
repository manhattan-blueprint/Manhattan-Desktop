using System.Collections.Generic;
using System.Collections;
using Model.Action;

namespace Model {
    public class InventoryState {
        public Dictionary<int, List<HexLocation>> inventoryContents;
        //TODO: fix to be dynamic
        public int inventorySize = 18;

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