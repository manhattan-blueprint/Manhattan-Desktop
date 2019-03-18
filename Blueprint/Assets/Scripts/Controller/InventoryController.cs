using System.Collections.Generic;
using System.IO;
using System.Net;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Model;
using Model.Action;
using Model.Redux;
using Model.State;
using Utils;
using View;
using Service;
using Service.Response;
using System.Threading.Tasks;
using UnityEngine.Assertions.Must;
using UnityEngine.Experimental.Rendering;

/* Attached to the player and controls inventory collection */
namespace Controller {
    public class InventoryController : MonoBehaviour, Subscriber<InventoryState> {
        public Dictionary<int, List<HexLocation>> inventoryContents;
        private Dictionary<int, InventorySlotController> itemSlots;
        private GameManager gameManager;
        private bool firstUIUpdate;

        public void Start() {
            firstUIUpdate = true;
            
            // Generate dictionary of item slots for fast lookup
            itemSlots = new Dictionary<int, InventorySlotController>();
            List<InventorySlotController> allSlots = GameObject.Find("InventoryUICanvas").GetComponentsInChildren<InventorySlotController>().ToList();
            foreach (InventorySlotController controller in allSlots) {
              itemSlots.Add(controller.getId(), controller);
            }
            
            GameManager.Instance().inventoryStore.Subscribe(this);
        }
        
        void Update() {
            if (firstUIUpdate) {
                StateDidUpdate(GameManager.Instance().inventoryStore.GetState());
                firstUIUpdate = false;
            }
        }
        
        public void StateDidUpdate(InventoryState state) {
            inventoryContents = state.inventoryContents;
            redrawInventory();
        }

        public string GetItemName(int id) {
            return GameManager.Instance().goh.GameObjs.items[id - 1].name;
        }
        
        public int GetItemType(int id) {
            return GameManager.Instance().goh.GameObjs.items[id - 1].type;
        }

        public void redrawInventory() {
            // Clear slots
            foreach (KeyValuePair<int, InventorySlotController> slot in itemSlots) {
                slot.Value.SetStoredItem(Optional<InventoryItem>.Empty());
            }
            
            // Re-populate slots
            foreach (KeyValuePair<int, List<HexLocation>> element in inventoryContents) {
                foreach(HexLocation loc in element.Value) {
                    InventoryItem item = new InventoryItem(GetItemName(element.Key), element.Key, loc.quantity);
                    itemSlots[loc.hexID].SetStoredItem(Optional<InventoryItem>.Of(item));
                } 
            }
        }
    }
}
