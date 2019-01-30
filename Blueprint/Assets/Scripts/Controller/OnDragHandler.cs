using System.Collections;
using System.Collections.Generic;
using Controller;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Model;
using View;
using System.Linq;

public class OnDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private List<InventorySlotController> slots;
    
    public void OnDrag(PointerEventData eventData) {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData) {
        transform.localPosition = Vector3.zero;
    }
}