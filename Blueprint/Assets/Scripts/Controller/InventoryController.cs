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
using UnityEngine.Experimental.UIElements;

/* Attached to the player and controls inventory collection */
namespace Controller {
    public class InventoryController : MonoBehaviour, Subscriber<GameState> {
        private List<InventorySlotController> itemSlots;
        private ResponseGetInventory remoteInv;

        public void Start() {
            itemSlots = GameObject.Find("GridPanel").GetComponentsInChildren<InventorySlotController>().ToList();
            UserCredentials user = GameManager.Instance().GetUserCredentials();
            GameManager.Instance().store.Subscribe(this);
            BlueprintAPI blueprintApi = BlueprintAPI.DefaultCredentials();

            Task.Run(async () => {
                APIResult<ResponseGetInventory, JsonError> finalInventoryResponse = await blueprintApi.AsyncGetInventory(user);
                if (finalInventoryResponse.isSuccess()) {
                    remoteInv = finalInventoryResponse.GetSuccess();
                } else {
                    JsonError error = finalInventoryResponse.GetError();
                }
            }).GetAwaiter().GetResult();
            
            foreach (InventoryEntry entry in remoteInv.items) {
                GameManager.Instance().store.Dispatch(
                    new AddItemToInventory(entry.item_id, entry.quantity, GetItemName(entry.item_id)));
            }
            /*            
                        Task.Run(async () => {
                            try {
                                APIResult<Boolean, JsonError> response = await blueprintApi.AsyncDeleteInventory(user);

                                // Success case
                            } catch (WebException e) {
                                // Failure case
                                throw new System.Exception("Did not delete inventory.");
                            }    
                        }).GetAwaiter().GetResult();
            */
        }
        
        public void StateDidUpdate(GameState state) {}

        public string GetItemName(int id) {
            GameObjectsHandler goh = GameObjectsHandler.WithRemoteSchema();
            return goh.GameObjs.items[id - 1].name;
        }
        
        public int GetItemType(int id) {
            GameObjectsHandler goh = GameObjectsHandler.WithRemoteSchema();
            return goh.GameObjs.items[id - 1].type;
        }
    }
}
