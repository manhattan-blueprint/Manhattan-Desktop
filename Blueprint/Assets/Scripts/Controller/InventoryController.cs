using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Model;
using Model.Action;
using Model.Reducer;
using Model.Redux;
using Model.State;
using View;

/* Attached to the player and controls inventory collection */
namespace Controller {
    public class InventoryController : MonoBehaviour, Subscriber<GameState> {
        private InventoryItem[] inventoryContents;
        private List<InventorySlotController> itemSlots;
        private const int size = 16;
        [SerializeField] private GameObject itemButton;
        private GameObject dropButton;

        public void Start() {
            inventoryContents = new InventoryItem[size];
            itemSlots = GameObject.Find("GridPanel").GetComponentsInChildren<InventorySlotController>().ToList();
            GameManager.Instance().store.Subscribe(this);
        }

        public InventoryItem[] GetItems() {
            return inventoryContents;
        }

        public void AddItem(InventoryItem item) {
            if (IsSpace()) {
                 GameManager.Instance().store.Dispatch(new AddItemToInventory(item.GetId(), item.GetQuantity()));
            } else {
                throw new System.Exception("No space in inventory.");
            }
        }

        public void StateDidUpdate(GameState state) {
            this.inventoryContents = state.inventoryState.inventoryContents;
            Debug.Log("Inventory changed somehow");
        }

        public void AddSlot(InventorySlotController slot) {
            itemSlots.Add(slot);
        }

        public string GetItemName(int id) {
            GameObjectsHandler goh = GameObjectsHandler.WithRemoteSchema();
            return goh.GameObjs.items[id + 1].name;
        }
        
        
        // Returns a slot for
        public int GetSlot(int id) {
            int firstNull = size + 1;
            for (int i = 0; i < size; i++) {
                if (inventoryContents[i] == null) {
                    if (i < firstNull) {
                        firstNull = i;
                    }
                } else if (inventoryContents[i].GetId() == id) {
                    return i;
                }
            }
            return firstNull;
        }
    
        
        public int CollectItem(Interactable focus, GameObject pickup) {
            InventoryItem newItem = new InventoryItem(focus.GetId(), 1);
            int nextSlot = GetSlot(newItem.GetId());
            // Add to inventory object
            AddItem(newItem);

            // Set to make access unique
            itemButton.name = GetNameForSlot(nextSlot);

            // Make game world object invisible and collider inactive
            Destroy(pickup);
            // Create a slot with text in inventory window, or update quantity bracket
            if (GetUISlots()[nextSlot].transform.childCount < 2) {
                Instantiate(itemButton, GetUISlots()[nextSlot].transform, false);
                GameObject.Find(GetSlotCloneName(nextSlot)).GetComponent<Text>().text =
                    focus.GetItemType();
            } else {
                GameObject.Find(GetSlotCloneName(nextSlot)).GetComponentInChildren<Text>().text =
                    focus.GetItemType() + " (" + GetItems()[nextSlot].GetQuantity() + ")";
            }

            // Change load order or UI elements for accessible hit-box
            string index = GetNameForButton(nextSlot);
            dropButton = GameObject.Find(index);
            itemButton.transform.SetSiblingIndex(0);
            dropButton.transform.SetSiblingIndex(1);

            return nextSlot;
        }

        public string GetSlotCloneName(int id) {
            return "InventoryItemSlot " + id + "(Clone)";
        }

        public string GetNameForSlot(int id) {
            return "InventoryItemSlot " + id;
        }

        public string GetNameForButton(int id) {
            return "Button" + (id + 1);
            ;
        }

        public List<InventorySlotController> GetUISlots() {
            return itemSlots;
        }

        private bool IsSpace() {
            return inventoryContents.Count(s => s != null) <= size;
        }

        public bool Equals(Object obj) {
            bool eq = false;
            if (obj.GetType() == typeof(InventoryController)) {
                InventoryController other = (InventoryController) obj;
                eq = other.GetItems().OrderBy(t => t).SequenceEqual(inventoryContents.OrderBy(t => t));
            }

            return eq;
        }
    }
}
