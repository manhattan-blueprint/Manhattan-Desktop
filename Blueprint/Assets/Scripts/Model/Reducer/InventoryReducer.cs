using Model.Action;
using Model.State;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Model.Reducer {
    public class InventoryReducer : Reducer<InventoryState, InventoryAction>, InventoryVisitor {
        private InventoryState state;
        
        public InventoryState Reduce(InventoryState current, InventoryAction action) {
            this.state = current;
            // Dispatch to visitor which will manipulate state
            action.Accept(this);
            return this.state;
        }

        private int getHighestItemID() {
            if (state.inventoryContents.Keys.Count > 0) {
                return state.inventoryContents.Keys.Max();
            } else {
                return 0;
            }
        } 

        private int getFirstEmptySlot() {
            List<int> occupiedSlots = new List<int>();

            for (int j = 0; j < getHighestItemID()+1; j++) {
                if (state.inventoryContents.ContainsKey(j)) {
                    List<HexLocation> tempList = state.inventoryContents[j];
                    foreach (HexLocation loc in tempList) {
                        occupiedSlots.Add(loc.hexID);
                    }
                }                
            }

            occupiedSlots.Sort();
            bool smallestSlot = false;
            int i = 0;
            while (!smallestSlot && i<state.inventorySize) {
                if (!occupiedSlots.Contains(i)) {
                    smallestSlot = true;
                    return i;
                } else {
                    i++;
                }
            }

            return 0;
        }

        public void visit(SetInventorySize setInventorySizeAction) {
            state.inventorySize = setInventorySizeAction.size;
        }

        public void visit(AddItemToInventory addItemToInventoryAction) {
            // If item exists, increment quantity on first stack
            // Else add item
            if (state.inventoryContents.ContainsKey(addItemToInventoryAction.item)) {
                state.inventoryContents[addItemToInventoryAction.item][0].quantity += addItemToInventoryAction.count;
            } else {
                HexLocation item = new HexLocation(getFirstEmptySlot(), addItemToInventoryAction.count);
                List<HexLocation> list = new List<HexLocation>();
                list.Add(item);
                
                state.inventoryContents.Add(addItemToInventoryAction.item, list);
            }
            
        }

        public void visit(RemoveItemFromInventory removeItemFromInventory) {
            int leftToRemove = removeItemFromInventory.count; 
            
            // If item is present in inventory
            if (state.inventoryContents.ContainsKey(removeItemFromInventory.item)) {
                // Iterate backwards through the stacks
                for (int i = state.inventoryContents[removeItemFromInventory.item].Count-1; i >= 0; i--) {
                    // If stack quantity > removal quantity
                    if (state.inventoryContents[removeItemFromInventory.item][i].quantity > removeItemFromInventory.count) {
                        state.inventoryContents[removeItemFromInventory.item][i].quantity -= removeItemFromInventory.count;
                        leftToRemove = 0;
                    } else {
                        leftToRemove = leftToRemove - state.inventoryContents[removeItemFromInventory.item][i].quantity;
                        state.inventoryContents[removeItemFromInventory.item].RemoveAt(i);
                    }
                    
                } 
            }
        }

        public void visit(RemoveItemFromStackInventory removeItemFromStackInventory) {
            for (int i = 0; i < state.inventoryContents[removeItemFromStackInventory.item].Count; i++) {
                if (state.inventoryContents[removeItemFromStackInventory.item][i].hexID ==
                    removeItemFromStackInventory.hexID &&
                    state.inventoryContents[removeItemFromStackInventory.item][i].quantity >=
                    removeItemFromStackInventory.count) {

                    state.inventoryContents[removeItemFromStackInventory.item][i].quantity -=
                        removeItemFromStackInventory.count;

                    if (state.inventoryContents[removeItemFromStackInventory.item][i].quantity == 0) {
                        state.inventoryContents[removeItemFromStackInventory.item].RemoveAt(i);
                        if (state.inventoryContents[removeItemFromStackInventory.item].Count == 0) {
                            state.inventoryContents.Remove(removeItemFromStackInventory.item);
                        }
                    }
                    return;
                }
            }
        }

        public void visit(SwapItemLocations swapItemLocations) {
            if (swapItemLocations.sourceItem.IsPresent()) {
                foreach (HexLocation hexLocation in state.inventoryContents[swapItemLocations.sourceItem.Get().GetId()]) {
                    if (hexLocation.hexID == swapItemLocations.sourceHexID) {
                        hexLocation.hexID = swapItemLocations.destinationHexID;
                    }
                }
            }
            
            if (swapItemLocations.destinationItem.IsPresent()) {
                foreach (HexLocation hexLocation in state.inventoryContents[swapItemLocations.destinationItem.Get().GetId()]) {
                    if (hexLocation.hexID == swapItemLocations.destinationHexID) {
                        hexLocation.hexID = swapItemLocations.sourceHexID;
                    }
                }
            }
        }

        public void visit(RemoveHeldItem removeHeldItem) {
            // Look for the object in that cell
            int index = GameManager.Instance().heldItemStore.GetState().indexOfHeldItem;
            
            foreach (KeyValuePair<int, List<HexLocation>> content in state.inventoryContents) {
                foreach (HexLocation hexLocation in content.Value) {
                    // Only remove and place if quantity > 0 and there is not a item placed at this location
                    if (hexLocation.hexID == index && hexLocation.quantity > 0 &&
                            !GameManager.Instance().mapStore.GetState().getObjects().ContainsKey(removeHeldItem.dropAt)) {
                        visit(new RemoveItemFromStackInventory(content.Key, 1, index));
                        GameManager.Instance().mapStore.Dispatch(new PlaceItem(removeHeldItem.dropAt, content.Key));
                        return;
                    } 
                }
            }
        }
    }
}