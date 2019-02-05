using System.Collections.Generic;
using Model.Action;

namespace Model {
    public class InventoryState {
        public InventoryItem[] inventoryContents;

        public InventoryState() {
            inventoryContents = new InventoryItem[16];
        }
    }
}