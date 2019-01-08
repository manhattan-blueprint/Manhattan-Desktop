using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Inventory : MonoBehaviour {

    // Seperate data from UI
    [SerializeField] InventoryItem[] items;
    [SerializeField] Transform itemsParent;
    [SerializeField] List<ItemSlot> itemSlots;
    [SerializeField] int Size = 9;

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
    
    public string GetNameForSlot(int id) {
        return "InventoryItemSlot" + id;
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
