using System.Collections;
using System.Collections.Generic;
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
        Sprite originalImage = GameObject.Find(this.transform.name).GetComponent<Image>().sprite;

        // Foreground object
        foregroundObject = GameObject.Find("drag");
        foregroundImage = foregroundObject.GetComponent<Image>();
        RectTransform rect = transform as RectTransform;
        
        foregroundImage.enabled = true;
        foregroundImage.sprite = originalImage;
        foregroundImage.rectTransform.sizeDelta = new Vector2(rect.rect.width, rect.rect.height);
        
        foregroundObject.transform.position = Input.mousePosition;
    }
	
    public void OnEndDrag(PointerEventData eventData) {
        transform.localPosition = Vector3.zero;
        foregroundImage.enabled = false;
    }
}
