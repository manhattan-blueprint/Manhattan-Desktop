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
            Bindings,
        };

        public OpenUI Selected;

        public UIState() {
            this.Selected = OpenUI.Playing;
        }
    }
}
