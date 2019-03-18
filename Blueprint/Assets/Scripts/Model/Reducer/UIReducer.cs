using Model.Action;

namespace Model.Reducer {
    // This reducer allows one to open UIs and close UIs. Note that it does not
    // a user to open a playing state or leave a playing state.
    public class UIReducer : Reducer<UIState, UIAction>, UIVisitor {
        private UIState state;

        public UIState Reduce(UIState current, UIAction action) {
            this.state = current;
            // Dispatch to visitor which will manipulate state
            action.Accept(this);
            return this.state;
        }

        // Returns to the previous UI/world in action depending on which UI the
        // user was in.
        public void visit(CloseUI closeUI) {
            UIState.OpenUI current = state.Selected;
            switch (current) {
                case UIState.OpenUI.MainMenu:
                    state.Selected = UIState.OpenUI.Playing;
                    break;
                case UIState.OpenUI.Playing:
                    throw new System.Exception("Attempting to close UI menu using reducer though no menu UIs should be open.");
                    break;
                case UIState.OpenUI.PauseMenu:
                    state.Selected = UIState.OpenUI.Playing;
                    break;
                case UIState.OpenUI.Inventory:
                    state.Selected = UIState.OpenUI.Playing;
                    break;
                case UIState.OpenUI.Blueprint:
                    state.Selected = UIState.OpenUI.Playing;
                    break;
                case UIState.OpenUI.Machine:
                    state.Selected = UIState.OpenUI.Playing;
                    break;
                default:
                    throw new System.Exception("Current state does not exist. Git blame Will.");
                    break;
            }
        }

        public void visit(OpenMainMenuUI mainMenu) {
            UIState.OpenUI current = state.Selected;
            if (current == UIState.OpenUI.PauseMenu) {
                state.Selected = UIState.OpenUI.MainMenu;
            } else {
                throw new System.Exception("Attempting to return to main menu from state other than the pause menu.");
            }
        }

        public void visit(Logout logout) {
            UIState.OpenUI current = state.Selected;
            if (current == UIState.OpenUI.Pause || current == UIState.OpenUI.InvPause ||
                current == UIState.OpenUI.BluePause || current == UIState.OpenUI.MachPause) {
                state.Selected = UIState.OpenUI.Welcome;
            }
        }

        public void visit(OpenInventoryUI inventory) {
            UIState.OpenUI current = state.Selected;
            if (current == UIState.OpenUI.Playing) {
                state.Selected = UIState.OpenUI.Inventory;
            } else {
                throw new System.Exception("Attempting to open inventory UI from state other than playing.");
            }
        }

        public void visit(OpenBlueprintUI blueprint) {
            // Update if exists or add new
            UIState.OpenUI current = state.Selected;
            if (current == UIState.OpenUI.Playing) {
                state.Selected = UIState.OpenUI.Blueprint;
            } else {
                throw new System.Exception("Attempting to open blueprint UI from state other than playing.");
            }
        }

        public void visit(OpenMachineUI machine) {
            // Update if exists or add new
            UIState.OpenUI current = state.Selected;
            if (current == UIState.OpenUI.Playing) {
                state.Selected = UIState.OpenUI.Machine;
            } else {
                throw new System.Exception("Attempting to open machine UI from state other than playing.");
            }
        }
    }
}
