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
            if (slotType == GoalSlotType.MidSlot) {
                if (source.storedItem.Get().GetId() != 31)
                    return;
                if (!goalUIController.CheckPlaced(GoalPosition.Top))
                    return;
            }
            if (slotType == GoalSlotType.BotSlot) {
                if (source.storedItem.Get().GetId() != 30)
                    return;
                if (!goalUIController.CheckPlaced(GoalPosition.Mid))
                    return;
            }

            // Only move the item if there is not already one in the goal slot.
            if (!destination.storedItem.IsPresent()) {
                destination.storedItem = source.storedItem;
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
                GameManager.Instance().mapStore.Dispatch(new UpdateGoal(GoalPosition.Top));
            }
            if (slotType == GoalSlotType.MidSlot) {
                GameManager.Instance().mapStore.Dispatch(new UpdateGoal(GoalPosition.Mid));
            }
            if (slotType == GoalSlotType.BotSlot) {
                GameManager.Instance().mapStore.Dispatch(new UpdateGoal(GoalPosition.Bot));
            }

            if (GameManager.Instance().mapStore.GetState().getGoal().IsComplete()) {
                GameObject.Find("MenuController").GetComponent<MenuController>().GameOver();
                goalUIController.StartWinAnimation();
            }

        } else {
            droppedObject.transform.parent.GetComponentInChildren<Text>().text =
                source.storedItem.Get().GetQuantity().ToString();
        }
    }
}
