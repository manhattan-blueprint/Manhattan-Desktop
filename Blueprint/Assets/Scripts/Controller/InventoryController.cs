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
        [SerializeField] InventoryItem[] items;
        [SerializeField] List<InventorySlotController> itemSlots;
        private int size;
        [SerializeField] GameObject itemButton;
        private GameObject dropButton;

        public void Start() {
            Size = 16;
            items = new InventoryItem[Size];
            itemSlots = GameObject.Find("GridPanel").GetComponentsInChildren<InventorySlotController>().ToList();
        }

        public InventoryItem[] GetItems() {
            return items;
        }

        public void AddItem(InventoryItem item) {
            if (IsSpace()) {
                InventoryItem slotItem = items[GetNextFreeSlot(item)];
                if (slotItem != null) {
                    items[GetNextFreeSlot(item)].SetQuantity(slotItem.GetQuantity() + 1);
                }
                else {
                    item.SetQuantity(1);
                    items[GetNextFreeSlot(item)] = item;
                }
            }
            else {
                Debug.Log("No space in inventory");
            }
        }

        public void AddSlot(InventorySlotController slot) {
            itemSlots.Add(slot);
        }

        public int CollectItem(Interactable focus, GameObject pickup) {

            InventoryItem newItem = ScriptableObject.CreateInstance<InventoryItem>();
            newItem.SetItemType(focus.GetItemType());

            int nextSlot = GetNextFreeSlot(newItem);
            // Add to inventory object
            AddItem(newItem);

            // Set to make access unique
            itemButton.name = GetNameForSlot(nextSlot);

            // Make game world object invisible and collider inactive
            Destroy(pickup);
            // Create a slot with text in inventory window, or update quantity bracket
            if (GetUISlots()[nextSlot].transform.childCount < 2) {
                Instantiate(itemButton, GetUISlots()[nextSlot].transform, false);
                GameObject.Find("InventoryItemSlot " + nextSlot + "(Clone)").GetComponent<Text>().text =
                    focus.GetItemType();
            }
            else {
                GameObject.Find("InventoryItemSlot " + nextSlot + "(Clone)").GetComponentInChildren<Text>().text =
                    focus.GetItemType() + " (" + GetItems()[nextSlot].GetQuantity() + ")";
            }

            // Change load order or UI elements for accessible hit-box
            string index = GetNameForButton(nextSlot);
            dropButton = GameObject.Find(index);
            itemButton.transform.SetSiblingIndex(0);
            dropButton.transform.SetSiblingIndex(1);

            return nextSlot;
        }

        public string GetNameForSlot(int id) {
            return "InventoryItemSlot " + id;
        }

        public string GetNameForButton(int id) {
            return "Button" + (id + 1);
            ;
        }

        public int GetNextFreeSlot(InventoryItem item) {
            int firstNull = Size + 1;
            for (int i = 0; i < Size; i++) {
                if (items[i] == null) {
                    if (i < firstNull) {
                        firstNull = i;
                    }
                }
                else if (items[i].GetId() == item.GetId()) {
                    return i;
                }
            }

            return firstNull;
        }

        public List<InventorySlotController> GetUISlots() {
            return itemSlots;
        }

        private bool IsSpace() {
            return items.Count(s => s != null) <= Size;
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
