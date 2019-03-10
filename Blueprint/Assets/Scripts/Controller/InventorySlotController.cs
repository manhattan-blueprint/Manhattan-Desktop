using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Model;
using Model.Action;
using Model.Reducer;
using Model.Redux;
using Model.State;
using UnityEditor;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.UIElements;
using Image = UnityEngine.UI.Image;

/* Attached to each slot in the inventory grid */
namespace Controller {
    public class InventorySlotController : MonoBehaviour, IDropHandler {
        private int id;
        private bool mouseOver = false;
        private InventoryItem nullItem = new InventoryItem("", 0, 0);
        private InventoryItem storedItem;
        private GameObject highlightObject;
        private float slotHeight;
        private float slotWidth;
        private GameManager gameManager;
        private AssetManager assetManager;
        private float mouseEntryTime;
        private GameObject rolloverObject;
        private Vector3 rolloverPosition;
        private bool rolloverState;
        
        // EDITABLE
        // Time before rollover text shows (secs)
        private float rolloverTime = 2.0f;

        private void Start() {
            highlightObject = GameObject.Find("Highlight");
            rolloverObject = GameObject.Find("Rollover");
            slotHeight = (transform as RectTransform).rect.height;
            slotWidth = (transform as RectTransform).rect.width;
            storedItem = nullItem;
            gameManager = GameManager.Instance();
            assetManager = AssetManager.Instance();
                
            // Item image and quantity
            GameObject newGO = new GameObject("Icon" + id);
            newGO.transform.SetParent(gameObject.transform);
            newGO.AddComponent<InventorySlotDragHandler>();

            setupImage(newGO, nullItem);
            setupText(this.gameObject, nullItem.GetQuantity().ToString());
            
            // Initialise rollover object
            rolloverObject.GetComponentInChildren<Text>().font = assetManager.FontHelveticaNeueBold;
            rolloverObject.GetComponentInChildren<Text>().alignment = TextAnchor.MiddleCenter;
        }

        private void Update() {
            RectTransform hex = transform as RectTransform;
            SVGImage highlight = highlightObject.GetComponent<SVGImage>();

            if (RectTransformUtility.RectangleContainsScreenPoint(hex, Input.mousePosition)) {
                if (!mouseOver) {
                    // Mouse entry
                    mouseOver = true;
                    mouseEntryTime = Time.realtimeSinceStartup;
                    setHighlightLocation(transform.position.x, transform.position.y);
                } else {
                    // Rollover 
                    if ((Time.realtimeSinceStartup - rolloverTime) > mouseEntryTime && storedItem != nullItem) {
                        if (!rolloverState) {
                            rolloverState = true;
                            rolloverObject.SetActive(true);
                            rolloverPosition = Input.mousePosition;
                            setRolloverLocation(Input.mousePosition.x, Input.mousePosition.y + slotHeight/6, storedItem.GetName());
                        } else {
                            if (Input.mousePosition != rolloverPosition) {
                                rolloverObject.SetActive(false);
                            }
                        }
                    }
                }
            }

            if (!RectTransformUtility.RectangleContainsScreenPoint(hex, Input.mousePosition) && mouseOver) {
                // Mouse exit 
                mouseOver = false;
                rolloverState = false;
                rolloverObject.SetActive(false);
            }
        }

        private void setRolloverLocation(float x, float y, string inputText) {
            rolloverObject.transform.position = new Vector2(x, y);
            Text text = rolloverObject.GetComponentInChildren<Text>();
            text.text = inputText;
        } 

        private void setHighlightLocation(float x, float y) {
            highlightObject.transform.position = new Vector2(x, y); 
        }

        public void setId(int id) {
            this.id = id;
        }

        public void SetStoredItem(InventoryItem item) {
            this.storedItem = item;
            updateFields(storedItem);
        }

        public InventoryItem GetStoredItem() {
            return storedItem;
        }

        private Text setupText(GameObject obj, string initialText) {
            GameObject textObj = new GameObject("Text" + id);
            textObj.transform.parent = this.transform;
            Text text = textObj.AddComponent<Text>();

            text.font = assetManager.FontHelveticaNeueBold;
            text.transform.localPosition = new Vector3(0, -slotHeight/6, 0);
            text.color = assetManager.ColourOffWhite;
            text.alignment = TextAnchor.MiddleCenter; 
            text.text = initialText;
            text.raycastTarget = false;
            text.fontSize = assetManager.QuantityFieldFontSize;

            if (initialText == "0") text.enabled = false;

            return text;
        }

        private Image setupImage(GameObject obj,  InventoryItem item) {
            Image image = obj.AddComponent<Image>();
            image.transform.localPosition = new Vector3(0, slotHeight/8, 0);

            if (item.GetId() != nullItem.GetId()){
                Sprite icon = assetManager.GetItemSprite(item.GetId());
                image.sprite = icon;
                image.enabled = true;
            } else {
                image.enabled = false;
            }
            
            image.rectTransform.sizeDelta = new Vector2(slotWidth/3, slotHeight/3);

            return image;
        }

        private void updateFields(InventoryItem item) {
            Image image = GameObject.Find("Icon" + id).GetComponentInChildren<Image>();
            Text text = transform.GetComponentInChildren<Text>();

            if (item.GetId() != nullItem.GetId()) {
                image.sprite = assetManager.GetItemSprite(item.GetId());
                text.text = item.GetQuantity().ToString();
                    
                image.enabled = true;
                text.enabled = true;
                image.transform.localPosition = new Vector3(0, slotHeight/8, 0);
            } else {
                image.enabled = false;
                text.enabled = false;
            }
        }
    
        public void OnDrop(PointerEventData eventData) {
            RectTransform invPanel = transform as RectTransform;
            GameObject droppedObject = eventData.pointerDrag;
            
            InventorySlotController source = GameObject.Find(droppedObject.transform.name)
                .GetComponentInParent<InventorySlotController>(); 
            InventorySlotController destination =
                GameObject.Find(transform.name).GetComponentInParent<InventorySlotController>();

            if (RectTransformUtility.RectangleContainsScreenPoint(invPanel, Input.mousePosition)) {
                if (destination.GetStoredItem().GetId() != nullItem.GetId()) {
                    // Move to occupied slot
                    InventoryItem temp = destination.GetStoredItem();
                    
                    destination.SetStoredItem(source.GetStoredItem());
                    source.SetStoredItem(temp);
                } else {
                    // Move to empty slot
                    destination.SetStoredItem(source.GetStoredItem());
                    source.SetStoredItem(nullItem);
                }

                this.gameManager.store.Dispatch(new SwapItemLocations(source.id, destination.id,
                    destination.GetStoredItem().GetId(), source.GetStoredItem().GetId()));
            }
        } 
    }
}    
