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
    private InventoryController machineInventoryController;
    private InventorySlotController inventorySlotController;
    private int newQuantity;

    private void Start() {
        raycaster = GetComponentInParent<GraphicRaycaster>();

        Transform parentCanvasObject = gameObject.transform.parent.parent; 
        if (parentCanvasObject.name == "MachineInventoryCanvas") {
            secondaryCanvasRaycaster = GameObject.Find("MachineCanvas").GetComponent<GraphicRaycaster>();
        } else if (parentCanvasObject.name == "MachineCanvas") {
            secondaryCanvasRaycaster = GameObject.Find("MachineInventoryCanvas").GetComponent<GraphicRaycaster>();
        }

        // Retrieve components
        eventSystem = GetComponent<EventSystem>();
        inventoryController = GameObject.Find("InventoryUICanvas").GetComponent<InventoryController>();
        machineInventoryController = GameObject.Find("MachineInventoryCanvas").GetComponent<InventoryController>();
        inventorySlotController = gameObject.transform.parent.GetComponent<InventorySlotController>();
        foregroundObject = GameObject.Find(this.transform.parent.parent.name + "/drag");
        foregroundImage = foregroundObject.GetComponent<Image>();

        // Reset DragDestination
        if (inventorySlotController.id == 0) inventoryController.DragDestination = -1;
    }

    private void Update() {
        // DRAG
        // When left mouse button is down
        if (Input.GetMouseButtonDown(0)) {
            // End drag behaviour
            if (dragging) {
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
                        // Dragging
                        if (isc is MachineSlotController) {
                            isc.GetComponentInParent<MachineSlotController>().OnDrop(dragObject, false);
                        } else {
                            isc.OnDrop(dragObject);
                        }
                    } else {
                        // Splitting 
                        InventoryItem item = inventorySlotController.storedItem.Get();
                        
                        if (isc is MachineSlotController) {
                            // Into machine
                            isc.GetComponentInParent<MachineSlotController>().OnDrop(dragObject, true);
                        } else {
                            // Into inventory slot
                            if (isc.storedItem.IsPresent()) {
                                InventoryItem originalItem = inventorySlotController.storedItem.Get();
                                
                                // Into occupied slot...
                                if (isc.storedItem.Get().GetId() == inventorySlotController.storedItem.Get().GetId()) {
                                    // ... of the same type 
                                    GameManager.Instance().inventoryStore.Dispatch(new AddItemToInventoryAtHex(originalItem.GetId(), 
                                    newQuantity, originalItem.GetName(), isc.id));
                                } else {
                                    // ... of a different type
                                    GameManager.Instance().inventoryStore.Dispatch(new AddItemToInventoryAtHex(originalItem.GetId(), 
                                    newQuantity, originalItem.GetName(), inventorySlotController.id));
                                }
                            } else {
                                // Into unoccupied slot
                                GameManager.Instance().inventoryStore.Dispatch(new AddItemToInventoryAtHex(item.GetId(), newQuantity, item.GetName(), isc.id));
                            }
                        }
                    }
                    
                    inventoryController.DragDestination = isc.id;
                } else {
                    // Drop item outside the inventory while splitting
                    if (splitting) {
                        InventoryItem originalItem = inventorySlotController.storedItem.Get();
                        GameManager.Instance().inventoryStore.Dispatch(new AddItemToInventoryAtHex(originalItem.GetId(), 
                            newQuantity, originalItem.GetName(), inventorySlotController.id));
                    }
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
        if (Input.GetMouseButtonDown(1) && mouseOver && currentItem.GetQuantity() > 1 && !inventoryController.DraggingInvItem) {
            newQuantity = (int) currentItem.GetQuantity() / 2;
            
            if (inventorySlotController is MachineSlotController) {
                Vector2 machineLocation = (inventorySlotController as MachineSlotController).MachineController
                    .machineLocation;
                currentItem.SetQuantity(currentItem.GetQuantity() - newQuantity);
                
                if (gameObject.transform.parent.name == "InputSlot0") {
                    GameManager.Instance().machineStore.Dispatch(new SetLeftInput(machineLocation, currentItem));
                    inventorySlotController.SetStoredItem(Optional<InventoryItem>.Of(currentItem)); 
                } else if (gameObject.transform.parent.name == "InputSlot1") {
                    GameManager.Instance().machineStore.Dispatch(new SetRightInput(machineLocation, currentItem));
                    inventorySlotController.SetStoredItem(Optional<InventoryItem>.Of(currentItem)); 
                } else if (gameObject.transform.parent.name == "FuelSlot") {
                    GameManager.Instance().machineStore
                        .Dispatch(new SetFuel(machineLocation, Optional<InventoryItem>.Of(currentItem)));
                    inventorySlotController.SetStoredItem(Optional<InventoryItem>.Of(currentItem));
                }

            } else {
                // Remove drag quantity from source hex
                GameManager.Instance().inventoryStore.Dispatch(new RemoveItemFromStackInventory(currentItem.GetId(), 
                newQuantity, inventorySlotController.id));
            }
            

            beginSplit();
        }

        // Icon follows mouse when left mouse button not down
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
        
        inventoryController.RedrawInventory();
        machineInventoryController.RedrawInventory();
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
        endDrag();
    }

    public void OnPointerEnter(PointerEventData pointerEventData) {
        if (!mouseOver) mouseOver = true;
    }
    
    public void OnPointerExit(PointerEventData pointerEventData) {
        if (mouseOver) mouseOver = false;
    }
}
