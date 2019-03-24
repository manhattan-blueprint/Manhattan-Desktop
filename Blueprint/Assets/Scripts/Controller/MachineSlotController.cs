using System.Collections;
using System.Collections.Generic;
using Controller;
using UnityEngine;
using UnityEngine.EventSystems;

public class MachineSlotController : InventorySlotController, IDropHandler {
    [SerializeField] private SlotType SlotType;

    public new void OnDrop(PointerEventData eventData) {
        // TODO: same logic as previously, but instead of 'SwapItemLocations': remove from inv, add to machine
    }
}

public enum SlotType {
    input,
    output,
    fuel
}
