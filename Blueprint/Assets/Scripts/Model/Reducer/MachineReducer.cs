using System;
using System.Collections.Generic;
using System.Numerics;
using Model.Action;
using Model.State;
using UnityEngine;
using Utils;
using Vector2 = UnityEngine.Vector2;

namespace Model.Reducer {
    public class MachineReducer: Reducer<MachineState, MachineAction>, MachineVisitor {
        private MachineState state;
        private HashSet<Vector2> consideredConnected;
        
        public MachineState Reduce(MachineState current, MachineAction action) {
            this.consideredConnected = new HashSet<Vector2>();
            this.state = current; 
            action.Accept(this);
            return this.state;
        }

        public void visit(AddMachine addMachine) {
            if (state.grid.ContainsKey(addMachine.machineLocation)) {
                throw new Exception("Machine already exists at a given location");
            }
            Machine machine = new Machine(addMachine.itemID);
            state.grid.Add(addMachine.machineLocation, machine);
        }

        public void visit(RemoveMachine removeMachine) {
            if (!state.grid.ContainsKey(removeMachine.machineLocation)) {
                throw new Exception("Machine does not exist at the given location " + removeMachine.machineLocation);
            }

            Machine machine = state.grid[removeMachine.machineLocation];
            
            // Add items within the machine back to the inventory
            if (machine.leftInput.IsPresent()) {
                InventoryItem item = machine.leftInput.Get();
                GameManager.Instance().inventoryStore.Dispatch(new AddItemToInventory(item.GetId(), item.GetQuantity(), item.GetName()));
            }

            if (machine.rightInput.IsPresent()) {
                InventoryItem item = machine.rightInput.Get();
                GameManager.Instance().inventoryStore.Dispatch(new AddItemToInventory(item.GetId(), item.GetQuantity(), item.GetName()));
            }

            if (machine.fuel.IsPresent()) {
                InventoryItem item = machine.fuel.Get();
                GameManager.Instance().inventoryStore.Dispatch(new AddItemToInventory(item.GetId(), item.GetQuantity(), item.GetName()));
            }

            state.grid.Remove(removeMachine.machineLocation);
            visit(new UpdateConnected());
        }

        public void visit(SetLeftInput setLeftInput) {
            if (!state.grid.ContainsKey(setLeftInput.machineLocation)) {
                throw new Exception("Machine does not exist at the given location");
            }

            Machine machine = state.grid[setLeftInput.machineLocation];
            machine.leftInput = Optional<InventoryItem>.Of(setLeftInput.item);
        }

        public void visit(SetRightInput setRightInput) {
            if (!state.grid.ContainsKey(setRightInput.machineLocation)) {
                throw new Exception("Machine does not exist at the given location");
            }

            Machine machine = state.grid[setRightInput.machineLocation];
            machine.rightInput = Optional<InventoryItem>.Of(setRightInput.item);
        }
        
        public void visit(SetInputs setInputs) {
            if (!state.grid.ContainsKey(setInputs.machineLocation)) {
                throw new Exception("Machine does not exist at the given location");
            }

            Machine machine = state.grid[setInputs.machineLocation];
            machine.leftInput = setInputs.left;
            machine.rightInput = setInputs.right;
        }
        
        public void visit(SetAll setAll) {
            if (!state.grid.ContainsKey(setAll.machineLocation)) {
                throw new Exception("Machine does not exist at the given location");
            }

            Machine machine = state.grid[setAll.machineLocation];
            machine.leftInput =  setAll.left;
            machine.rightInput = setAll.right;
            machine.fuel = setAll.fuel;
        }

        public void visit(SetFuel setFuel) {
            if (!state.grid.ContainsKey(setFuel.machineLocation)) {
                throw new Exception("Machine does not exist at the given location");
            }

            Machine machine = state.grid[setFuel.machineLocation];
            machine.fuel = setFuel.item;
        }

        public void visit(ClearLeftInput clearLeftInput) {
            if (!state.grid.ContainsKey(clearLeftInput.machineLocation)) {
                throw new Exception("Machine does not exist at the given location");
            }

            state.grid[clearLeftInput.machineLocation].leftInput = Optional<InventoryItem>.Empty();
        }
        
