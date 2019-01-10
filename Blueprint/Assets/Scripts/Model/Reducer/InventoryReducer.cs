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

        public void visit(AddItemToInventory addItemToInventoryAction) {
            if (state.inventoryContents.ContainsKey(addItemToInventoryAction.item)) {
                state.inventoryContents[addItemToInventoryAction.item] += addItemToInventoryAction.count;
            } else {
                state.inventoryContents.Add(addItemToInventoryAction.item, addItemToInventoryAction.count);
            }
        }

        public void visit(RemoveItemFromInventory removeItemFromInventory) {
            if (state.inventoryContents.ContainsKey(removeItemFromInventory.item)) {
                var newValue = Math.Max(0,  state.inventoryContents[removeItemFromInventory.item] - removeItemFromInventory.count);
                state.inventoryContents[removeItemFromInventory.item] = newValue;
            }
        }
    }
}