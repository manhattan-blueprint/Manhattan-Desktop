using System.Collections.Generic;
using Model.Action;

namespace Model {
    public class InventoryState {
        //TODO: Replace with inventory type
        public Dictionary<int, int> inventoryContents;

        public InventoryState() {
            this.inventoryContents = new Dictionary<int, int>();
        }
    }
}