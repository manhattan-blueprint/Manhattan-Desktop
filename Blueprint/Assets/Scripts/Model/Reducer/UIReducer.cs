using System;
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

        public void visit(OpenUI openUI) {
            // Update if exists or add new
            var current = state.selected;
            UIState.OpenUI next = openUI.next;
            switch (next) {
                case UIState.OpenUI.Login:
                    if (current == UIState.OpenUI.Welcome) {
                        state.selected = UIState.OpenUI.Login;
                    }
                    break;
                case UIState.OpenUI.Playing:
                    if ((current == UIState.OpenUI.Playing) || (current == UIState.OpenUI.PlaySettings)) {
                        state.selected = UIState.OpenUI.Playing;
                    }
                    break;
                case UIState.OpenUI.Inventory:
                    if ((current == UIState.OpenUI.Playing) || (current == UIState.OpenUI.InvSettings)) {
                        state.selected = UIState.OpenUI.Inventory;
                    }
                    break;
                case UIState.OpenUI.Blueprint:
                    if ((current == UIState.OpenUI.Playing) || (current == UIState.OpenUI.BlueSettings)) {
                        state.selected = UIState.OpenUI.Blueprint;
                    }
                    break;
                case UIState.OpenUI.Machine:
                    if ((current == UIState.OpenUI.Playing) || (current == UIState.OpenUI.MachSettings)) {
                        state.selected = UIState.OpenUI.Machine;
                    }
                    break;
                case UIState.OpenUI.PlaySettings:
                    if (current == UIState.OpenUI.Playing) {
                        state.selected = UIState.OpenUI.PlaySettings;
                    }
                    break;
                case UIState.OpenUI.InvSettings:
                    if (current == UIState.OpenUI.Inventory) {
                        state.selected = UIState.OpenUI.InvSettings;
                    }
                    break;
                case UIState.OpenUI.BlueSettings:
                    if (current == UIState.OpenUI.Blueprint) {
                        state.selected = UIState.OpenUI.BlueSettings;
                    }
                    break;
                case UIState.OpenUI.MachSettings:
                    if (current == UIState.OpenUI.Machine) {
                        state.selected = UIState.OpenUI.MachSettings;
                    }
                    break;
                default:
                    break;
            }
        }

        public void visit(CloseUI closeUI) {
            var current = state.selected;
            switch (current) {
                case UIState.OpenUI.Inventory:
                    state.selected = UIState.OpenUI.Playing;
                    break;
                case UIState.OpenUI.Blueprint:
                    state.selected = UIState.OpenUI.Playing;
                    break;
                case UIState.OpenUI.Machine:
                    state.selected = UIState.OpenUI.Playing;
                    break;
                case UIState.OpenUI.PlaySettings:
                    state.selected = UIState.OpenUI.Playing;
                    break;
                case UIState.OpenUI.InvSettings:
                    state.selected = UIState.OpenUI.Inventory;
                    break;
                case UIState.OpenUI.BlueSettings:
                    state.selected = UIState.OpenUI.Blueprint;
                    break;
                case UIState.OpenUI.MachSettings:
                    state.selected = UIState.OpenUI.Machine;
                    break;
                default:
                    break;
            }
        }
    }
}