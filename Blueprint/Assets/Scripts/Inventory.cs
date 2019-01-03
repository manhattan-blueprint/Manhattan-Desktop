using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Inventory : MonoBehaviour {

    // Seperate data from UI
    [SerializeField] InventoryItem[] items;
    [SerializeField] Transform itemsParent;
    public List<ItemSlot> itemSlots;
    public int Size = 9;

    public InventoryItem[] GetItems() {
        return items;
    }

    public void AddItem(InventoryItem item) {
        if (IsSpace()) {
            InventoryItem slotItem = this.items[this.GetNextFreeSlot(item)];
            if (slotItem != null) {
                Debug.Log("Quantity before: " + this.items[this.GetNextFreeSlot(item)].GetQuantity());
                this.items[this.GetNextFreeSlot(item)].SetQuantity(slotItem.GetQuantity()+1);
                Debug.Log("Quantity after: " + this.items[this.GetNextFreeSlot(item)].GetQuantity());
            } else {
                item.SetQuantity(1);
                this.items[this.GetNextFreeSlot(item)] = item;
            }
        } else {
            Debug.Log("No space in inventory");
        }
    }

    public int GetNextFreeSlot(InventoryItem item) {
        int firstNull = this.Size+1;
        for (int i=0; i < this.Size; i++) {
            if (items[i] == null) {
                if (i < firstNull) {
                    firstNull = i;
                }
            } else if (items[i].id == item.id) {
                return i;
            }
        }
        return firstNull;
    
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
