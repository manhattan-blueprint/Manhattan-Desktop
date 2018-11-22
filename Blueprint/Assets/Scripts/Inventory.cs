using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Inventory : MonoBehaviour {

	// Seperate data from UI
	[SerializeField] List<InventoryItem> items;
	[SerializeField] Transform itemsParent;
	public ItemSlot[] itemSlots;


	// public Inventory(List<InventoryItem> items) {
	// 	this.items = items;
	// }

	public List<InventoryItem> GetItems() {
		return items;
	}
	
	private void OnValidate() {
		if (itemsParent != null) {
			itemSlots = itemsParent.GetComponentsInChildren<ItemSlot>();
		}
		RefreshUI();
	}

	public void AddItem(InventoryItem item) {
		if (isSpace()){
			Debug.Log("Added item yo");
			this.items.Add(item);
		} else {
			Debug.Log("No space bro");
		}
	}

	public bool isSpace(){
		if (items.Count > 9) return false;
		return true;
	}
	
	private void RefreshUI() {
		int i = 0;
		for (; i < items.Count && i < itemSlots.Length; i++) {
			itemSlots[i].item = items[i];
		}

		for (; i < itemSlots.Length; i++) {
			itemSlots[i].item = null;
		}
	}

		public bool Equals(Object obj) {
		bool eq = false;
		if (obj.GetType () == typeof(Inventory)) {
			Inventory other = (Inventory) obj;
			//eq = other.GetItems ().Equals(_items);
			eq = other.GetItems().OrderBy(t => t).SequenceEqual(items.OrderBy(t => t));
		}

		return eq;
	}
}
