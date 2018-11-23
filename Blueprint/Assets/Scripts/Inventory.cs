using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Inventory : MonoBehaviour {

	// Seperate data from UI
	[SerializeField] List<InventoryItem> items;
	[SerializeField] Transform itemsParent;
	public List<ItemSlot> itemSlots;

	public Inventory(List<InventoryItem> items) {
		this.items = items;
	}

	public List<InventoryItem> GetItems() {
		return items;
	}

	public void AddItem(InventoryItem item) {
		if (isSpace()){
			Debug.Log("Added item to inventory: " + item.name);
			this.items.Add(item);
		} else {
			Debug.Log("No space in inventory");
		}
	}

	public bool isSpace(){
		if (items.Count >= 9) return false;
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
