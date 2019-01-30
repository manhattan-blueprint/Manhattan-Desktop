using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Model;
using Model.Action;
using Model.Redux;
using Model.State;
using Utils;
using View;

/* Attached to the player and controls inventory collection */
namespace Controller {
    public class InventoryController : MonoBehaviour, Subscriber<GameState> {
        [SerializeField] private GameObject itemLabel;
        private InventoryItem[] inventoryContents;
        private List<InventorySlotController> itemSlots;
        private const int size = 16;

        public void Start() {
            inventoryContents = new InventoryItem[size];
            itemSlots = GameObject.Find("GridPanel").GetComponentsInChildren<InventorySlotController>().ToList();
            GameManager.Instance().store.Subscribe(this);
        }

        public InventoryItem[] GetItems() {
            return inventoryContents;
        }

        public void StateDidUpdate(GameState state) {
            inventoryContents = state.inventoryState.inventoryContents;
            
            // Update UI based on new state
            inventoryContents.Where(x => x != null).Each((element, i) => {
                if (itemSlots[i].transform.childCount < 2) {
                    GameObject label = Instantiate(itemLabel, itemSlots[i].transform, false);
                    label.name = getSlotName(i);
                    label.GetComponent<Text>().text = element.GetName();
                } else if (element.GetQuantity() > 0){
                    GameObject.Find(getSlotName(i)).GetComponentInChildren<Text>().text =
                        $"{element.GetName()} ({element.GetQuantity()})";
                } else {
                    GameObject.Find(getSlotName(i)).GetComponentInChildren<Text>().text = "";
                }
                
                // Change load order or UI elements for accessible hit-box
                GameObject dropButton = GameObject.Find(getButtonName(i));
                itemLabel.transform.SetSiblingIndex(0);
                dropButton.transform.SetSiblingIndex(1);
                    
            });
        }

        public void CollectItem(Interactable focus, GameObject pickup) {
            // TODO: This destroy should be the role of the map state
            Destroy(pickup);
            GameManager.Instance().store.Dispatch(
                new AddItemToInventory(focus.GetId(), 1, focus.GetItemType()));
        }
        
        private string getSlotName(int id) {
            return "InventoryItemSlot " + id;
        }

        private string getButtonName(int id) {
            return "Button" + (id + 1);
        }
    }
}
