namespace Model.State {
    public class GameState {
        public InventoryState inventoryState;
        public MapState mapState;
        public UIState uiState;
        
        public GameState() {
            inventoryState = new InventoryState();
            mapState = new MapState();
            uiState = new UIState();
        }
    }
}