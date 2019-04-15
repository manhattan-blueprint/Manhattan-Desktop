using System.Collections;
using System.Collections.Generic;
using Controller;
using Model;
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
    private GraphicRaycaster raycaster;
    private EventSystem eventSystem;
    private GameObject dragObject;
    private InventoryController inventoryController;
    private InventorySlotController inventorySlotController;

    private void Start() {
        raycaster = GetComponentInParent<GraphicRaycaster>();
        eventSystem = GetComponent<EventSystem>();
        inventoryController = GameObject.Find("InventoryUICanvas").GetComponent<InventoryController>();
        inventorySlotController = gameObject.transform.parent.GetComponent<InventorySlotController>();

        if (inventorySlotController.id == 0) inventoryController.DragDestination = -1;
    }

    private void Update() {
        
        // When left mouse button is down
        if (Input.GetMouseButtonDown(0)) {
            
            // End drag behaviour
            if (dragging && !mouseOver) {
                // Raycast to determine new slot
                PointerEventData ped = new PointerEventData(eventSystem);
                ped.position = Input.mousePosition;
                List<RaycastResult> results = new List<RaycastResult>();
                raycaster.Raycast(ped, results);

                // Drop over slot?  
                InventorySlotController isc = null;
                foreach (RaycastResult result in results) {
                    if (result.gameObject.GetComponent<InventorySlotController>()) {
                        isc = result.gameObject.GetComponent<InventorySlotController>();
                    } 
                }

                if (isc != null) {
                    isc.OnDrop(dragObject);
                    inventoryController.DragDestination = isc.id;
                }
                endDrag();
            }
            
            // Begin drag behaviour
            if (mouseOver && !dragging && !inventoryController.DraggingInvItem &&
                inventorySlotController.storedItem.IsPresent() && (inventorySlotController.id != inventoryController.DragDestination)) {
                
                beginDrag();
            }

            // Reset drag destination once object has been placed
            if (inventorySlotController.id == inventoryController.DragDestination) {
                inventoryController.DragDestination = -1;
            }
        }

        // Icon follows mouse when left mouse button not down
        if (dragging && !mouseOver) {
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

        // Foreground object
        foregroundObject = GameObject.Find(this.transform.parent.parent.name + "/drag");
        foregroundImage = foregroundObject.GetComponent<Image>();
        RectTransform rect = transform as RectTransform;
        
        foregroundImage.enabled = true;
        foregroundImage.sprite = originalSprite;
        foregroundImage.rectTransform.sizeDelta = new Vector2((originalImage.transform as RectTransform).sizeDelta.x,
            (originalImage.transform as RectTransform).sizeDelta.y);
        originalImage.enabled = false;

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

    public void OnPointerEnter(PointerEventData pointerEventData) {
        if (!mouseOver) mouseOver = true;
    }
    
    public void OnPointerExit(PointerEventData pointerEventData) {
        if (mouseOver) mouseOver = false;
    }
}
