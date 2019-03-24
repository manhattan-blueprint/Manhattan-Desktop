using System.Numerics;
using Vector2 = UnityEngine.Vector2;

namespace Model.Action {
    public interface MachineVisitor {
        void visit(AddMachine addMachine);
        void visit(SetLeftInput setLeftInput);
        void visit(SetRightInput setRightInput);
        void visit(SetFuel setFuel);
    }

    public abstract class MachineAction : Action {
        public abstract void Accept(MachineVisitor visitor);
    }

    public class AddMachine : MachineAction {
        public readonly Vector2 machineLocation;
        public readonly int itemID;

        public AddMachine(Vector2 machineLocation, int itemID) {
            this.machineLocation = machineLocation;
            this.itemID = itemID;
        }

        public override void Accept(MachineVisitor visitor) {
            visitor.visit(this);
        }
    }
    
    /* Call when item is dropped on left input */
    public class SetLeftInput : MachineAction {
        public readonly Vector2 machineLocation;
        public readonly InventoryItem item;

        public SetLeftInput(Vector2 machineLocation, InventoryItem item) {
            this.machineLocation = machineLocation;
            this.item = item;
        }

        public override void Accept(MachineVisitor visitor) {
            visitor.visit(this);
        }
    }
    
    /* Call when item is dropped on right input */
    public class SetRightInput : MachineAction {
        public readonly Vector2 machineLocation;
        public readonly InventoryItem item;

        public SetRightInput(Vector2 machineLocation, InventoryItem item) {
            this.machineLocation = machineLocation;
            this.item = item;
        }

        public override void Accept(MachineVisitor visitor) {
            visitor.visit(this);
        }
    }

    /* Call when item is dropped on fuel */
    public class SetFuel : MachineAction {
        public readonly Vector2 machineLocation;
        public readonly InventoryItem item;

        public SetFuel(Vector2 machineLocation, InventoryItem item) {
            this.machineLocation = machineLocation;
            this.item = item;
        }

        public override void Accept(MachineVisitor visitor) {
            visitor.visit(this);
        }
    }

}