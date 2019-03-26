using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Controller;
using FullSerializer;
using Model;
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
    public readonly int inventoryLayers = 2;

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
        // Calculate the number of inventory slots and set inventory size, i.e. 3n^2 - 3n + 1 + numberOfHeldItem slots - 1 for zero indexing
        inventoryStore.Dispatch(new SetInventorySize((int) (3 * Math.Pow(inventoryLayers + 1, 2) - 3 * (inventoryLayers + 1) + 6)));

        BlueprintAPI blueprintApi = BlueprintAPI.DefaultCredentials();

        // TODO: FIX ALL THIS (by moving API to coroutines)
        
        Task.Run(async () => {
            
            // Load desktop state
            APIResult<GameState, JsonError> finalGameStateResponse = await blueprintApi.AsyncGetState(credentials);
            if (finalGameStateResponse.isSuccess()) {
                GameState remoteGameState = finalGameStateResponse.GetSuccess();
                mapStore.SetState(remoteGameState.mapState);
                heldItemStore.SetState(remoteGameState.heldItemState);
                inventoryStore.SetState(remoteGameState.inventoryState);
            } else {
                // TODO: Do something with this error
                JsonError error = finalGameStateResponse.GetError();
            }
            
            // Load player backpack into inventory
            APIResult<ResponseGetInventory, JsonError> finalInventoryResponse = await blueprintApi.AsyncGetInventory(credentials);
            
            // Delete backpack contents from server
            APIResult<Boolean, JsonError> finalDeleteInventoryResponse = await blueprintApi.AsyncDeleteInventory(credentials);
            
            if (finalInventoryResponse.isSuccess()) {
                ResponseGetInventory remoteInv = finalInventoryResponse.GetSuccess();
                foreach (InventoryEntry entry in remoteInv.items) {
                    inventoryStore.Dispatch(
                        new AddItemToInventory(entry.item_id, entry.quantity, goh.GameObjs.items[entry.item_id - 1].name));
                }
                // Can't call a scene dispatch in an asynchronous function.
                this.uiStore.Dispatch(new OpenPlayingUI());
            } else {
                // TODO: Do something with this error
                JsonError error = finalInventoryResponse.GetError();
            }

            if (!finalDeleteInventoryResponse.isSuccess()) {
                // TODO: Do something with this error
                JsonError error = finalDeleteInventoryResponse.GetError();
            }
        }).GetAwaiter().GetResult();
    }

    public void ResetGame() {
        manager = new GameManager();
    }
}
