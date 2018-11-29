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
	public int size = 9;

	// public Inventory(InventoryItem[] items) {
	// 	this.items = items;
	// }

	public InventoryItem[] GetItems() {
		return items;
	}

	public void AddItem(InventoryItem item) {
		if (isSpace()){
			Debug.Log("Added item to inventory: " + item.name);
			this.items[this.GetNextFreeSlot()] = item;
		} else {
			Debug.Log("No space in inventory");
		}
	}

	public int GetNextFreeSlot() {
		for (int i=0; i<this.size; i++) {
			if (items[i] == null) {
				Debug.Log(i);
				return i;
			}
		}
		Debug.Log("-1");
		return -1;
	
	}

	public bool isSpace(){
		if (items.Length > 9) return false;
		return true;
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
