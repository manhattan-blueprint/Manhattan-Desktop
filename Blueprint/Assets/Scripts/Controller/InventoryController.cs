using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Model;
using View;

/* Attached to the player and controls inventory collection */
namespace Controller {
    public class InventoryController : MonoBehaviour {
        private InventoryItem[] items;
        private List<InventorySlotController> itemSlots;
        private const int size = 16;
        [SerializeField] private GameObject itemButton;
        private GameObject dropButton;
        private GameObject heldItem;
        private int currentHeld;

        public void Start() {
            items = new InventoryItem[size];
            itemSlots = GameObject.Find("GridPanel").GetComponentsInChildren<InventorySlotController>().ToList();
            heldItem = GameObject.Find("HeldItem");
            currentHeld = 1;
            foreach (Transform child in heldItem.transform) {
                if (child.gameObject.CompareTag("Held")) {
                    child.gameObject.GetComponent<Text>().text = GetItemName(currentHeld);
                }
            }
        }

        public InventoryItem[] GetItems() {
            return items;
        }

        public void AddItem(InventoryItem item) {
            if (IsSpace()) {
                InventoryItem slotItem = items[GetSlot(item)];
                if (slotItem != null) {
                    items[GetSlot(item)].SetQuantity(slotItem.GetQuantity() + 1);
                } else {
                    item.SetQuantity(1);
                    items[GetSlot(item)] = item;
                }
            } else {
                throw new System.Exception("No space in inventory.");
            }
        }

        public void AddSlot(InventorySlotController slot) {
            itemSlots.Add(slot);
        }

        public int CollectItem(Interactable focus, GameObject pickup) {

            InventoryItem newItem = ScriptableObject.CreateInstance<InventoryItem>();
            newItem.SetItemType(focus.GetItemType());

            int nextSlot = GetSlot(newItem);
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

        void Update() {
            if (Input.GetKeyDown(KeyCode.Equals)) {
                SwitchHeld(1);
            } else if (Input.GetKeyDown(KeyCode.Minus)) {
                SwitchHeld(0);
            }
        }

        void SwitchHeld(int i) {
            foreach (Transform child in heldItem.transform) {
                if (child.gameObject.name == "held") {
                    if (i > 0) {
                        if (currentHeld == 16) currentHeld = 0;
                        child.gameObject.GetComponent<Text>().text = GetItemName(++currentHeld);
                    }
                    else {
                        if (currentHeld == 1) currentHeld = 17;
                        child.gameObject.GetComponent<Text>().text = GetItemName(--currentHeld);
                    }
                }
            }
        }

        public int GetCurrentHeld() {
            return currentHeld;
        }

        public string GetItemName(int id) {
            GameObjectsHandler goh = GameObjectsHandler.WithRemoteSchema();
            return goh.GameObjs.items[id - 1].name;
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

        public int GetSlot(InventoryItem item) {
            int firstNull = size + 1;
            for (int i = 0; i < size; i++) {
                if (items[i] == null) {
                    if (i < firstNull) {
                        firstNull = i;
                    }
                } else if (items[i].GetId() == item.GetId()) {
                    return i;
                }
            }
            return firstNull;
        }

        public List<InventorySlotController> GetUISlots() {
            return itemSlots;
        }

        private bool IsSpace() {
            return items.Count(s => s != null) <= size;
        }

        public bool Equals(Object obj) {
            bool eq = false;
            if (obj.GetType() == typeof(InventoryController)) {
                InventoryController other = (InventoryController) obj;
                eq = other.GetItems().OrderBy(t => t).SequenceEqual(items.OrderBy(t => t));
            }

            return eq;
        }
    }
}
