using System.Collections;
using System.Collections.Generic;
using Controller;
using Model;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnDropHandler : MonoBehaviour, IDropHandler
{   
    public void OnDrop(PointerEventData eventData) {
        RectTransform invPanel = transform as RectTransform;
        GameObject droppedObject = eventData.pointerDrag;
        
        InventorySlotController isc = GameObject.Find(droppedObject.name).GetComponentInParent<InventorySlotController>();
        InventoryController inv = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryController>();
        MachineryController mc = GameObject.FindGameObjectWithTag("Player").GetComponent<MachineryController>();

        if (RectTransformUtility.RectangleContainsScreenPoint(invPanel, Input.mousePosition)) {
            // TODO: Remove dependency on imported MachineryController
            mc.AddInputItem(inv.GetItems()[isc.id].GetId(), inv.GetItems()[isc.id].GetQuantity());
            
            Text text = GetComponentInChildren<Text>();
            Font ArialFont = (Font) Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
            text.font = ArialFont;
            text.material = ArialFont.material;
            text.transform.localPosition = new Vector3(0,0,0);
            text.color = Color.black;
            text.alignment = TextAnchor.MiddleCenter;
            text.text = inv.GetItems()[isc.id].GetName();
        }
    }
}
