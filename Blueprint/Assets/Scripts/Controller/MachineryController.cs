using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Model;
using Model.Action;
using Model.Redux;
using Model.State;
using UnityEditor;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Rendering;
using View;

namespace Controller {
    public class MachineryController : MonoBehaviour { 
        private const int leftButton = 0;
        private const int rightButton = 1;
        private List<OnDragHandler> itemSlots;
        private List<OnDropHandler> inputSlots;
        private List<RecipeElement> inputs;
        private OnDragHandler outputSlot;
        //TODO: below are bodged for demo, need better solution
        private int inventorySize = 16;
        private bool beenChecked;
        private List<InventorySlotController> inventorySlotsControllers;
            
        void Start() {                     
            itemSlots = GameObject.Find("InventoryGrid").GetComponentsInChildren<OnDragHandler>().ToList();
            inventorySlotsControllers = GameObject.Find("InventoryGrid").GetComponentsInChildren<InventorySlotController>().ToList();
            inputSlots = GameObject.Find("MachineryInputGrid").GetComponentsInChildren<OnDropHandler>().ToList();
            //outputSlot = GameObject.Find("MachineryOutput").GetComponent<OnDragHandler>();
            inputs = new List<RecipeElement>();
            beenChecked = false;
           
            // TODO: Neaten the dynamic generation of text fields
            // Add text fields
            for (int i = 0; i < inventorySize; i++) {
                GameObject newGO = new GameObject("Text"+i);
                newGO.transform.SetParent(itemSlots[i].transform);
                Text text = newGO.AddComponent<Text>();
                newGO.AddComponent<OnDragHandler>();
                
                // TODO: InventorySlotController is overkill for machinery, design new type to combine with OnDragHandler
                // Set InventorySlotController IDs
                inventorySlotsControllers[i].SetId(i);
            }

            for (int i = 0; i < 2; i++) {
                GameObject newGO1 = new GameObject("Text"+i);
                newGO1.transform.SetParent(inputSlots[i].transform);
                Text text1 = newGO1.AddComponent<Text>();
                newGO1.AddComponent<OnDragHandler>();
            }
            
            /*GameObject newGO2 = new GameObject("Text0");
            newGO2.transform.SetParent(outputSlot.transform);
            Text text2 = newGO2.AddComponent<Text>();
            newGO2.AddComponent<OnDragHandler>();*/
        }

        void Update() {
            if (inputs.Count > 1 && !beenChecked) {
                beenChecked = true;
                GameObjectEntry goe = checkOutput();

                if (goe != null) {
                    clearInputFields();
                    // setOutputField(goe.name);
                    // TODO: Should go to the output slot, rather than directly into the inventory
                    GameManager.Instance().store.Dispatch(new AddItemToInventory(goe.item_id, 1, goe.name));

                    foreach (RecipeElement input in inputs) {
                        GameManager.Instance().store.Dispatch(new RemoveItemFromInventory(input.item_id, 1));
                    }
                    
                    inputs = new List<RecipeElement>();
                    beenChecked = false;
                } else {
                    clearInputFields();
                    inputs = new List<RecipeElement>();
                    beenChecked = false;
                }
            }
        }

        // TODO: Move to MachineryUIController when UI State is implemented (e.g. new FailureToCreateObject)
        private void clearInputFields() {
            List<Text> texts = GameObject.Find("MachineryInputGrid").GetComponentsInChildren<Text>().ToList();

            foreach (Text text in texts) {
                text.text = "";
            }
        }

        // TODO: Move to MachineryUIController when UI State is implemented (e.g. new NewObjectCreated)
        private void setOutputField(string name) {
            Text outputFieldText = GameObject.Find("MachineryOutput").GetComponentInChildren<Text>();

            outputFieldText.text = name;
        }

        public void AddInputItem(int itemID, int quantity) {
            inputs.Add(new RecipeElement(itemID, quantity));
        }

        private GameObjectEntry checkOutput() {
            GameObjectsHandler goh = GameObjectsHandler.WithRemoteSchema();
            
            // TODO: Globalise for all machine types
            return goh.GetRecipe(inputs, 7);
        }
    }  

}

