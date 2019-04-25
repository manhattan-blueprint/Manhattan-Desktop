using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Controller;
using Model;
using Model.Action;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotDragHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    private GameObject foregroundObject;
    private Image foregroundImage;
    private bool mouseOver = false;
    private bool dragging = false;
    private bool splitting = false;
    private GraphicRaycaster raycaster;
    private GraphicRaycaster secondaryCanvasRaycaster;
    private EventSystem eventSystem;
    private GameObject dragObject;
    private InventoryController inventoryController;
    private InventorySlotController inventorySlotController;

    private void Start() {
        raycaster = GetComponentInParent<GraphicRaycaster>();

        Transform parentCanvasObject = gameObject.transform.parent.parent; 
        if (parentCanvasObject.name == "MachineInventoryCanvas") {
            secondaryCanvasRaycaster = GameObject.Find("MachineCanvas").GetComponent<GraphicRaycaster>();
        } else if (parentCanvasObject.name == "MachineCanvas") {
            secondaryCanvasRaycaster = GameObject.Find("MachineInventoryCanvas").GetComponent<GraphicRaycaster>();
        }

        eventSystem = GetComponent<EventSystem>();
        inventoryController = GameObject.Find("InventoryUICanvas").GetComponent<InventoryController>();
        inventorySlotController = gameObject.transform.parent.GetComponent<InventorySlotController>();

        if (inventorySlotController.id == 0) inventoryController.DragDestination = -1;
        
        foregroundObject = GameObject.Find(this.transform.parent.parent.name + "/drag");
        foregroundImage = foregroundObject.GetComponent<Image>();
    }

    private void Update() {
        
        // DRAG
        // When left mouse button is down
        if (Input.GetMouseButtonDown(0)) {
            
            // End drag behaviour
            if (dragging && !mouseOver) {
                // Raycast to determine new slot
                PointerEventData ped = new PointerEventData(eventSystem);
                ped.position = Input.mousePosition;
                List<RaycastResult> results = new List<RaycastResult>();
                raycaster.Raycast(ped, results);
                
                // If no results, try secondary canvas
                if (results.Count == 0) {
                   secondaryCanvasRaycaster.Raycast(ped, results); 
                } 

                // Drop over slot?  
                InventorySlotController isc = null;
                foreach (RaycastResult result in results) {
                    if (result.gameObject.GetComponent<InventorySlotController>()) {
                        isc = result.gameObject.GetComponent<InventorySlotController>();
                    } 
                }

                if (isc != null) {
                    if (!splitting) {
                        if (isc is MachineSlotController) {
                            isc.GetComponentInParent<MachineSlotController>().OnDrop(dragObject, false);
                        } else {
                            isc.OnDrop(dragObject);
                        }
                    } else {
                        InventoryItem item = inventorySlotController.storedItem.Get();
                        
                        if (isc is MachineSlotController) {
                            isc.GetComponentInParent<MachineSlotController>().OnDrop(dragObject, true);
                        } else {
                            GameManager.Instance().inventoryStore.Dispatch(new AddItemToInventoryAtHex(item.GetId(), item.GetQuantity(), item.GetName(), isc.id));
                        }

                    }
                    
                    inventoryController.DragDestination = isc.id;
                }

                if (!splitting) {
                    endDrag();
                } else {
                    endSplit();
                } 
            }
            
            // Begin drag behaviour
            if (mouseOver && !dragging && !inventoryController.DraggingInvItem &&
                inventorySlotController.storedItem.IsPresent() && (inventorySlotController.id != inventoryController.DragDestination)) {
                
                beginDrag();
            }

            // Reset drag destination once object has been placed
            if (mouseOver && inventorySlotController.id == inventoryController.DragDestination) {
                inventoryController.DragDestination = -1;
            }
        }

        // SPLIT
        // When right mouse button is down
        InventoryItem currentItem = inventorySlotController.storedItem.Get();
        if (Input.GetMouseButtonDown(1) && mouseOver && currentItem.GetQuantity() > 1) {
            int newQuantity = (int) currentItem.GetQuantity() / 2;
            
            // Remove drag quantity from source hex
            GameManager.Instance().inventoryStore.Dispatch(new RemoveItemFromStackInventory(currentItem.GetId(), newQuantity, inventorySlotController.id));

            beginSplit();
        }

        // Icon follows mouse when left mouse button not down
        //if (dragging && !mouseOver) {
        if (dragging) {
            foregroundObject.transform.position = Input.mousePosition;
        }
    }

    private void beginDrag() {
        dragging = true;
        inventoryController.DraggingInvItem = true;
                
        transform.parent.SetSiblingIndex(1);
        transform.position = Input.mousePosition;
        Sprite originalSprite = gameObject.transform.GetChild(0).GetComponent<Image>().sprite;
        Image originalImage = gameObject.transform.GetChild(0).GetComponent<Image>();
        gameObject.transform.parent.GetComponentInChildren<Text>().text = "";

        RectTransform rect = transform as RectTransform;
        foregroundImage.enabled = true;
        foregroundImage.rectTransform.sizeDelta = new Vector2((originalImage.transform as RectTransform).sizeDelta.x,
            (originalImage.transform as RectTransform).sizeDelta.y);
        originalImage.enabled = false;

        // NOTE: unloading & reloading sprite solves resizing issues
        foregroundImage.sprite = null;
        foregroundImage.sprite = originalSprite;
        
        dragObject = gameObject.transform.parent.gameObject;
    }

    private void endDrag() {
        dragging = false;
        inventoryController.DraggingInvItem = false;
        
        transform.localPosition = new Vector3(0, 0, 0);
        foregroundImage.enabled = false;
        GameObject.Find("InventoryUICanvas").GetComponent<InventoryController>().RedrawInventory();
        GameObject.Find("MachineInventoryCanvas").GetComponent<InventoryController>().RedrawInventory();
    }

    private void beginSplit() {
        splitting = true;
        dragging = true;
        inventoryController.DraggingInvItem = true;
                
        transform.parent.SetSiblingIndex(1);
        Sprite originalSprite = gameObject.transform.GetChild(0).GetComponent<Image>().sprite;
        Image originalImage = gameObject.transform.GetChild(0).GetComponent<Image>();

        // Foreground object
        RectTransform rect = transform as RectTransform;
        foregroundImage.enabled = true;
        foregroundImage.rectTransform.sizeDelta = new Vector2((originalImage.transform as RectTransform).sizeDelta.x,
            (originalImage.transform as RectTransform).sizeDelta.y);
        
        // NOTE: unloading & reloading sprite solves resizing issues
        foregroundImage.sprite = null;
        foregroundImage.sprite = originalSprite;
        
        dragObject = gameObject.transform.parent.gameObject;
    }
    
    private void endSplit() {
        splitting = false;
        dragging = false;
        inventoryController.DraggingInvItem = false;
        
        transform.localPosition = new Vector3(0, 0, 0);
        foregroundImage.enabled = false;
        GameObject.Find("InventoryUICanvas").GetComponent<InventoryController>().RedrawInventory();
        GameObject.Find("MachineInventoryCanvas").GetComponent<InventoryController>().RedrawInventory();
    }

    public void OnPointerEnter(PointerEventData pointerEventData) {
        if (!mouseOver) mouseOver = true;
    }
    
    public void OnPointerExit(PointerEventData pointerEventData) {
        if (mouseOver) mouseOver = false;
    }
}
