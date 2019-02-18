using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler {
    public void OnDrag(PointerEventData eventData) {
        transform.parent.SetSiblingIndex(1);
        transform.position = Input.mousePosition;
    }
	
    public void OnEndDrag(PointerEventData eventData) {
        transform.localPosition = Vector3.zero;
    }
}
