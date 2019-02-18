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
using UnityEditor.Experimental.UIElements;
using UnityEngine.EventSystems;

/* Attached to each slot in the inventory grid */
namespace Controller {
    public class InventorySlotController : MonoBehaviour, IDropHandler {
        private Sprite hexSprite;
        private Sprite hexSpriteHighlighted;
        private int id;
        private bool mouseOver = false;
        private Image image;
        private InventoryItem nullItem = new InventoryItem("", 0, 0);
        private InventoryItem storedItem;
        
        private void Start() {
            image = GameObject.Find(this.transform.name).GetComponent<Image>();
            var temp = image.color;
            temp.a = 0.5f;
            image.color = temp;
            
            hexSprite = Resources.Load("InventoryHexagon", typeof(Sprite)) as Sprite;
            hexSpriteHighlighted = Resources.Load("InventoryHexagonHighlighted", typeof(Sprite)) as Sprite;
            storedItem = nullItem;
            
            // Item image and quantity
            // TODO: change to image when items complete
            GameObject newGO = new GameObject("Icon"+id);
            newGO.transform.SetParent(gameObject.transform);
            newGO.AddComponent<InventorySlotDragHandler>();
            //newGO.AddComponent<InventorySlotDropHandler>();

            if (id == 0) {
                storedItem = new InventoryItem("wood", 1, 1);
                setupImage(newGO, storedItem.GetName());
            } 
            else {
                setupImage(newGO, "");
            }
            
        }

        private void Update() {
            RectTransform hex = transform as RectTransform;

            if (RectTransformUtility.RectangleContainsScreenPoint(hex, Input.mousePosition) && !mouseOver) {
                // Mouse entry
                mouseOver = true;
                image.sprite = hexSpriteHighlighted;
            }

            if (!RectTransformUtility.RectangleContainsScreenPoint(hex, Input.mousePosition) && mouseOver) {
                // Mouse exit 
                mouseOver = false;
                image.sprite = hexSprite;
            }
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
            Text text = obj.AddComponent<Text>();
            
            Font ArialFont = (Font) Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
            text.font = ArialFont;
            text.material = ArialFont.material;
            text.transform.localPosition = new Vector3(0,0,0);
            text.color = Color.black;
            text.alignment = TextAnchor.MiddleCenter; 
            text.text = initialText;

            return text;
        }

        private Image setupImage(GameObject obj, string itemName) {
            Image image = obj.AddComponent<Image>();
            image.transform.localPosition = new Vector3(0,0,0);

            float slotWidth = GetComponent<Image>().sprite.rect.width;
            float slotHeight = GetComponent<Image>().sprite.rect.height;
            
            image.rectTransform.sizeDelta = new Vector2(slotWidth/8, slotHeight/8);

            if (itemName != "") {
                Sprite icon = Resources.Load("InventoryIcons/" + itemName, typeof(Sprite)) as Sprite;
                image.sprite = icon;
                image.enabled = true;
            }
            else {
                image.enabled = false;
            }

            return image;
        }

        private void updateFields(InventoryItem item) {
            Image image = GameObject.Find("Icon" + id).GetComponentInChildren<Image>();

            if (item.GetId() != nullItem.GetId()) {
                image.sprite = Resources.Load("InventoryIcons/" + item.GetName(), typeof(Sprite)) as Sprite;
                image.enabled = true;
            } else {
                image.enabled = false;
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
                    InventoryItem temp = destination.GetStoredItem();
                    
                    destination.SetStoredItem(source.GetStoredItem());
                    source.SetStoredItem(temp);
                } else {
                    destination.SetStoredItem(source.GetStoredItem());
                    source.SetStoredItem(nullItem);
                }
                
            }
        } 
       
        
        
        
    }
}    
