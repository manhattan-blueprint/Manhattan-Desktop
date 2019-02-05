namespace Model.Action {
    public interface UIVisitor {
        void visit(OpenUI openUI);
        void visit(CloseUI closeUI);
    }

    public abstract class UIAction : Action {
        public abstract void Accept(UIVisitor visitor);
    }

    /* Add an item to the inventory for the user */
    public class OpenUI : UIAction {
        public readonly UIState.OpenUI next;
        
        public OpenUI(UIState.OpenUI next) {
            this.next = next;
        }

        public override void Accept(UIVisitor visitor) {
            visitor.visit(this);
        }
    }

    /* Remove an item to the inventory for the user */
    public class CloseUI : UIAction {
        public override void Accept(UIVisitor visitor) {
            visitor.visit(this);
        }
    }

}