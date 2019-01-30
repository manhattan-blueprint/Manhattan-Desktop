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
        MachineryController mc = GameObject.Find("MachineryUICanvas").GetComponent<MachineryController>();
        
        // Returns object
        // Text -> Slot.GetId() -> Inventory.GetItems[Slot.GetId]
        inv.GetItems()[isc.id].GetItemType();
        
        // Add object to list in machinery controller
        // Write fn to check inputs against GameObjectsHandler when both slots are populated
        // If object, add to output location and remove inputs from inventory
        // Else do nothing

        if (RectTransformUtility.RectangleContainsScreenPoint(invPanel, Input.mousePosition)) {
            mc.inputs.Add(inv.GetItems()[isc.id]);
            
            Text text = GetComponentInChildren<Text>();
            Font ArialFont = (Font) Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
            text.font = ArialFont;
            text.material = ArialFont.material;
            text.transform.localPosition = new Vector3(0,0,0);
            text.color = Color.black;
            text.alignment = TextAnchor.MiddleCenter;
            text.text = inv.GetItems()[isc.id].GetItemType();
        }
    }
}
