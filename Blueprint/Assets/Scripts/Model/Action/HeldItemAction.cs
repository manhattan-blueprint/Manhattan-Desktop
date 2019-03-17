namespace Model.Action {
    public interface HeldItemVisitor {
        void visit(RotateHeldItemForward rotateHeldItemForward);
        void visit(RotateHeldItemBackward rotateHeldItemBackward);
    }

    public abstract class HeldItemAction: Action {
        public abstract void Accept(HeldItemVisitor visitor);
    }
    
    public class RotateHeldItemForward: HeldItemAction {
        public override void Accept(HeldItemVisitor visitor) {
            visitor.visit(this);
        }
    }

    public class RotateHeldItemBackward : HeldItemAction {
        public override void Accept(HeldItemVisitor visitor) {
            visitor.visit(this);
        }
    }
}