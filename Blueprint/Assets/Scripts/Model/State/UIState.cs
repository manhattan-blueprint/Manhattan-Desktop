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
            MachExit
        };

        public OpenUI Selected;

        public UIState() {
// Default to in world if in unity editor simulation.
#if UNITY_EDITOR
            this.Selected = OpenUI.Playing;
#else
            this.Selected = OpenUI.Login;
#endif
        }
    }
}
