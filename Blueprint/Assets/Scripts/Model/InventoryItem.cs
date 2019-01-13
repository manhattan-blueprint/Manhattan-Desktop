namespace Model {
    public class InventoryItem {
        private int id;
        private string type;
        public int quantity;

        public InventoryItem(int id, string type, int quantity) {
            this.id = id;
            this.type = type;
            this.quantity = quantity;
        }

        public int GetId() {
            return id;
        }

        public string GetItemType() {
            return type;
        }
    }
}