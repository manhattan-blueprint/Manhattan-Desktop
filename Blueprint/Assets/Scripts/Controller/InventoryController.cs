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
        [SerializeField] private GameObject itemButton;
        [SerializeField] private List<InventorySlotController> inventorySlots;
        private const int size = 9;
        private InventoryItem[] items;
        private Transform itemsParent;
        private GameObject dropButton;

        public void Start() {
            items = new InventoryItem[size];
        }

        public InventoryItem[] GetItems() {
            return items;
        }

        private void AddItem(InventoryItem item) {
            if (items.Length <= size) {
                InventoryItem slotItem = items[getNextFreeSlot(item)];
                if (slotItem != null) {
                    items[getNextFreeSlot(item)].quantity += 1;
                } else {
                    item.quantity = 1;
                    items[getNextFreeSlot(item)] = item;
                }
            } else {
                Debug.Log("No space in inventory");
            }
        }
        
        public int CollectItem(Interactable focus, GameObject pickup) {
            InventoryItem newItem = new InventoryItem(focus.GetId(), focus.GetItemType(), 1);
            int nextSlot = getNextFreeSlot(newItem);
            AddItem(newItem);

            // Set to make access unique
            itemButton.name = getNameForSlot(nextSlot);
            itemButton.GetComponent<Text>().text = newItem.GetItemType();
            
            // Make game world object invisible and collider inactive
            Destroy(pickup);
            
            // Create a slot with text in inventory window, or update quantity bracket
            if (inventorySlots[nextSlot].transform.childCount < 2) {
                Instantiate(itemButton, inventorySlots[nextSlot].transform, false);
            } else {
                GameObject.Find("InventoryItemSlot " + nextSlot + "(Clone)").GetComponentInChildren<Text>().text =
                    newItem.GetItemType() + " (" + GetItems()[nextSlot].quantity+ ")";
            }

            // Change load order or UI elements for accessible hit-box
            string index = getNameForButton(nextSlot);
            dropButton = GameObject.Find(index);
            itemButton.transform.SetSiblingIndex(0);
            dropButton.transform.SetSiblingIndex(1);

            return nextSlot;
        }

        private string getNameForSlot(int id) {
            return "InventoryItemSlot " + id;
        }

        private string getNameForButton(int id) {
            return "Button" + (id + 1);;
        }

        private int getNextFreeSlot(InventoryItem item) {
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

        public bool Equals(Object obj) {
            bool eq = false;
            if (obj.GetType () == typeof(InventoryController)) {
                InventoryController other = (InventoryController) obj;
                eq = other.GetItems().OrderBy(t => t).SequenceEqual(items.OrderBy(t => t));
            }
            return eq;
        }
    }
}