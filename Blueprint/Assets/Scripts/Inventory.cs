using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Serialization;

public class Inventory : MonoBehaviour {

	[SerializeField] private List<InventoryItem> _items;

	public Inventory(List<InventoryItem> items) {
		this._items = items;
	}

	public List<InventoryItem> GetItems() {
		return _items;
	}

	public bool Equals(Object obj) {
		bool eq = false;
		if (obj.GetType () == typeof(Inventory)) {
			Inventory other = (Inventory) obj;
			//eq = other.GetItems ().Equals(_items);
			eq = other.GetItems().OrderBy(t => t).SequenceEqual(_items.OrderBy(t => t));
		}

		return eq;
	}
	

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
