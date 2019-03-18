namespace Model {
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

        public UIState() {
            this.Selected = OpenUI.Playing;
        }
    }
}
