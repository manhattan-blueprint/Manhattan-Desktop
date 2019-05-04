using System.Collections.Generic;
using System.IO;
using System.Net;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using Model;
using Model.Action;
using Model.Redux;
using Model.State;
using Utils;
using View;
using Service;
using Service.Response;
using System.Threading.Tasks;
using UnityEngine.Assertions.Must;
using UnityEngine.Experimental.Rendering;

/* Attached to the inventory canvas and controls inventory collection */
namespace Controller {
    public class InventoryController : MonoBehaviour, Subscriber<InventoryState>, Subscriber<UIState> {
        public Dictionary<int, List<HexLocation>> inventoryContents;
        public bool DraggingInvItem;
        public int DragDestination;
        
        private Dictionary<int, InventorySlotController> itemSlots;
        private GameManager gameManager;
        private bool firstUIUpdate;
        private List<InventoryEntry> backpackContents;

        public void Start() {
            firstUIUpdate = true;
            itemSlots = new Dictionary<int, InventorySlotController>();
            backpackContents = new List<InventoryEntry>();
        }

        void Update() {
            if (firstUIUpdate) {
                List<InventorySlotController> allSlots = gameObject.GetComponentsInChildren<InventorySlotController>().ToList();
                
                foreach (InventorySlotController controller in allSlots) {
                  itemSlots.Add(controller.getId(), controller);
                }
                firstUIUpdate = false;

                // *MUST* subscribe *AFTER* finishing configuring the UI.
                GameManager.Instance().inventoryStore.Subscribe(this);
                GameManager.Instance().uiStore.Subscribe(this);
            }
        }

        public void StateDidUpdate(InventoryState state) {
            inventoryContents = state.inventoryContents;
            RedrawInventory();
        }

        public void StateDidUpdate(UIState state) {
            if (state.Selected != UIState.OpenUI.Inventory) return;
            
            // If inventory UI opened, check how many things the user has in their backpack and populate UI
            StartCoroutine(BlueprintAPI.GetInventory(GameManager.Instance().GetAccessToken(), result => {
                if (!result.isSuccess()) {
                    Debug.LogError("Could not get inventory: " + result.GetError()) ;
                    // TODO
                } else {
                    backpackContents = result.GetSuccess().items; 
                    Debug.Log("The user has " + backpackContents.Count + " items in their backpack");
                    // TODO: Update UI
                }
            }));
        }

        public void RedrawInventory() {
            // Clear slots
            foreach (KeyValuePair<int, InventorySlotController> slot in itemSlots) {
                slot.Value.SetStoredItem(Optional<InventoryItem>.Empty());
            }

            // Re-populate slots
            foreach (KeyValuePair<int, List<HexLocation>> element in inventoryContents) {
                foreach(HexLocation loc in element.Value) {
                    string itemName = GameManager.Instance().sm.GameObjs.items.Find(x => x.item_id == element.Key).name;
                    InventoryItem item = new InventoryItem(itemName, element.Key, loc.quantity);
                    itemSlots[loc.hexID].SetStoredItem(Optional<InventoryItem>.Of(item));
                }
            }
        }

        private void TransferBackpackToInventory() {
            foreach (InventoryEntry entry in backpackContents) {
                GameManager.Instance().inventoryStore.Dispatch(new AddItemToInventory(entry.item_id, entry.quantity));
            }
            
            // Save game to avoid losing resources if they don't save again
            GameState gameState = new GameState(GameManager.Instance().mapStore.GetState(),
                GameManager.Instance().heldItemStore.GetState(),
                GameManager.Instance().inventoryStore.GetState(),
                GameManager.Instance().machineStore.GetState());

            StartCoroutine(BlueprintAPI.SaveGameState(GameManager.Instance().GetAccessToken(), gameState, result => {
                if (!result.isSuccess()) {
                    // TODO
                } else {
                    // TODO: Handle failure via UI?
                }
            }));

            // Delete backpack items
            StartCoroutine(BlueprintAPI.DeleteInventory(GameManager.Instance().GetAccessToken(), result => {
                if (!result.isSuccess()) {
                    Debug.LogError("Could not delete inventory: " + result.GetError()) ;
                    // TODO
                } else {
                    Debug.Log("Deleting was a success");
                }
            }));
        }
    }
}
