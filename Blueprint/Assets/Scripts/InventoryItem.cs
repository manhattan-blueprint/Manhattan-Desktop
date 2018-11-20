using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DefaultNamespace
{
    public class InventoryItem : MonoBehaviour
    {
        [SerializeField] private string _id;

        [SerializeField] private int _quantity;

        public InventoryItem(string id, int quantity)
        {
            this._id = id;
            this._quantity = quantity;
        }


        public int GetQuantity()
        {
            return _quantity;
        }

        public string GetId()
        {
            return _id;
        }

        public Boolean Equals(Object obj)
        {
            Boolean result = false;
            if (obj.GetType() == typeof(InventoryItem))
            {
                InventoryItem other = (InventoryItem) obj;
                result = this._id.Equals(other._id) && this._quantity == other._quantity;
            }
            return result;
        }

        // Use this for initialization
        void Start () {
		
        }
	
        // Update is called once per frame
        void Update () {
		
        }
    }
}