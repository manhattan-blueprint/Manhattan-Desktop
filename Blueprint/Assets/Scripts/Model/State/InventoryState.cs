using System.Collections.Generic;
using Model.Action;

namespace Model {
    public class InventoryState {
        //TODO: Replace with inventory type
        public InventoryItem[] inventoryContents;

        public InventoryState() {
            this.inventoryContents = new InventoryItem[16];
        }
    }
}