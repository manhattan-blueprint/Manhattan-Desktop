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

            if (destination.storedItem.IsPresent()) {
                Debug.Log("Moving item " + source.storedItem);

                // Move to occupied slot
                Optional<InventoryItem> temp = destination.storedItem;

                destination.SetStoredItem(source.storedItem);
                source.SetStoredItem(temp);
            } else {
                // Move to empty slot
                destination.SetStoredItem(source.storedItem);
                source.SetStoredItem(Optional<InventoryItem>.Empty());
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

            // Add to correct element of machine
            if (slotType == GoalSlotType.TopSlot) {
                InventorySlotController topSlot = GameObject.Find("TopSlot").GetComponent<InventorySlotController>();
                InventorySlotController rightSlot = GameObject.Find("InputSlot1").GetComponent<InventorySlotController>();
                Debug.Log("SLOTTING");
            }
        } else {
            droppedObject.transform.parent.GetComponentInChildren<Text>().text =
                source.storedItem.Get().GetQuantity().ToString();
        }
    }
}
