using System.Collections;
using System.Collections.Generic;
using Controller;
using Model;
using Model.Action;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum GoalSlotType {
    TopSlot,
    MidSlot,
    BotSlot
}

public class GoalSlotController : InventorySlotController, IDropHandler {
    [SerializeField] private GoalSlotType slotType;
    private GoalUIController goalUIController = null;

    public new void OnDrop(PointerEventData eventData) {
        RectTransform invPanel = transform as RectTransform;
        GameObject droppedObject = eventData.pointerDrag;
        if (goalUIController == null)
            goalUIController = GameObject.Find("GoalUICanvas").GetComponent<GoalUIController>();

        InventorySlotController source = droppedObject.transform.parent.GetComponent<InventorySlotController>();
        InventorySlotController destination = gameObject.GetComponent<InventorySlotController>();


        if (RectTransformUtility.RectangleContainsScreenPoint(invPanel, Input.mousePosition)) {

            // Don't move the item if item is incorrect.
            // TODO: Update to correct number once have items for testing in
            // inventory.
            if (slotType == GoalSlotType.TopSlot && source.storedItem.Get().GetId() != 28)
                return;
            if (slotType == GoalSlotType.MidSlot && source.storedItem.Get().GetId() != 31)
                return;
            if (slotType == GoalSlotType.BotSlot && source.storedItem.Get().GetId() != 30)
                return;

            // Only move the item if there is not already one in the goal slot.
            if (!destination.storedItem.IsPresent()) {
                destination.SetStoredItem(source.storedItem);
                source.SetStoredItem(Optional<InventoryItem>.Empty());
            }
            else {
                return;
            }

            // If not from the goal, remove from inventory
            if (droppedObject.transform.parent.GetComponent<GoalSlotController>() == null) {
                if (source.storedItem.IsPresent()) {
                    GameManager.Instance().inventoryStore.Dispatch(new AddItemToInventoryAtHex(source.storedItem.Get().GetId(),
                        source.storedItem.Get().GetQuantity(), source.storedItem.Get().GetName(), source.id));
                }

                GameManager.Instance().inventoryStore.Dispatch(new RemoveItemFromStackInventory(
                    destination.storedItem.Get().GetId(), destination.storedItem.Get().GetQuantity(), source.id));
            }

            if (slotType == GoalSlotType.TopSlot) {
                goalUIController.SetAlpha("TopSlot/TopItem", 1.0f);
                goalUIController.ActivateDish();
                goalUIController.SetSlotActive("MidSlot");
            }
            if (slotType == GoalSlotType.MidSlot) {
                goalUIController.SetAlpha("MidSlot/MidItem", 1.0f);
                goalUIController.ActivateAntenna();
            }
            if (slotType == GoalSlotType.BotSlot) {
                goalUIController.SetAlpha("BotSlot/BotItem", 1.0f);
                goalUIController.ActivateTransmitter();
            }

            // TODO: If all slots filled then start it spinning and congratulate for completing.

        } else {
            droppedObject.transform.parent.GetComponentInChildren<Text>().text =
                source.storedItem.Get().GetQuantity().ToString();
        }
    }
}
