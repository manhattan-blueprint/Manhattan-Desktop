namespace Model.Action {
    public interface InventoryVisitor {
        void visit(AddItemToInventory addItemToInventoryAction);
        void visit(RemoveItemFromInventory anotherInventoryAction);
        void visit(RemoveItemFromStackInventory anotherInventoryAction);
    }

    public abstract class InventoryAction : Action {
        public abstract void Accept(InventoryVisitor visitor);
    }

    /* Add an item to the inventory for the user */
    public class AddItemToInventory : InventoryAction {
        public readonly int item;
        public readonly int count;
        public readonly string name;
        
        public AddItemToInventory(int item, int count, string name) {
            this.item = item;
            this.count = count;
            this.name = name;
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
    
    /* Remove an item in a specific stack to the inventory for the user */
    public class RemoveItemFromStackInventory : InventoryAction {
        public readonly int item;
        public readonly int count;
        public readonly int hexId;
        
        public RemoveItemFromStackInventory(int item, int count, int hexId) {
            this.item = item;
            this.count = count;
            this.hexId = hexId;
        }

        public override void Accept(InventoryVisitor visitor) {
            visitor.visit(this);
        }
    }

}