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
        Sprite originalSprite = gameObject.transform.GetChild(0).GetComponent<Image>().sprite;
        Image originalImage = gameObject.transform.GetChild(0).GetComponent<Image>();
        gameObject.transform.parent.GetComponentInChildren<Text>().text = "";

        // Foreground object
        foregroundObject = GameObject.Find(this.transform.parent.parent.name + "/drag");
        foregroundImage = foregroundObject.GetComponent<Image>();
        RectTransform rect = transform as RectTransform;
        
        foregroundImage.enabled = true;
        foregroundImage.sprite = originalSprite;
        foregroundImage.rectTransform.sizeDelta = new Vector2((originalImage.transform as RectTransform).sizeDelta.x,
            (originalImage.transform as RectTransform).sizeDelta.y);
        originalImage.enabled = false;
        
        foregroundObject.transform.position = Input.mousePosition;
    }
	
    public void OnEndDrag(PointerEventData eventData) {
        float slotHeight = (eventData.pointerEnter.transform.parent.transform as RectTransform).rect.height;
        transform.localPosition = new Vector3(0, 0, 0);
        foregroundImage.enabled = false;
        GameObject.Find("InventoryUICanvas").GetComponent<InventoryController>().RedrawInventory();
        GameObject.Find("MachineInventoryCanvas").GetComponent<InventoryController>().RedrawInventory();
    }
}
