using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Model;
using Model.State;
using Model.Action;
using Model.Reducer;
using Model.Redux;
using Service;
using Service.Request;
using Service.Response;
using UnityEngine;

public class GameManager {
    private static GameManager manager;
    public readonly StateStore<MapState, MapAction> mapStore;
    public readonly StateStore<InventoryState, InventoryAction> inventoryStore;
    public readonly StateStore<UIState, UIAction> uiStore;
    public readonly StateStore<HeldItemState, HeldItemAction> heldItemStore;
    public readonly StateStore<MachineState, MachineAction> machineStore;
    public SchemaManager sm;
    public List<Item> completedBlueprints;
    private AccessToken accessToken;

    public readonly int gridSize = 16;
    public readonly int inventoryLayers = 2;

    private GameManager() {
        this.mapStore = new StateStore<MapState, MapAction>(new MapReducer(), new MapState());
        this.inventoryStore = new StateStore<InventoryState, InventoryAction>(new InventoryReducer(), new InventoryState());
        this.uiStore = new StateStore<UIState, UIAction>(new UIReducer(), new UIState());
        this.heldItemStore = new StateStore<HeldItemState, HeldItemAction>(new HeldItemReducer(), new HeldItemState());
        this.machineStore = new StateStore<MachineState, MachineAction>(new MachineReducer(), new MachineState());
    }

    public static GameManager Instance() {
        if (manager == null) {
            manager = new GameManager();
        }
        return manager;
    }

    public void ConfigureGame(SchemaItems schemaItems, GameState gameState, List<InventoryEntry> inventoryEntries) {
        this.sm = new SchemaManager(schemaItems);
        mapStore.SetState(gameState.mapState);
        heldItemStore.SetState(gameState.heldItemState);
        inventoryStore.SetState(gameState.inventoryState);
        machineStore.SetState(gameState.machineState);

        // Calculate the number of inventory slots and set inventory size, i.e. 3n^2 - 3n + 1 + numberOfHeldItem slots - 1 for zero indexing
        // This must be done after setting state, overriding any previous value
        inventoryStore.Dispatch(
            new SetInventorySize((int) (3 * Math.Pow(inventoryLayers + 1, 2) - 3 * (inventoryLayers + 1) + 6)));

        foreach (InventoryEntry entry in inventoryEntries) {
            inventoryStore.Dispatch(new AddItemToInventory(entry.item_id, entry.quantity,
                sm.GameObjs.items[entry.item_id - 1].name));
        }
        // Update which machines are connected when loading from save state
        machineStore.Dispatch(new UpdateConnected());
    }

    public void ResetGame() {
        manager = new GameManager();
    }

    public AccessToken GetAccessToken() {
        return this.accessToken;
    }

    public void SetAccessToken(AccessToken accessToken) {
        this.accessToken = accessToken;
    }

}
