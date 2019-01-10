namespace Model.Action {
    public interface InventoryVisitor {
        void visit(AddItemToInventory addItemToInventoryAction);
        void visit(RemoveItemFromInventory anotherInventoryAction);
    }

    public abstract class InventoryAction : Action {
        public abstract void Accept(InventoryVisitor visitor);
    }

    /* Add an item to the inventory for the user */
    public class AddItemToInventory : InventoryAction {
        public readonly int item;
        public readonly int count;
        
        public AddItemToInventory(int item, int count) {
            this.item = item;
            this.count = count;
        }

        public override void Accept(InventoryVisitor visitor) {
            visitor.visit(this);
        }
    }

    /* Remove an item to the inventory for the user */
    public class RemoveItemFromInventory : InventoryAction {
        public readonly int item;
        public readonly int count;
        
        public RemoveItemFromInventory(int item, int count) {
            this.item = item;
            this.count = count;
        }

        public override void Accept(InventoryVisitor visitor) {
            visitor.visit(this);
        }
    }

}