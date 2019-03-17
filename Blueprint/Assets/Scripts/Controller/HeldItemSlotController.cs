using Model;
using UnityEngine;
using UnityEngine.UI;

namespace Controller {
    public class HeldItemSlotController : MonoBehaviour {
        private float slotHeight;
        private float slotWidth;
        private Image image;
        private Text text; 
        
        public int id;
        public SVGImage border;

        private void Start() {
            this.slotHeight = (transform as RectTransform).rect.height;
            this.slotWidth = (transform as RectTransform).rect.width;
            
            GameObject newGO = new GameObject("Icon" + id);
            newGO.transform.SetParent(gameObject.transform);

            // Configure image
            image = newGO.AddComponent<Image>();
            image.transform.localPosition = new Vector3(0, slotHeight/8, 0);
            image.enabled = false;
            image.rectTransform.sizeDelta = new Vector2(slotWidth/3, slotHeight/3);
           
            // Configure text
            GameObject textObj = new GameObject("Text" + id);
            textObj.transform.parent = this.transform;
            
            text = textObj.AddComponent<Text>();
            text.font = AssetManager.Instance().FontHelveticaNeueBold;
            text.transform.localPosition = new Vector3(0, -slotHeight/6, 0);
            text.color = AssetManager.Instance().ColourOffWhite;
            text.alignment = TextAnchor.MiddleCenter; 
            text.text = "";
            text.raycastTarget = false;
            text.fontSize = AssetManager.Instance().QuantityFieldFontSize;
            text.enabled = false;
        }

        public void SetStoredItem(Optional<InventoryItem> item) {
            if (image == null || text == null) return;
            
            if (!item.IsPresent() || item.Get().GetQuantity() == 0) {
                image.enabled = false;
                text.enabled = false;
            } else {
                image.sprite = AssetManager.Instance().GetItemSprite(item.Get().GetId());
                text.text = item.Get().GetQuantity().ToString();
                    
                image.enabled = true;
                text.enabled = true;
                image.transform.localPosition = new Vector3(0, slotHeight/8, 0);
            }
        }
    }
}