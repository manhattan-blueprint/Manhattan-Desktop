using UnityEngine;

namespace Model.State {
    public class UIState {
        public enum OpenUI {
            Welcome,
            Login,
            Playing,
            Inventory,
            Blueprint,
            BlueprintTemplate,
            Machine,
            Goal,
            Pause,
            Exit,
            Logout,
            BindingsPause,
            BindingsIntro,
            Gate,
            GateMouse,
            BeaconMouse,
            Intro,
        };

        public OpenUI Selected;
        public Vector2 SelectedMachineLocation;
        public int SelectedBlueprintID = -1;
        public bool ShouldShowHelpUI = false;

        public UIState() {
            this.Selected = OpenUI.Login;
        }
    }
}
