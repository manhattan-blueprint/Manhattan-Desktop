using System;

[Serializable]
public class InventoryEntry {
	public int item_id;
	public int quantity;

	public InventoryEntry(int itemId, int quantity) {
		this.item_id = itemId;
		this.quantity = quantity;
	}
}