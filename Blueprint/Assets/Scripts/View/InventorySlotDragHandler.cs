using System.Collections;
using System.Collections.Generic;
using Controller;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler {
    private GameObject foregroundObject;
    private Image foregroundImage;
    
    public void OnDrag(PointerEventData eventData) {
        transform.parent.SetSiblingIndex(1);
        transform.position = Input.mousePosition;
        Sprite originalImage = gameObject.GetComponent<Image>().sprite;
        gameObject.transform.parent.GetComponentInChildren<Text>().text = "";

        // Foreground object
        foregroundObject = GameObject.Find(this.transform.parent.parent.name + "/drag");
        foregroundImage = foregroundObject.GetComponent<Image>();
        RectTransform rect = transform as RectTransform;
        
        foregroundImage.enabled = true;
        foregroundImage.sprite = originalImage;
        foregroundImage.rectTransform.sizeDelta = new Vector2(rect.rect.width, rect.rect.height);
        
        foregroundObject.transform.position = Input.mousePosition;
    }
	
    public void OnEndDrag(PointerEventData eventData) {
        float slotHeight = (eventData.pointerEnter.transform.parent.transform as RectTransform).rect.height;
        transform.localPosition = new Vector3(0, slotHeight/8, 0);
        foregroundImage.enabled = false;
        GameObject.Find("InventoryUICanvas").GetComponent<InventoryController>().RedrawInventory();
    }
}
