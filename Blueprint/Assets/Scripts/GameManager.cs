using Model.State;
using Model.Action;
using Model.Reducer;
using Model.Redux;
using Service.Response;

public class GameManager {
    private static GameManager manager;
    public readonly StateStore<MapState, MapAction> mapStore;
    public readonly StateStore<InventoryState, InventoryAction> inventoryStore;
    public readonly StateStore<UIState, UIAction> uiStore;
    public readonly StateStore<HeldItemState, HeldItemAction> heldItemStore;
    public readonly GameObjectsHandler goh;
    
    private UserCredentials credentials;
    
    public readonly int gridSize = 16;

    public UserCredentials GetUserCredentials() {
        return this.credentials;
    }
    
    public void SetUserCredentials(UserCredentials credentials) {
        this.credentials = credentials;
    }
    
    private GameManager() {
        this.mapStore = new StateStore<MapState, MapAction>(new MapReducer(), new MapState());
        this.inventoryStore = new StateStore<InventoryState, InventoryAction>(new InventoryReducer(), new InventoryState());
        this.uiStore = new StateStore<UIState, UIAction>(new UIReducer(), new UIState());
        this.heldItemStore = new StateStore<HeldItemState, HeldItemAction>(new HeldItemReducer(), new HeldItemState());
        
        // Load item schema from server
        this.goh = GameObjectsHandler.WithRemoteSchema();
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
