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
        if (MachineController == null) loadMachineController();

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

            // If not from a machine, remove from inventory
            if (droppedObject.transform.parent.GetComponent<MachineSlotController>() == null) {
                if (source.storedItem.IsPresent()) {
                    GameManager.Instance().inventoryStore.Dispatch(new AddItemToInventoryAtHex(source.storedItem.Get().GetId(), 
                        source.storedItem.Get().GetQuantity(), source.storedItem.Get().GetName(), source.id));
                }
                
                GameManager.Instance().inventoryStore.Dispatch(new RemoveItemFromStackInventory(
                    destination.storedItem.Get().GetId(), destination.storedItem.Get().GetQuantity(), source.id));
            }

            // Add to correct element of machine
            if (SlotType == SlotType.leftInput) {
                InventorySlotController fuelSlot = GameObject.Find("FuelSlot").GetComponent<InventorySlotController>();
                InventorySlotController rightSlot = GameObject.Find("InputSlot1").GetComponent<InventorySlotController>();

                GameManager.Instance().machineStore.Dispatch(new SetAll(MachineController.machineLocation, storedItem, 
                    rightSlot.storedItem, fuelSlot.storedItem));
                
            } else if (SlotType == SlotType.rightInput) {
                InventorySlotController leftSlot = GameObject.Find("InputSlot0").GetComponent<InventorySlotController>();
                InventorySlotController fuelSlot = GameObject.Find("FuelSlot").GetComponent<InventorySlotController>();
                
                GameManager.Instance().machineStore.Dispatch(new SetAll(MachineController.machineLocation, leftSlot.storedItem, 
                    storedItem, fuelSlot.storedItem));
                
            } else if (SlotType == SlotType.fuel) {
                InventorySlotController leftSlot = GameObject.Find("InputSlot0").GetComponent<InventorySlotController>();
                InventorySlotController rightSlot = GameObject.Find("InputSlot1").GetComponent<InventorySlotController>();
                
                GameManager.Instance().machineStore.Dispatch(new SetAll(MachineController.machineLocation, leftSlot.storedItem, 
                    rightSlot.storedItem, storedItem));
                
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
