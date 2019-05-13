using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Model;
using Model.Action;
using Model.Redux;
using Model.State;
using Service;
using Service.Response;
using UnityEngine.EventSystems;
using Utils;

/* Attached to the inventory canvas and controls inventory collection */
namespace Controller {
    public class InventoryController : MonoBehaviour, Subscriber<InventoryState>, Subscriber<UIState> {
        public Dictionary<int, List<HexLocation>> inventoryContents;
        public bool DraggingInvItem;
        public int DragDestination;

        private Dictionary<int, InventorySlotController> itemSlots;
        private GameManager gameManager;
        private bool firstUIUpdate;
        private List<InventorySlotController> allSlots;
        public Button backpackButton;
        public List<InventoryEntry> backpackContents;
             
        public void Start() {
            firstUIUpdate = true;
            itemSlots = new Dictionary<int, InventorySlotController>();
            backpackContents = new List<InventoryEntry>();
        }

        private void subscribeToInventory() {
            GameManager.Instance().inventoryStore.Subscribe(this);
        }

        void Update() {
            if (firstUIUpdate) {
                allSlots = gameObject.GetComponentsInChildren<InventorySlotController>().ToList();
                
                foreach (InventorySlotController controller in allSlots) {
                  itemSlots.Add(controller.getId(), controller);
                }

                firstUIUpdate = false;

                // *MUST* subscribe *AFTER* finishing configuring the UI.
                GameManager.Instance().uiStore.Subscribe(this);
                Invoke(nameof(subscribeToInventory), 5);
            }
        }
        
        public void StateDidUpdate(InventoryState state) {
            inventoryContents = state.inventoryContents;
            RedrawInventory();
        }

        public void StateDidUpdate(UIState state) {
            if (state.Selected != UIState.OpenUI.Inventory || gameObject.name == "MachineInventoryCanvas") return;
            
            // If inventory UI opened, check how many things the user has in their backpack and populate UI
            StartCoroutine(BlueprintAPI.GetInventory(GameManager.Instance().GetAccessToken(), result => {
                if (!result.isSuccess()) {
                    this.ShowAlert("Error", "Could not get inventory " + result.GetError());
                } else {
                    backpackContents = result.GetSuccess().items;
                    SetBackpackState();
                }
            }));
        }

        private bool isInventoryFull() {
            foreach (InventorySlotController isc in allSlots) {
                if (!isc.storedItem.IsPresent()) {
                    return false;
                }
            }

            return true;
        }

        public void SetBackpackState() {
            if (backpackContents.Count > 0) {
                SpriteState ss = new SpriteState {
                    highlightedSprite = AssetManager.Instance().backpackButtonOccupiedHighlight
                };
                backpackButton.spriteState = ss;
                backpackButton.GetComponent<Image>().sprite = AssetManager.Instance().backpackButtonOccupied;
                backpackButton.GetComponentInChildren<Text>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            } else {
                SpriteState ss = new SpriteState {
                    highlightedSprite = AssetManager.Instance().backpackButtonUnoccupiedHighlight
                };

                backpackButton.spriteState = ss;
                backpackButton.GetComponent<Image>().sprite = AssetManager.Instance().backpackButtonUnoccupied;
                backpackButton.GetComponentInChildren<Text>().color = new Color(1.0f, 1.0f, 1.0f, 0.3f);
            }
            backpackButton.GetComponentInChildren<Text>().text = backpackContents.Count.ToString();
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

        public void OnBackpackClick() {
            if (isInventoryFull()) {
                this.ShowAlert("Inventory Full!", "Please make more space before retrieving backpack contents...");
                return;
            }

            foreach (InventoryEntry entry in backpackContents) {
                GameManager.Instance().inventoryStore.Dispatch(new AddItemToInventory(entry.item_id, entry.quantity));
            }
            backpackContents.Clear();
            SetBackpackState();
            
            // Save game to avoid losing resources if they don't save again
            GameState gameState = new GameState(GameManager.Instance().mapStore.GetState(),
                GameManager.Instance().heldItemStore.GetState(),
                GameManager.Instance().inventoryStore.GetState(),
                GameManager.Instance().machineStore.GetState());

            StartCoroutine(BlueprintAPI.SaveGameState(GameManager.Instance().GetAccessToken(), gameState, result => {
                if (!result.isSuccess()) {
                    this.ShowAlert("Error", "Could not get inventory " + result.GetError());
                }
            }));

            // Delete backpack items
            StartCoroutine(BlueprintAPI.DeleteInventory(GameManager.Instance().GetAccessToken(), result => {
                if (!result.isSuccess()) {
                    this.ShowAlert("Error", "Could not clear inventory " + result.GetError());
                } 
            }));
            
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
