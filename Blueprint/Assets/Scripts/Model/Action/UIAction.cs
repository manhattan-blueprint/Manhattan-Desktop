namespace Model.Action {
    public interface UIVisitor {
        void visit(CloseUI close);
        void visit(OpenLoginUI login);
        void visit(OpenPlayingUI playing);
        void visit(OpenInventoryUI inventory);
        void visit(OpenBlueprintUI blueprint);
        void visit(OpenMachineUI machine);
        void visit(OpenSettingsUI settings);
        void visit(Logout logout);
        void visit(Exit exit);
    }

    public abstract class UIAction : Action {
        public abstract void Accept(UIVisitor visitor);
    }

    // Close a UI screen, e.g. close inventory to continue playing
    public class CloseUI : UIAction {
        public override void Accept(UIVisitor visitor) {
            visitor.visit(this);
        }
    }

    public class OpenLoginUI : UIAction {
        public override void Accept(UIVisitor visitor) {
            visitor.visit(this);
        }
    }

    public class OpenPlayingUI : UIAction {
        public override void Accept(UIVisitor visitor) {
            visitor.visit(this);
        }
    }

    public class OpenInventoryUI : UIAction {
        public override void Accept(UIVisitor visitor) {
            visitor.visit(this);
        }
    }

    public class OpenBlueprintUI : UIAction {
        public override void Accept(UIVisitor visitor) {
            visitor.visit(this);
        }
    }

    public class OpenMachineUI : UIAction {
        public override void Accept(UIVisitor visitor) {
            visitor.visit(this);
        }
    }

    public class OpenSettingsUI : UIAction {
        public override void Accept(UIVisitor visitor) {
            visitor.visit(this);
        }
    }

    public class Logout : UIAction {
        public override void Accept(UIVisitor visitor) {
            visitor.visit(this);
        }
    }

    public class Exit : UIAction {
        public override void Accept(UIVisitor visitor) {
            visitor.visit(this);
        }
    }
}
