using System;
using Model.State;
using UnityEngine;

/* Wrapper for the states to transfer to/from the server */
namespace Model {
    [Serializable]
    public class GameState {
        [SerializeField] private MapState mapState;
        [SerializeField] private HeldItemState heldItemState;
        [SerializeField] private InventoryState inventoryState;

        public GameState(MapState mapState, HeldItemState heldItemState, InventoryState inventoryState) {
            this.mapState = mapState;
            this.heldItemState = heldItemState;
            this.inventoryState = inventoryState;
        }
    }
}