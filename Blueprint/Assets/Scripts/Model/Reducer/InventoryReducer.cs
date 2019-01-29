using System;
using Model.Action;

namespace Model.Reducer {
    public class InventoryReducer : Reducer<InventoryState, InventoryAction>, InventoryVisitor {
        private InventoryState state;
        
        public InventoryState Reduce(InventoryState current, InventoryAction action) {
            this.state = current;
            // Dispatch to visitor which will manipulate state
            action.Accept(this);
            return this.state;
        }

        // Returns a slot for
        public int GetSlot(int id, int size, InventoryItem[] items) {
            int firstNull = size + 1;
            for (int i = 0; i < size; i++) {
                if (items[i] == null) {
                    if (i < firstNull) {
                        firstNull = i;
                    }
                } else if (items[i].GetId() == id) {
                    return i;
                }
            }
            return firstNull;
        }

        public void visit(AddItemToInventory addItemToInventoryAction) {
            int id = addItemToInventoryAction.item;
            int quantity = addItemToInventoryAction.count;
            int size = state.inventoryContents.Length;

            InventoryItem item = new InventoryItem(id, quantity);
            InventoryItem slotItem = state.inventoryContents[GetSlot(id, size, state.inventoryContents)];

            // If there is a slot already containign
            if (slotItem != null) {
                state.inventoryContents[GetSlot(id, size, state.inventoryContents)].AddQuantity(quantity);
            } else {
                state.inventoryContents[GetSlot(id, size, state.inventoryContents)] = item;
            }
        }

        public void visit(RemoveItemFromInventory removeItemFromInventory) {
            int id = removeItemFromInventory.item;
            int quantity = removeItemFromInventory.count;
            int size = state.inventoryContents.Length;
            InventoryItem slotItem = state.inventoryContents[GetSlot(id, size, state.inventoryContents)];

            if (slotItem != null) {
                var newValue = Math.Max(0,  slotItem.GetQuantity() - quantity);
                slotItem.SetQuantity(newValue);
            } else {
                throw new System.Exception("This id does not exist in inventory.");
            }
        }
    }
}