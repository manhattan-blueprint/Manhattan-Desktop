using System.Collections;
using System.Collections.Generic;
using Controller;
using Model;
using Model.Action;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MachineSlotController : InventorySlotController, IDropHandler {
    [SerializeField] private SlotType SlotType;
    private MachineController MachineController = null;

    public new void OnDrop(PointerEventData eventData) {
        RectTransform invPanel = transform as RectTransform;
        GameObject droppedObject = eventData.pointerDrag;

        InventorySlotController source = droppedObject.transform.parent.GetComponent<InventorySlotController>(); 
        InventorySlotController destination = gameObject.GetComponent<InventorySlotController>();

        if (RectTransformUtility.RectangleContainsScreenPoint(invPanel, Input.mousePosition) 
            && SlotType != SlotType.output) {
            
            if (destination.storedItem.IsPresent()) {
                // Move to occupied slot
                Optional<InventoryItem> temp = destination.storedItem;
                
                destination.SetStoredItem(source.storedItem);
                source.SetStoredItem(temp);
            } else {
                // Move to empty slot
                destination.SetStoredItem(source.storedItem);
                source.SetStoredItem(Optional<InventoryItem>.Empty());
            }

            // Remove from inventory
            GameManager.Instance().inventoryStore.Dispatch(new RemoveItemFromStackInventory(
                destination.storedItem.Get().GetId(), destination.storedItem.Get().GetQuantity(), source.id));

            if (MachineController == null) loadMachineController();

            // Add to correct element of machine
            if (SlotType == SlotType.leftInput) {
                GameManager.Instance().machineStore
                    .Dispatch(new SetLeftInput(MachineController.machineLocation, storedItem.Get()));
            } else if (SlotType == SlotType.rightInput) {
                GameManager.Instance().machineStore
                    .Dispatch(new SetRightInput(MachineController.machineLocation, storedItem.Get()));
            } else if (SlotType == SlotType.fuel) {
                GameManager.Instance().machineStore
                    .Dispatch(new SetFuel(MachineController.machineLocation, storedItem.Get()));
            }
        } else {
            droppedObject.transform.parent.GetComponentInChildren<Text>().text = 
                source.storedItem.Get().GetQuantity().ToString();
        }
    }

    private void loadMachineController() {
        MachineController = GameObject.Find("MachineCanvas").GetComponent<MachineController>();
    }
}

public enum SlotType {
    leftInput,
    rightInput,
    output,
    fuel
}
