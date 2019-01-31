using System.Collections;
using System.Collections.Generic;
using Model;
using Model.Redux;
using Model.State;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;
using View;

namespace Controller {
    public class MachineryUIController : MonoBehaviour, Subscriber<GameState> {
        private Canvas machineryCanvas;
        private Canvas cursorCanvas;
        private RaycastHit hit;
        private InventoryItem[] inventoryItems;
        //TODO: Only hard coded for demo, better solution needed
        private int inventorySize = 16;
        
        void Start() {
            machineryCanvas = GetComponent<Canvas>();
            machineryCanvas.enabled = false;
            cursorCanvas = GameObject.FindGameObjectWithTag("Cursor").GetComponent<Canvas>();
            GameManager.Instance().store.Subscribe(this);
        }

        void Update() {
            if (Input.GetKeyDown(KeyMapping.Machinery)) {
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
                hit = new RaycastHit();

                // If a GameObject is hit
                if (!Physics.Raycast(ray, out hit)) return;
                if (hit.collider.GetComponent<Interactable>().GetItemType() == "Furnace") {
                    if (machineryCanvas.enabled) {
                        ContinueGame();
                    } else {
                        PauseGame();
                        LoadInventory();
                    }
                }
            }
        }

        public void StateDidUpdate(GameState state) {
            inventoryItems = state.inventoryState.inventoryContents;
            LoadInventory();
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
        
        // TODO: Implement quantity
        private void LoadInventory() {
            for (int i = 0; i < inventorySize; i++) {
                Text text = GameObject.Find("Text" + i).GetComponent<Text>();
                InventoryItem item = inventoryItems[i];
                
                Font ArialFont = (Font) Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
                text.font = ArialFont;
                text.material = ArialFont.material;
                text.transform.localPosition = new Vector3(0,0,0);
                text.color = Color.black;
                text.alignment = TextAnchor.MiddleCenter;

                if (item != null && (item.GetQuantity() > 0)) {                   
                    text.text = item.GetName();
                } else {
                    text.text = "";
                }
            }
        }
    }
}
