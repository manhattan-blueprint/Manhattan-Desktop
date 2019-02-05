using System;
using Model.Action;
using UnityEngine;
using Utils;

namespace Model.Reducer {
    public class InventoryReducer : Reducer<InventoryState, InventoryAction>, InventoryVisitor {
        private InventoryState state;
        
        public InventoryState Reduce(InventoryState current, InventoryAction action) {
            this.state = current;
            // Dispatch to visitor which will manipulate state
            action.Accept(this);
            return this.state;
        }

        // Returns a slot for a resource id
        private int getSlot(int id) {
            for (int i = 0; i < state.inventoryContents.Length; i++) {
                if (state.inventoryContents[i] == null || state.inventoryContents[i].GetId() == id) {
                    return i;
                }
            }
            return -1;
        }

        public void visit(AddItemToInventory addItemToInventoryAction) {
            int slot = getSlot(addItemToInventoryAction.item);
            
            // Update if exists or add new
            if (state.inventoryContents[slot] != null) {
                state.inventoryContents[slot].AddQuantity(addItemToInventoryAction.count);
            } else {
                state.inventoryContents[slot] = new InventoryItem(addItemToInventoryAction.name, 
                                                    addItemToInventoryAction.item, addItemToInventoryAction.count);
            }
        }

        public void visit(RemoveItemFromInventory removeItemFromInventory) {
            int id = removeItemFromInventory.item;
            int quantity = removeItemFromInventory.count;
            InventoryItem slotItem = state.inventoryContents[getSlot(id)];

            if (slotItem != null) {
                var newValue = Math.Max(0,  slotItem.GetQuantity() - quantity);
                slotItem.SetQuantity(newValue);
            } else {
                throw new System.Exception("This id does not exist in inventory.");
            }
        }
    }
}