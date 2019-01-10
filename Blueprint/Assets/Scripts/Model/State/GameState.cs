namespace Model.State {
    public class GameState {
        public InventoryState inventoryState;
        public MapState mapState;
        
        public GameState() {
            inventoryState = new InventoryState();
            mapState = new MapState();
        }
    }
}