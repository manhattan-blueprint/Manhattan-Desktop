using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Controller;
using Model;
using Model.Action;
using Model.Redux;
using Model.State;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class MachineController : MonoBehaviour, Subscriber<MachineState>, Subscriber<UIState> {
    // TODO: avoid making this public
    public Vector2 machineLocation;
    private GameObject machineInventoryCanvas;
    private GameObject outputSlot;
    private GameObject fuelSlot;
    private GameObject inputSlot0;
    private GameObject inputSlot1;
    
    void Start() {
        machineInventoryCanvas = GameObject.Find("MachineInventoryCanvas");
        machineInventoryCanvas.GetComponent<CanvasScaler>().scaleFactor = 0.7f;
        machineInventoryCanvas.transform.position = new Vector2(Screen.width/3, Screen.height/2);
        outputSlot = GameObject.Find("OutputSlot");
        fuelSlot = GameObject.Find("FuelSlot");
        inputSlot0 = GameObject.Find("InputSlot0");
        inputSlot1 = GameObject.Find("InputSlot1");
        
        GameManager.Instance().uiStore.Subscribe(this);
        GameManager.Instance().machineStore.Subscribe(this);
    }
    
    public void StateDidUpdate(MachineState state) {
        if (!state.grid.ContainsKey(machineLocation)) {
            return;
        }
        
        populateOutputSlot(Optional<InventoryItem>.Empty());
        Machine machine = state.grid[machineLocation];
        // TODO: Layout ui based on machine info (clear output)
        
        // Check the fuel is present otherwise don't bother checking what we can make
        if (!machine.HasFuel()) {
            return;
        }

        // Check if anything can be made
        Optional<GameObjectEntry> possibleOutput = GameManager.Instance().goh.GetRecipe(machine.GetInputs(), machine.id);
        if (possibleOutput.IsPresent()) {
            GameObjectEntry output = possibleOutput.Get();
            // This _should_ be an explicit state action, but that will cause this function to be called indefinitely
            // TODO: Think of a better way of doing this
            machine.output = Optional<InventoryItem>.Of(new InventoryItem(output.name, output.item_id, 1));    
            // TODO: show output in output cell, fade opacity to 50% of inputs
            
            populateOutputSlot(machine.output);
        }

        if (machine.leftInput.IsPresent()) {
            inputSlot0.GetComponent<InventorySlotController>().SetStoredItem(machine.leftInput);
        }
        if (machine.rightInput.IsPresent()) {
            inputSlot1.GetComponent<InventorySlotController>().SetStoredItem(machine.rightInput);
        }
        if (machine.fuel.IsPresent()) {
            fuelSlot.GetComponent<InventorySlotController>().SetStoredItem(machine.fuel);
        }
        
        refreshInputSlots(machine.leftInput, machine.rightInput, machine.fuel);
    }

    public void StateDidUpdate(UIState state) {
        if (state.Selected != UIState.OpenUI.Machine) return;
        this.machineLocation = state.SelectedMachineLocation;
    }

    private void populateOutputSlot(Optional<InventoryItem> item) {
        outputSlot.GetComponent<MachineSlotController>().SetStoredItem(item);

        // Make output slot image translucent
        Color temp = outputSlot.GetComponentsInChildren<Image>()[1].color;
        temp.a = 0.5f;
        outputSlot.GetComponentsInChildren<Image>()[1].color = temp;
    }

    private void refreshInputSlots(Optional<InventoryItem> left, Optional<InventoryItem> right, Optional<InventoryItem> fuel) {
        inputSlot0.GetComponent<InventorySlotController>().SetStoredItem(left);
        inputSlot1.GetComponent<InventorySlotController>().SetStoredItem(right);
        fuelSlot.GetComponent<InventorySlotController>().SetStoredItem(fuel);
    }
    
    // TODO
    // Increase quantity / split stacks?
    // Inv -> Machine : RemoveFromInv ] - This should be called from MachineSlotController
    // Machine -> Inv : AddToInventoryPosition 
    // On drag result : change opacity to 1, 'consume' inv inputs
    // Machine destruction w/ items, AddToInv
    // MachineInputSlotController & MachineOutputSlotController
}
