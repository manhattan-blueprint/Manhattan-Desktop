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

public class GoalSlotController : InventorySlotController {
    [SerializeField] private GoalSlotType slotType;
    internal GoalUIController goalUIController = null;

    public new void OnDrop(GameObject droppedObject, bool splitting) {
        RectTransform invPanel = transform as RectTransform;
        if (goalUIController == null)
            goalUIController = GameObject.Find("GoalCanvas").GetComponent<GoalUIController>();

        InventorySlotController source = droppedObject.transform.GetComponent<InventorySlotController>();
        InventorySlotController destination = gameObject.GetComponent<InventorySlotController>();

        if (RectTransformUtility.RectangleContainsScreenPoint(invPanel, Input.mousePosition)) {

            // Don't move the item if item is incorrect.
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
