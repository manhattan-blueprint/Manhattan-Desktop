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
            UIState.OpenUI current = state.Selected;
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
                case UIState.OpenUI.Pause:
                    state.Selected = UIState.OpenUI.Playing;
                    break;
                case UIState.OpenUI.InvPause:
                    state.Selected = UIState.OpenUI.Inventory;
                    break;
                case UIState.OpenUI.BluePause:
                    state.Selected = UIState.OpenUI.Blueprint;
                    break;
                case UIState.OpenUI.MachPause:
                    state.Selected = UIState.OpenUI.Machine;
                    break;
                default:
                    break;
            }
        }
        
        public void visit(OpenLoginUI login) {
            // Update if exists or add new
            UIState.OpenUI current = state.Selected;
            if (current == UIState.OpenUI.Welcome) {
                state.Selected = UIState.OpenUI.Login;
            }
        }
        
        public void visit(OpenPlayingUI playing) {
            // Update if exists or add new
            UIState.OpenUI current = state.Selected;
            if (current == UIState.OpenUI.Login) {
                state.Selected = UIState.OpenUI.Playing;
            }
        }
        
        public void visit(OpenInventoryUI inventory) {
            // Update if exists or add new
            UIState.OpenUI current = state.Selected;
            if (current == UIState.OpenUI.Playing) {
                state.Selected = UIState.OpenUI.Inventory;
            }
        }
        
        public void visit(OpenBlueprintUI blueprint) {
            // Update if exists or add new
            UIState.OpenUI current = state.Selected;
            if (current == UIState.OpenUI.Playing) {
                state.Selected = UIState.OpenUI.Blueprint;
            }
        }
                
        public void visit(OpenMachineUI machine) {
            // Update if exists or add new
            UIState.OpenUI current = state.Selected;
            if (current == UIState.OpenUI.Playing) {
                state.Selected = UIState.OpenUI.Machine;
            }
        }
        
        public void visit(OpenSettingsUI settings) {
            // Update if exists or add new
            UIState.OpenUI current = state.Selected;
            switch (current) {
                case UIState.OpenUI.Playing:
                    state.Selected = UIState.OpenUI.Pause;
                    break;
                case UIState.OpenUI.Inventory:
                    state.Selected = UIState.OpenUI.InvPause;
                    break;
                case UIState.OpenUI.Blueprint:
                    state.Selected = UIState.OpenUI.BluePause;
                    break;
                case UIState.OpenUI.Machine:
                    state.Selected = UIState.OpenUI.MachPause;
                    break;
            }
        }

        public void visit(Logout logout) {
            UIState.OpenUI current = state.Selected;
            if (current == UIState.OpenUI.Pause || current == UIState.OpenUI.InvPause ||
                current == UIState.OpenUI.BluePause || current == UIState.OpenUI.MachPause) {
                state.Selected = UIState.OpenUI.Welcome;
            }
        }
    }
}