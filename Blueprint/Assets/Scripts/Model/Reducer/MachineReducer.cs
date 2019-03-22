using System;
using Model.Action;
using Model.State;

namespace Model.Reducer {
    public class MachineReducer: Reducer<MachineState, MachineAction>, MachineVisitor {
        private MachineState state;
        public MachineState Reduce(MachineState current, MachineAction action) {
            this.state = current; 
            action.Accept(this);
            return this.state;
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
    }
}