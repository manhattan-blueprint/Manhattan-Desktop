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
        
        public void visit(OpenLoginUI login) {
            // Update if exists or add new
            var current = state.selected;
            if (current == UIState.OpenUI.Welcome) {
                state.selected = UIState.OpenUI.Login;
            }
        }
        
        public void visit(OpenPlayingUI playing) {
            // Update if exists or add new
            var current = state.selected;
            if (current == UIState.OpenUI.Login) {
                state.selected = UIState.OpenUI.Playing;
            }
        }
        
        public void visit(OpenInventoryUI inventory) {
            // Update if exists or add new
            var current = state.selected;
            if (current == UIState.OpenUI.Playing) {
                state.selected = UIState.OpenUI.Inventory;
            }
        }
        
        public void visit(OpenBlueprintUI blueprint) {
            // Update if exists or add new
            var current = state.selected;
            if (current == UIState.OpenUI.Playing) {
                state.selected = UIState.OpenUI.Blueprint;
            }
        }
                
        public void visit(OpenMachineUI machine) {
            // Update if exists or add new
            var current = state.selected;
            if (current == UIState.OpenUI.Playing) {
                state.selected = UIState.OpenUI.Machine;
            }
        }
        
        public void visit(OpenSettingsUI settings) {
            // Update if exists or add new
            var current = state.selected;
            switch (current) {
                case UIState.OpenUI.Playing:
                    state.selected = UIState.OpenUI.PlaySettings;
                    break;
                case UIState.OpenUI.Inventory:
                    state.selected = UIState.OpenUI.InvSettings;
                    break;
                case UIState.OpenUI.Blueprint:
                    state.selected = UIState.OpenUI.BlueSettings;
                    break;
                case UIState.OpenUI.Machine:
                    state.selected = UIState.OpenUI.MachSettings;
                    break;
            }
        }

        public void visit(Logout logout) {
            var current = state.selected;
            if (current == UIState.OpenUI.PlaySettings || current == UIState.OpenUI.InvSettings ||
                current == UIState.OpenUI.BlueSettings || current == UIState.OpenUI.MachSettings) {
                state.selected = UIState.OpenUI.Welcome;
            }
        }
    }
}