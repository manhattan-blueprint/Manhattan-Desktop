using Model;
using Model.Action;
using Model.Reducer;
using Model.Redux;
using Model.State;
using Service;
using Service.Response;

public class GameManager {
    private class GameStateReducer : Reducer<GameState, Action> {
        private readonly InventoryReducer inventoryReducer;
        private readonly MapReducer mapReducer;
        private readonly UIReducer uiReducer;

        public GameStateReducer() {
            inventoryReducer = new InventoryReducer();
            mapReducer = new MapReducer();
            uiReducer = new UIReducer();
        }
       
        // Dispatch to appropriate handler
        public GameState Reduce(GameState current, Action action) {
            if (action is InventoryAction){
                current.inventoryState = inventoryReducer.Reduce(current.inventoryState, (InventoryAction) action);
            } else if (action is MapAction) {
                current.mapState = mapReducer.Reduce(current.mapState, (MapAction) action);
            } else if (action is UIAction) {
                current.uiState = uiReducer.Reduce(current.uiState, (UIAction) action);
            }
            return current;
        }
    }
    
    private static GameManager manager;
    public readonly StateStore<GameState, Action> store;
    private UserCredentials credentials;

    public UserCredentials GetUserCredentials() {
        return this.credentials;
    }
    
    public void SetUserCredentials(UserCredentials credentials) {
        this.credentials = credentials;
    }
    
    private GameManager() {
        this.store = new StateStore<GameState, Action>(new GameStateReducer(), new GameState());
    }
    
    public static GameManager Instance() {
        if (manager == null) {
            manager = new GameManager();
        }
        return manager;
    }

    public void ResetGame() {
        manager = new GameManager();
    }
}
