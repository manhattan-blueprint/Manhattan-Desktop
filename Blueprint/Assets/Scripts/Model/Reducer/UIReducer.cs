using Model.Action;

namespace Model.Reducer {
    public class UIReducer : Reducer<UIState, UIAction>, UIVisitor {
        private UIState state;

        public UIState Reduce(UIState current, UIAction action) {
            this.state = current;
            // Dispatch to visitor which will manipulate state
            action.Accept(this);
            return this.state;
        }

        public void visit(CloseUI closeUI) {
            var current = state.Selected;
            switch (current) {
                case UIState.OpenUI.Inventory:
                    state.Selected = UIState.OpenUI.Playing;
                    break;
                case UIState.OpenUI.Blueprint:
                    state.Selected = UIState.OpenUI.Playing;
                    break;
                case UIState.OpenUI.Machine:
                    state.Selected = UIState.OpenUI.Playing;
                    break;
                case UIState.OpenUI.PlaySettings:
                    state.Selected = UIState.OpenUI.Playing;
                    break;
                case UIState.OpenUI.InvSettings:
                    state.Selected = UIState.OpenUI.Inventory;
                    break;
                case UIState.OpenUI.BlueSettings:
                    state.Selected = UIState.OpenUI.Blueprint;
                    break;
                case UIState.OpenUI.MachSettings:
                    state.Selected = UIState.OpenUI.Machine;
                    break;
                default:
                    break;
            }
        }
        
        public void visit(OpenLoginUI login) {
            // Update if exists or add new
            var current = state.Selected;
            if (current == UIState.OpenUI.Welcome) {
                state.Selected = UIState.OpenUI.Login;
            }
        }
        
        public void visit(OpenPlayingUI playing) {
            // Update if exists or add new
            var current = state.Selected;
            if (current == UIState.OpenUI.Login) {
                state.Selected = UIState.OpenUI.Playing;
            }
        }
        
        public void visit(OpenInventoryUI inventory) {
            // Update if exists or add new
            var current = state.Selected;
            if (current == UIState.OpenUI.Playing) {
                state.Selected = UIState.OpenUI.Inventory;
            }
        }
        
        public void visit(OpenBlueprintUI blueprint) {
            // Update if exists or add new
            var current = state.Selected;
            if (current == UIState.OpenUI.Playing) {
                state.Selected = UIState.OpenUI.Blueprint;
            }
        }
                
        public void visit(OpenMachineUI machine) {
            // Update if exists or add new
            var current = state.Selected;
            if (current == UIState.OpenUI.Playing) {
                state.Selected = UIState.OpenUI.Machine;
            }
        }
        
        public void visit(OpenSettingsUI settings) {
            // Update if exists or add new
            var current = state.Selected;
            switch (current) {
                case UIState.OpenUI.Playing:
                    state.Selected = UIState.OpenUI.PlaySettings;
                    break;
                case UIState.OpenUI.Inventory:
                    state.Selected = UIState.OpenUI.InvSettings;
                    break;
                case UIState.OpenUI.Blueprint:
                    state.Selected = UIState.OpenUI.BlueSettings;
                    break;
                case UIState.OpenUI.Machine:
                    state.Selected = UIState.OpenUI.MachSettings;
                    break;
            }
        }

        public void visit(Logout logout) {
            var current = state.Selected;
            if (current == UIState.OpenUI.PlaySettings || current == UIState.OpenUI.InvSettings ||
                current == UIState.OpenUI.BlueSettings || current == UIState.OpenUI.MachSettings) {
                state.Selected = UIState.OpenUI.Welcome;
            }
        }
    }
}