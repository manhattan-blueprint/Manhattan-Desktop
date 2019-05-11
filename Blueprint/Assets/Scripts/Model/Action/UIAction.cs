using UnityEditor;
using UnityEngine;

namespace Model.Action {
    public interface UIVisitor {
        void visit(CloseUI close);
        void visit(OpenLoginUI login);
        void visit(OpenPlayingUI playing);
        void visit(OpenInventoryUI inventory);
        void visit(OpenBlueprintUI blueprint);
        void visit(OpenBlueprintTemplateUI blueprintTemplate);
        void visit(OpenBindingsUI bindings);
        void visit(OpenGateMouseUI mouse);
        void visit(OpenBeaconMouseUI mouse);
        void visit(OpenGateUI gate);
        void visit(OpenIntroUI intro);
        void visit(OpenMachineUI machine);
        void visit(OpenGoalUI machine);
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

    public class OpenBlueprintTemplateUI : UIAction {
        public readonly int id;

        public OpenBlueprintTemplateUI(int id) {
            this.id = id;
        }
        public override void Accept(UIVisitor visitor) {
            visitor.visit(this);
        }
    }

    public class OpenBindingsUI : UIAction {
        public override void Accept(UIVisitor visitor) {
            visitor.visit(this);
        }
    }

    public class OpenGateMouseUI : UIAction {
        public override void Accept(UIVisitor visitor) {
            visitor.visit(this);
        }
    }

    public class OpenBeaconMouseUI : UIAction
    {
        public override void Accept(UIVisitor visitor)
        {
            visitor.visit(this);
        }
    }

    public class OpenGateUI : UIAction {
        public override void Accept(UIVisitor visitor) {
            visitor.visit(this);
        }
    }

    public class OpenIntroUI : UIAction {
        public override void Accept(UIVisitor visitor) {
            visitor.visit(this);
        }
    }

    public class OpenMachineUI : UIAction {
        public readonly Vector2 machinePosition;

        public OpenMachineUI(Vector2 machinePosition) {
            this.machinePosition = machinePosition;
        }

        public override void Accept(UIVisitor visitor) {
            visitor.visit(this);
        }
    }

    public class OpenGoalUI : UIAction {
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
