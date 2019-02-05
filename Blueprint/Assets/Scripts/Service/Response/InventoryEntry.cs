using System;

namespace Service.Response {
    [Serializable]
    public class InventoryEntry {
        // TODO: Remove the underscore and tell the serializer how to decode
        public int item_id;
        public int quantity;

        public InventoryEntry(int itemId, int quantity) {
            this.item_id = itemId;
            this.quantity = quantity;
        }
        
        public string ToString() {
            return this.item_id + " " + this.quantity;
        }
    }
}
