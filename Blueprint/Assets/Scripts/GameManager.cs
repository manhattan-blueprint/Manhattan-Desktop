using System.Threading.Tasks;
using Controller;
using Model.State;
using Model.Action;
using Model.Reducer;
using Model.Redux;
using Service;
using Service.Response;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void StartGame() {
        BlueprintAPI blueprintApi = BlueprintAPI.DefaultCredentials();
        
        // Load player inventory and then load world
        Task.Run(async () => {
            APIResult<ResponseGetInventory, JsonError> finalInventoryResponse = await blueprintApi.AsyncGetInventory(credentials);
            if (finalInventoryResponse.isSuccess()) {
                ResponseGetInventory remoteInv = finalInventoryResponse.GetSuccess();
                foreach (InventoryEntry entry in remoteInv.items) {
                    inventoryStore.Dispatch(
                        new AddItemToInventory(entry.item_id, entry.quantity, goh.GameObjs.items[entry.item_id - 1].name));
                }
                SceneManager.LoadScene(SceneMapping.World);
            } else {
                JsonError error = finalInventoryResponse.GetError();
            }
        }).GetAwaiter().GetResult();
    }

    public void ResetGame() {
        manager = new GameManager();
    }
}