        public void visit(ClearRightInput clearRightInput) {
            if (!state.grid.ContainsKey(clearRightInput.machineLocation)) {
                throw new Exception("Machine does not exist at the given location");
            }

            state.grid[clearRightInput.machineLocation].rightInput = Optional<InventoryItem>.Empty();
        }
        
        public void visit(ClearFuel clearFuel) {
            if (!state.grid.ContainsKey(clearFuel.machineLocation)) {
                throw new Exception("Machine does not exist at the given location");
            }

            state.grid[clearFuel.machineLocation].fuel = Optional<InventoryItem>.Empty();
        }

        public void visit(ConsumeInputs consumeInputs) {
            if (!state.grid.ContainsKey(consumeInputs.machineLocation)) {
                throw new Exception("Machine does not exist at the given location");
            }

            Machine machine = state.grid[consumeInputs.machineLocation];
            Optional<MachineProduct> product = GameManager.Instance().sm.GetRecipe(machine.GetInputs(), machine.id);
            if (!product.IsPresent()) {
                throw new Exception("Consuming inputs for an invalid recipe!");
            }

            bool shouldRemoveLeft = machine.leftInput.IsPresent() &&
                                    product.Get().item.recipe.Find(recipe => recipe.item_id == machine.leftInput.Get().GetId()) != null;
            bool shouldRemoveRight = machine.rightInput.IsPresent() &&
                                    product.Get().item.recipe.Find(recipe => recipe.item_id == machine.rightInput.Get().GetId()) != null;
           
            int maxQuantity = product.Get().maxQuantity;
            if (!GameManager.Instance().sm.GameObjs.items.Find(x => x.item_id == machine.id).isPoweredByElectricity()) {
                maxQuantity = Math.Min(maxQuantity, machine.fuel.Get().GetQuantity());
            }
            
            if (shouldRemoveLeft) {
                machine.leftInput.Get().SetQuantity(machine.leftInput.Get().GetQuantity() - maxQuantity);
                if (machine.leftInput.Get().GetQuantity() == 0) machine.leftInput = Optional<InventoryItem>.Empty();
            }
            if (shouldRemoveRight) {
                machine.rightInput.Get().SetQuantity(machine.rightInput.Get().GetQuantity() - maxQuantity);
                if (machine.rightInput.Get().GetQuantity() == 0) machine.rightInput = Optional<InventoryItem>.Empty();
            }
            if (machine.fuel.IsPresent()) {
                machine.fuel.Get().SetQuantity(machine.fuel.Get().GetQuantity() - maxQuantity);
                if (machine.fuel.Get().GetQuantity() == 0) visit(new ClearFuel(consumeInputs.machineLocation));
            }
        }

        public void visit(UpdateConnected updateConnected) {
            foreach (KeyValuePair<Vector2, Machine> keyValuePair in state.grid) {
                // Clear considered and path for every machine
                this.consideredConnected = new HashSet<Vector2>();
                SchemaItem item = GameManager.Instance().sm.GameObjs.items.Find(x => x.item_id == keyValuePair.Value.id);
                // If contains electricity
                if (item.isPoweredByElectricity()) {
                    state.grid[keyValuePair.Key].SetHasElectricity(isConnected(keyValuePair.Key));
                }
            }
        }


        private bool isConnected(Vector2 location) {
            consideredConnected.Add(location);
            bool connected = false;
            foreach (Vector2 neighbour in location.HexNeighbours()) {
                // If we've already done it, don't bother doing again (avoids cycles)
                if (consideredConnected.Contains(neighbour)) continue;
                consideredConnected.Add(neighbour);
                
                if (!GameManager.Instance().mapStore.GetState().GetObjects().ContainsKey(neighbour)) continue;
                int neighbourID = GameManager.Instance().mapStore.GetState().GetObjects()[neighbour].GetID();

                // If is a solar panel
                if (neighbourID == 25) {
                    return true;
                } 
                
                // If is a wire 
                if (neighbourID == 22) {
                    connected = connected || isConnected(neighbour);
                }
            }

            return connected;
        }
    }
}