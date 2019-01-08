using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

    // Seperate data from UI
    [SerializeField] InventoryItem[] items;
    [SerializeField] Transform itemsParent;
    [SerializeField] List<ItemSlot> itemSlots;
    [SerializeField] int Size = 9;
    [SerializeField] GameObject itemButton;
    private GameObject dropButton;

    public void Start() {
        items = new InventoryItem[Size];
    }

    public InventoryItem[] GetItems() {
        return items;
    }

    public void AddItem(InventoryItem item) {
        if (IsSpace()) {
            InventoryItem slotItem = this.items[this.GetNextFreeSlot(item)];
            if (slotItem != null) {
                this.items[this.GetNextFreeSlot(item)].SetQuantity(slotItem.GetQuantity()+1);
            } else {
                item.SetQuantity(1);
                this.items[this.GetNextFreeSlot(item)] = item;
            }
        } else {
            Debug.Log("No space in inventory");
        }
    }
    
    public int CollectItem(Interactable focus, GameObject pickup) {
        
        InventoryItem newItem = new InventoryItem(focus.GetId(), focus.GetItemType(), 1);

        int nextSlot = GetNextFreeSlot(newItem);

        // Add to inventory object
        AddItem(newItem);

        // Set to make access unique
        itemButton.name = GetNameForSlot(nextSlot);
        itemButton.GetComponent<Text>().text = newItem.GetItemType();
        
        // Make game world object invisible and collider inactive
        Destroy(pickup);
        
        // Create a slot with text in inventory window, or update quantity bracket
        if (GetUISlots()[nextSlot].transform.childCount < 2) {
            Instantiate(itemButton, GetUISlots()[nextSlot].transform, false);
        }
        else {
            GameObject.Find("InventoryItemSlot " + nextSlot + "(Clone)").GetComponentInChildren<Text>().text =
                newItem.GetItemType() + " (" + GetItems()[nextSlot].GetQuantity() + ")";
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
        return "Button" + (id + 1);;
    }

    public int GetNextFreeSlot(InventoryItem item) {
        int firstNull = this.Size+1;
        for (int i=0; i < this.Size; i++) {
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

    public List<ItemSlot> GetUISlots() {
        return this.itemSlots;
    }

    private bool IsSpace(){
        return items.Length <= 9;
    }

    public bool Equals(Object obj) {
        bool eq = false;
        if (obj.GetType () == typeof(Inventory)) {
            Inventory other = (Inventory) obj;
            eq = other.GetItems().OrderBy(t => t).SequenceEqual(items.OrderBy(t => t));
        }
        return eq;
    }
}
