using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Action;
using Model.Redux;
using Model.State;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Controller {
    //public class BlueprintController : MonoBehaviour, Subscriber<GameState> {
    public class BlueprintController : MonoBehaviour {

        private Button blueprintButton;
        private Image checkImage;

        void Start() {
            // TODO: Change finding game objects by name
            blueprintButton = GameObject.Find("BlueprintButton").GetComponent<Button>();
            blueprintButton.onClick.AddListener(OnButtonClick);
            checkImage = GameObject.Find("CheckImage").GetComponent<Image>();
            //GameManager.Instance().store.Subscribe(this);
        }

        //public void StateDidUpdate(GameState state) {
        //    InventoryItem[] inventoryContents = state.inventoryState.inventoryContents;
        //    
        //    // TODO: Replace all of this after the demo, hardcoded to make furnaces
        //    checkImage.sprite = Resources.Load<Sprite>("Cross");
        //    blueprintButton.interactable = false;
        //    
        //    inventoryContents.Where(x => x != null).Each((element, i) => {
        //        if (element.GetId() == 2 && element.GetQuantity() >= 8) {
        //            checkImage.sprite = Resources.Load<Sprite>("Tick");
        //            blueprintButton.interactable = true;
        //        }
        //    });
        //}

        public void OnButtonClick() {
            GameManager.Instance().inventoryStore.Dispatch(new AddItemToInventory(7, 1, "furnace"));
            GameManager.Instance().inventoryStore.Dispatch((new RemoveItemFromInventory(2, 8)));
        }
    }
}
