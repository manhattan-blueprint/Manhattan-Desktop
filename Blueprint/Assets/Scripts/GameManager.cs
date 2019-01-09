﻿using Model;
using Model.Action;
using Model.Reducer;
using Model.Redux;
using Model.State;

public class GameManager {
    private class GameStateReducer : Reducer<GameState, Action> {
        private readonly InventoryReducer inventoryReducer;
        private readonly MapReducer mapReducer;

        public GameStateReducer() {
            inventoryReducer = new InventoryReducer();
            mapReducer = new MapReducer();
        }
       
        // Dispatch to appropriate handler
        public GameState Reduce(GameState current, Action action) {
            if (action is InventoryAction){
                current.inventoryState = inventoryReducer.Reduce(current.inventoryState, (InventoryAction) action);
            } else if (action is MapAction) {
                current.mapState = mapReducer.Reduce(current.mapState, (MapAction) action);
            }
            return current;
        }
    }
    
    private static GameManager manager;
    public readonly StateStore<GameState, Action> store; 

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