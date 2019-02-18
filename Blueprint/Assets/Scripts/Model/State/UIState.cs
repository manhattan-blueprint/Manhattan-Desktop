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
            MachPause 
        };

        public OpenUI Selected;

        public UIState() {
            this.Selected = OpenUI.Login;
        }
    }
}
