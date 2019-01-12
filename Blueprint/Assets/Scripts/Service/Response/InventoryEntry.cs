using System;

namespace Service.Response {
	[Serializable]
	public class InventoryEntry {
		public int itemId;
		public int quantity;

		public InventoryEntry(int itemId, int quantity) {
			this.itemId = itemId;
			this.quantity = quantity;
		}
	}
}