using System;
using Model.Action;
using Model.State;
using UnityEngine;

namespace Model.Reducer {
    public class MachineReducer: Reducer<MachineState, MachineAction>, MachineVisitor {
        private MachineState state;
        public MachineState Reduce(MachineState current, MachineAction action) {
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

            if (machine.output.IsPresent()) {
                InventoryItem item = machine.output.Get();
                GameManager.Instance().inventoryStore.Dispatch(new AddItemToInventory(item.GetId(), item.GetQuantity(), item.GetName()));
            }

            state.grid.Remove(removeMachine.machineLocation);
        }

        public void visit(SetLeftInput setLeftInput) {
            if (!state.grid.ContainsKey(setLeftInput.machineLocation)) {
                throw new Exception("Machine does not exist at the given location");
            }

            Machine machine = state.grid[setLeftInput.machineLocation];
            if (machine.leftInput.IsPresent()) {
                // TODO: Do something with the current value? Add to inventory?
            }
            machine.leftInput = Optional<InventoryItem>.Of(setLeftInput.item);
        }

        public void visit(SetRightInput setRightInput) {
            if (!state.grid.ContainsKey(setRightInput.machineLocation)) {
                throw new Exception("Machine does not exist at the given location");
            }

            Machine machine = state.grid[setRightInput.machineLocation];
            if (machine.rightInput.IsPresent()) {
                // TODO: Do something with the current value? Add to inventory?
            }
            machine.rightInput = Optional<InventoryItem>.Of(setRightInput.item);
        }

        public void visit(SetFuel setFuel) {
            if (!state.grid.ContainsKey(setFuel.machineLocation)) {
                throw new Exception("Machine does not exist at the given location");
            }

            Machine machine = state.grid[setFuel.machineLocation];
            if (machine.fuel.IsPresent()) {
                // TODO: Do something with the current value? Add to inventory?
            }
            machine.fuel = Optional<InventoryItem>.Of(setFuel.item);
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
            if (machine.leftInput.IsPresent()) {
                machine.leftInput.Get().SetQuantity(machine.leftInput.Get().GetQuantity() - 1);
                if (machine.leftInput.Get().GetQuantity() == 0) machine.leftInput = Optional<InventoryItem>.Empty();
            }
            if (machine.rightInput.IsPresent()) {
                machine.rightInput.Get().SetQuantity(machine.rightInput.Get().GetQuantity() - 1);
                if (machine.rightInput.Get().GetQuantity() == 0) machine.rightInput = Optional<InventoryItem>.Empty();
            }
            if (machine.fuel.IsPresent()) {
                machine.fuel.Get().SetQuantity(machine.fuel.Get().GetQuantity() - 1);
                //if (machine.fuel.Get().GetQuantity() == 0) visit(new ClearFuel(consumeInputs.machineLocation));
            }
        }
    }
}