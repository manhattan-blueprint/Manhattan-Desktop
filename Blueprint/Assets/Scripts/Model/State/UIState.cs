namespace Model {
    public class UIState {
        public enum OpenUI {
            Welcome,
            Login,
            Playing,
            Inventory,
            Blueprint,
            Machine,
            PlaySettings,
            InvSettings,
            BlueSettings,
            MachSettings
        };

        public OpenUI selected;

        public UIState() {
            this.selected = OpenUI.Welcome;
        }
    }
}