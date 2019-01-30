using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Model;
using UnityEditor;
using UnityEngine.Experimental.Rendering;
using View;

namespace Controller {
//    public class MachineryController : MonoBehaviour, Subscriber<> {
    public class MachineryController : MonoBehaviour {
        private Canvas machineryCanvas;
        private RaycastHit hit;
        private InventoryController inventory;
        private const int leftButton = 0;
        private const int rightButton = 1;
        private MenuController menu;
        private Canvas cursorCanvas;
        private List<InventorySlotController> itemSlots;
        private List<OnDropHandler> inputSlots;
        public List<InventoryItem> inputs;
        private InventorySlotController outputSlot;
            
        // Start is called before the first frame update
        void Start() {
            machineryCanvas = GetComponent<Canvas>();
            machineryCanvas.enabled = false;
            inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryController>();
            cursorCanvas = GameObject.FindGameObjectWithTag("Cursor").GetComponent<Canvas>();
            itemSlots = GameObject.Find("InventoryGrid").GetComponentsInChildren<InventorySlotController>().ToList();
            inputSlots = GameObject.Find("MachineryInputGrid").GetComponentsInChildren<OnDropHandler>().ToList();
            outputSlot = GameObject.Find("MachineryOutput").GetComponent<InventorySlotController>();
           
            //add text fields
            for (int i = 0; i < inventory.GetInventorySize(); i++) {
                GameObject newGO = new GameObject("Text"+i);
                newGO.transform.SetParent(itemSlots[i].transform);
                Text text = newGO.AddComponent<Text>();
                newGO.AddComponent<OnDragHandler>();
            }

            for (int i = 0; i < 2; i++) {
                GameObject newGO1 = new GameObject("Text"+i);
                newGO1.transform.SetParent(inputSlots[i].transform);
                Text text1 = newGO1.AddComponent<Text>();
                newGO1.AddComponent<OnDragHandler>();
            }
            
            GameObject newGO2 = new GameObject("Text0");
            newGO2.transform.SetParent(outputSlot.transform);
            Text text2 = newGO2.AddComponent<Text>();
            newGO2.AddComponent<OnDragHandler>();
        }

        // Update is called once per frame
        void Update() {
            if (Input.GetKeyDown(KeyCode.Q)) {
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
                hit = new RaycastHit();

                // If a GameObject is hit
                if (!Physics.Raycast(ray, out hit)) return;
                if (hit.collider.GetComponent<Interactable>().GetItemType() == "Machinery") {
                    if (machineryCanvas.enabled) {
                        ContinueGame();
                    } else {
                        PauseGame();
                        LoadInventory();
                    }
                }
            }
        }
        
        private void PauseGame() {
            Time.timeScale = 0;
            machineryCanvas.enabled = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            cursorCanvas.enabled = false;
        }

        private void ContinueGame() {
            Time.timeScale = 1;
            machineryCanvas.enabled = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            cursorCanvas.enabled = true;
        }

        private void LoadInventory() {
            for (int i = 0; i < inventory.GetInventorySize(); i++) {
                Text text = GameObject.Find("Text" + i).GetComponent<Text>();
                InventoryItem item = inventory.GetItems()[i];
                
                Font ArialFont = (Font) Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
                text.font = ArialFont;
                text.material = ArialFont.material;
                text.transform.localPosition = new Vector3(0,0,0);
                text.color = Color.black;
                text.alignment = TextAnchor.MiddleCenter;

                if (item != null) {
                    text.text = item.GetItemType();
                } else {
                    text.text = "";
                }
            }
        }

        private void checkOutput() {
            GameObjectsHandler goh = GameObjectsHandler.WithRemoteSchema();
            List<RecipeElement> inputItems = null;

            foreach (InventoryItem item in inputs) {
                inputItems.Add(new RecipeElement(item.GetId(), item.GetQuantity()));
            }
            
            GameObjectEntry goe = goh.GetRecipe(inputItems, 7);
        }
    }  

}

