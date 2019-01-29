using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model {
    public class InventoryItem {

        private int id;
        private int quantity;
    
        public InventoryItem(int id, int quantity) {
            this.id = id;
            this.quantity = quantity;
        }
    
        public int GetId() {
            return this.id;
        }
    
        public int GetQuantity() {
            return quantity;
        }
        
        public void AddQuantity(int quantity) {
            this.quantity += quantity;
        }

        public void SetQuantity(int quantity) {
            this.quantity = quantity;
        }
    }
}