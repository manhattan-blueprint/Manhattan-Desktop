namespace Model {
    public class UIState {
        public enum OpenUI {
            MainMenu,   // Splash screen, login and register menus before the world is loaded.
            Playing,    // In world and no menus open.
            PauseMenu,  // In world and pause/settings menu visible, world paused.
            Inventory,  // In world and inventory UI open, world paused.
            Blueprint,  // In world and blueprint UI open, world paused.
            Machine     // In world and some machine UI open, world paused.
        };

        public OpenUI Selected;

        public UIState() {
// Default to world rather than main menu if in unity editor simulation.
#if UNITY_EDITOR
            this.Selected = OpenUI.Playing;
#else
            this.Selected = OpenUI.MainMenu;
#endif
        }
    }
}
