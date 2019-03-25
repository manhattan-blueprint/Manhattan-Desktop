using Model.Action;
using Model.State;

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
                case UIState.OpenUI.Bindings:
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
                case UIState.OpenUI.Logout:
                    state.Selected = UIState.OpenUI.Pause;
                    break;
                case UIState.OpenUI.InvLogout:
                    state.Selected = UIState.OpenUI.InvPause;
                    break;
                case UIState.OpenUI.BlueLogout:
                    state.Selected = UIState.OpenUI.BluePause;
                    break;
                case UIState.OpenUI.MachLogout:
                    state.Selected = UIState.OpenUI.MachPause;
                    break;
                case UIState.OpenUI.Exit:
                    state.Selected = UIState.OpenUI.Pause;
                    break;
                case UIState.OpenUI.InvExit:
                    state.Selected = UIState.OpenUI.InvPause;
                    break;
                case UIState.OpenUI.BlueExit:
                    state.Selected = UIState.OpenUI.BluePause;
                    break;
                case UIState.OpenUI.MachExit:
                    state.Selected = UIState.OpenUI.MachPause;
                    break;
                default:
                    break;
            }
        }

        public void visit(OpenLoginUI login) {
            // Update if exists or add new
            UIState.OpenUI current = state.Selected;
            if (current == UIState.OpenUI.Welcome || current == UIState.OpenUI.Logout
                || current == UIState.OpenUI.InvLogout || current == UIState.OpenUI.BlueLogout
                || current == UIState.OpenUI.MachLogout) {
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

        public void visit(OpenBindingsUI blueprint) {
            // Update if exists or add new
            UIState.OpenUI current = state.Selected;
            if (current == UIState.OpenUI.Playing) {
                state.Selected = UIState.OpenUI.Bindings;
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
                case UIState.OpenUI.Exit:
                    state.Selected = UIState.OpenUI.Pause;
                    break;
                case UIState.OpenUI.InvExit:
                    state.Selected = UIState.OpenUI.InvPause;
                    break;
                case UIState.OpenUI.BlueExit:
                    state.Selected = UIState.OpenUI.BluePause;
                    break;
                case UIState.OpenUI.MachExit:
                    state.Selected = UIState.OpenUI.MachPause;
                    break;
                case UIState.OpenUI.Logout:
                    state.Selected = UIState.OpenUI.Pause;
                    break;
                case UIState.OpenUI.InvLogout:
                    state.Selected = UIState.OpenUI.InvPause;
                    break;
                case UIState.OpenUI.BlueLogout:
                    state.Selected = UIState.OpenUI.BluePause;
                    break;
                case UIState.OpenUI.MachLogout:
                    state.Selected = UIState.OpenUI.MachPause;
                    break;
            }
        }

        public void visit(Logout logout) {
            UIState.OpenUI current = state.Selected;
            switch (current) {
                case UIState.OpenUI.Pause:
                    state.Selected = UIState.OpenUI.Logout;
                    break;
                case UIState.OpenUI.InvPause:
                    state.Selected = UIState.OpenUI.InvLogout;
                    break;
                case UIState.OpenUI.BluePause:
                    state.Selected = UIState.OpenUI.BlueLogout;
                    break;
                case UIState.OpenUI.MachPause:
                    state.Selected = UIState.OpenUI.MachLogout;
                    break;
            }
        }

        public void visit(Exit exit) {
            UIState.OpenUI current = state.Selected;
            switch (current) {
                case UIState.OpenUI.Pause:
                    state.Selected = UIState.OpenUI.Exit;
                    break;
                case UIState.OpenUI.InvPause:
                    state.Selected = UIState.OpenUI.InvExit;
                    break;
                case UIState.OpenUI.BluePause:
                    state.Selected = UIState.OpenUI.BlueExit;
                    break;
                case UIState.OpenUI.MachPause:
                    state.Selected = UIState.OpenUI.MachExit;
                    break;
            }
        }
    }
}
