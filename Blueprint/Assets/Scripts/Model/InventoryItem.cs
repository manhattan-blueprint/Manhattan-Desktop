using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model {
    [Serializable]
    public class InventoryItem {
        [SerializeField] private string name;
        [SerializeField] private int id;
        [SerializeField] private int quantity;
    
        public InventoryItem(string name, int id, int quantity) {
            this.name = name;
            this.id = id;
            this.quantity = quantity;
        }

        public string GetName() {
            return name;
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