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
            Gate,
            GateMouse,
            BeaconMouse,
            Intro,
            EndGame
        };

        public OpenUI Selected;
        public Vector2 SelectedMachineLocation;
        public int SelectedBlueprintID = -1;
        public bool fromGateRMB = false;
        public bool fromBeaconRMB = false;

        public UIState() {
            this.Selected = OpenUI.Login;
        }
    }
}
