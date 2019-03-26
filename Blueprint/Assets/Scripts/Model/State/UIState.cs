using UnityEngine;

namespace Model.State {
    public class UIState {
        public enum OpenUI {
            Welcome,
            Login,
            Playing,
            Inventory,
            Blueprint,
            Machine,
            Pause,
            InvPause,
            BluePause,
            MachPause,
            Exit,
            InvExit,
            BlueExit,
            MachExit,
            Logout,
            InvLogout,
            BlueLogout,
            MachLogout,
        };

        public OpenUI Selected;
        public Vector2 SelectedMachineLocation;

        public UIState() {
#if UNITY_EDITOR
            this.Selected = OpenUI.Playing;
#else
            this.Selected = OpenUI.Login;
#endif
        }
    }
}
