using UnityEngine;

namespace Model.Action {
    public interface InventoryVisitor {
        void visit(AddItemToInventory addItemToInventoryAction);
        void visit(RemoveItemFromInventory anotherInventoryAction);
        void visit(RemoveItemFromStackInventory anotherInventoryAction);
        void visit(SwapItemLocations anotherInventoryAction);
        void visit(RemoveHeldItem anotherInventoryAction);
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
    
    // Swaps the locations (hexID) of two items in the inventory
    public class SwapItemLocations : InventoryAction {
        public readonly int sourceHexID;
        public readonly int destinationHexID;
        public readonly Optional<InventoryItem> sourceItem;
        public readonly Optional<InventoryItem> destinationItem;

        public SwapItemLocations(int sourceHexID, int destinationHexID, Optional<InventoryItem> sourceItem, Optional<InventoryItem> destinationItem) {
            this.sourceHexID = sourceHexID;
            this.destinationHexID = destinationHexID;
            this.sourceItem = sourceItem;
            this.destinationItem = destinationItem;
        }

        public override void Accept(InventoryVisitor visitor) {
            visitor.visit(this);
        }
    }
    
    public class RemoveHeldItem : InventoryAction {
        public readonly Vector2 dropAt;

        public RemoveHeldItem(Vector2 dropAt) {
            this.dropAt = dropAt;
        }
        
        public override void Accept(InventoryVisitor visitor) {
            visitor.visit(this);
        }
    }
}