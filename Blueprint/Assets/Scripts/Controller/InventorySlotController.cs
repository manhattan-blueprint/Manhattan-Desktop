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
    public class InventorySlotController : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {
        private int id;
        private bool mouseOver;
        private Optional<InventoryItem> storedItem;
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
        private float rolloverTime = 1.0f;

        private void Start() {
            highlightObject = GameObject.Find("Highlight");
            rolloverObject = GameObject.Find("Rollover");
            slotHeight = (transform as RectTransform).rect.height;
            slotWidth = (transform as RectTransform).rect.width;
            storedItem = Optional<InventoryItem>.Empty();
            gameManager = GameManager.Instance();
            assetManager = AssetManager.Instance();
                
            // Item image and quantity
            GameObject newGO = new GameObject("Icon" + id);
            newGO.transform.SetParent(gameObject.transform);
            newGO.AddComponent<InventorySlotDragHandler>();

            setupImage(newGO);
            setupText(this.gameObject);
            
            // Initialise rollover object
            rolloverObject.GetComponentInChildren<Text>().font = assetManager.FontHelveticaNeueBold;
            rolloverObject.GetComponentInChildren<Text>().alignment = TextAnchor.MiddleCenter;
        }
        
        public void OnPointerEnter(PointerEventData pointerEventData) {
            setHighlightLocation(transform.position.x, transform.position.y);
                
            if (!mouseOver) {
                // Mouse entry
                mouseOver = true;
                mouseEntryTime = Time.realtimeSinceStartup;
            } 
        }

        public void OnPointerExit(PointerEventData pointerEventData) {
            mouseOver = false;
            rolloverState = false;
            rolloverObject.SetActive(false);
            highlightObject.SetActive(false);
        }

        private void Update() {
            if (mouseOver && (Time.realtimeSinceStartup - rolloverTime) > mouseEntryTime && storedItem.IsPresent()) {
                 
                if (!rolloverState) {
                    rolloverState = true;
                    rolloverObject.SetActive(true);
                    rolloverPosition = Input.mousePosition;
                    setRolloverLocation(Input.mousePosition.x, Input.mousePosition.y + slotHeight/6, storedItem.Get().GetName());
                } else if (Input.mousePosition != rolloverPosition) {
                    rolloverObject.SetActive(false);
                    rolloverState = false;
                    mouseEntryTime = Time.realtimeSinceStartup;
                }
            } 
        }
        
        private void setRolloverLocation(float x, float y, string inputText) {
            rolloverObject.transform.position = new Vector2(x, y);
            Text text = rolloverObject.GetComponentInChildren<Text>();
            text.text = inputText;
            
            // Set box to width of word
            RectTransform rect = rolloverObject.transform as RectTransform;
            rect.sizeDelta = new Vector2(text.preferredWidth + slotWidth/8, slotHeight/5);            
        } 

        private void setHighlightLocation(float x, float y) {
            highlightObject.SetActive(true);
            highlightObject.transform.position = new Vector2(x, y);
            (highlightObject.transform as RectTransform).sizeDelta = (this.transform as RectTransform).sizeDelta;
        }

        public void setID(int id) {
            this.id = id;
        }

        public int getId() {
            return id;
        }

        public void SetStoredItem(Optional<InventoryItem> item) {
            this.storedItem = item;
            Image image = GameObject.Find("Icon" + id).GetComponentInChildren<Image>();
            Text text = transform.GetComponentInChildren<Text>();

            if (!this.storedItem.IsPresent()) {
                image.enabled = false;
                text.enabled = false;
            } else {
                image.sprite = assetManager.GetItemSprite(item.Get().GetId());
                text.text = item.Get().GetQuantity().ToString();
                    
                image.enabled = true;
                text.enabled = true;
                image.transform.localPosition = new Vector3(0, slotHeight/8, 0);
            }
        }

        private Text setupText(GameObject obj) {
            GameObject textObj = new GameObject("Text" + id);
            textObj.transform.parent = this.transform;
            Text text = textObj.AddComponent<Text>();

            text.font = assetManager.FontHelveticaNeueBold;
            text.transform.localPosition = new Vector3(0, -slotHeight/6, 0);
            text.color = assetManager.ColourOffWhite;
            text.alignment = TextAnchor.MiddleCenter; 
            text.text = "";
            text.raycastTarget = false;
            text.fontSize = assetManager.QuantityFieldFontSize;
            text.enabled = false;

            return text;
        }

        private Image setupImage(GameObject obj) {
            Image image = obj.AddComponent<Image>();
            image.transform.localPosition = new Vector3(0, slotHeight/8, 0);
            image.enabled = false;
            image.rectTransform.sizeDelta = new Vector2(slotWidth/3, slotHeight/3);
            return image;
        }

//        private void updateFields(InventoryItem item) {
//        }
    
        public void OnDrop(PointerEventData eventData) {
            RectTransform invPanel = transform as RectTransform;
            GameObject droppedObject = eventData.pointerDrag;
            
            InventorySlotController source = GameObject.Find(droppedObject.transform.name).GetComponentInParent<InventorySlotController>(); 
            InventorySlotController destination = GameObject.Find(transform.name).GetComponentInParent<InventorySlotController>();

            if (RectTransformUtility.RectangleContainsScreenPoint(invPanel, Input.mousePosition)) {
                if (destination.storedItem.IsPresent()) {
                    // Move to occupied slot
                    Optional<InventoryItem> temp = destination.storedItem;
                    
                    destination.SetStoredItem(source.storedItem);
                    source.SetStoredItem(temp);
                } else {
                    // Move to empty slot
                    destination.SetStoredItem(source.storedItem);
                    source.SetStoredItem(Optional<InventoryItem>.Empty());
                }

                this.gameManager.inventoryStore.Dispatch(new SwapItemLocations(source.id, destination.id,
                    destination.storedItem, source.storedItem));
            }
        } 
    }
}    
