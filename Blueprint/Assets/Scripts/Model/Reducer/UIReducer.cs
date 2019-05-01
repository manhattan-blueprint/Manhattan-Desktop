using System;
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
                case UIState.OpenUI.Blueprint:
                case UIState.OpenUI.Machine:
                case UIState.OpenUI.Pause:
                case UIState.OpenUI.Bindings:
                case UIState.OpenUI.Mouse:
                case UIState.OpenUI.Intro:
                    state.Selected = UIState.OpenUI.Playing;
                    break;
                case UIState.OpenUI.Gate:
                    state.Selected = UIState.OpenUI.Mouse;
                    break;
                case UIState.OpenUI.Logout:
                case UIState.OpenUI.Exit:
                    state.Selected = UIState.OpenUI.Pause;
                    break;
                default:
                    throw new Exception("Invalid state transition. Cannot transition from " + current + " to CloseUI");
            }
        }

        public void visit(OpenLoginUI login) {
            UIState.OpenUI current = state.Selected;
            switch (current) {
                case UIState.OpenUI.Welcome:
                case UIState.OpenUI.Logout:
                    state.Selected = UIState.OpenUI.Login;
                    break;
                default:
                    throw new Exception("Invalid state transition. Cannot transition from " + current + " to OpenLoginUI");
            }
        }

        public void visit(OpenPlayingUI playing) {
            UIState.OpenUI current = state.Selected;
            switch (current) {
                case UIState.OpenUI.Login:
                    state.Selected = UIState.OpenUI.Playing;
                    break;
                default:
                    throw new Exception("Invalid state transition. Cannot transition from " + current + " to OpenPlayingUI");
            }
        }

        public void visit(OpenInventoryUI inventory) {
            UIState.OpenUI current = state.Selected;
            switch (current) {
                case UIState.OpenUI.Playing:
                    state.Selected = UIState.OpenUI.Inventory;
                    break;
                default:
                    throw new Exception("Invalid state transition. Cannot transition from " + current + " to OpenInventoryUI");
            }
        }

        public void visit(OpenBlueprintUI blueprint) {
            UIState.OpenUI current = state.Selected;
            switch (current) {
                case UIState.OpenUI.Playing:
                    state.Selected = UIState.OpenUI.Blueprint;
                    break;
                default:
                    throw new Exception("Invalid state transition. Cannot transition from " + current + " to OpenBlueprintUI");
            }
        }

        public void visit(OpenBindingsUI blueprint) {
            UIState.OpenUI current = state.Selected;
            switch (current) {
                case UIState.OpenUI.Playing:
                    state.Selected = UIState.OpenUI.Bindings;
                    break;
                default:
                    throw new Exception("Invalid state transition. Cannot transition from " + current + " to OpenBindingsUI");
            }
        }

        public void visit(OpenMouseUI mouse) {
            UIState.OpenUI current = state.Selected;
            switch (current) {
                case UIState.OpenUI.Playing:
                    state.Selected = UIState.OpenUI.Mouse;
                    break;
                default:
                    throw new Exception("Invalid state transition. Cannot transition from " + current + " to OpenMouseUI");
            }
        }

        public void visit(OpenGateUI gate) {
            UIState.OpenUI current = state.Selected;
            switch (current) {
                case UIState.OpenUI.Mouse:
                    state.Selected = UIState.OpenUI.Gate;
                    break;
                default:
                    throw new Exception("Invalid state transition. Cannot transition from " + current + " to OpenGateUI");
            }
        }

        public void visit(OpenIntroUI intro) {
            UIState.OpenUI current = state.Selected;
            switch (current) {
                case UIState.OpenUI.Playing:
                    state.Selected = UIState.OpenUI.Intro;
                    break;
                default:
                    throw new Exception("Invalid state transition. Cannot transition from " + current + " to OpenIntroUI");
            }
        }

        public void visit(OpenMachineUI machine) {
            UIState.OpenUI current = state.Selected;
            switch (current) {
                case UIState.OpenUI.Playing:
                    state.Selected = UIState.OpenUI.Machine;
                    state.SelectedMachineLocation = machine.machinePosition;
                    break;
                default:
                    throw new Exception("Invalid state transition. Cannot transition from " + current + " to OpenMachineUI");
            }
        }

        public void visit(OpenSettingsUI settings) {
            UIState.OpenUI current = state.Selected;
            switch (current) {
                case UIState.OpenUI.Playing:
                case UIState.OpenUI.Exit:
                case UIState.OpenUI.Logout:
                    state.Selected = UIState.OpenUI.Pause;
                    break;
                default:
                    throw new Exception("Invalid state transition. Cannot transition from " + current + " to OpenSettingsUI");
            }
        }

        public void visit(Logout logout) {
            UIState.OpenUI current = state.Selected;
            switch (current) {
                case UIState.OpenUI.Pause:
                    state.Selected = UIState.OpenUI.Logout;
                    break;
                default:
                    throw new Exception("Invalid state transition. Cannot transition from " + current + " to Logout");
            }
        }

        public void visit(Exit exit) {
            UIState.OpenUI current = state.Selected;
            switch (current) {
                case UIState.OpenUI.Pause:
                    state.Selected = UIState.OpenUI.Exit;
                    break;
                default:
                    throw new Exception("Invalid state transition. Cannot transition from " + current + " to Exit");
            }
        }
    }
}
