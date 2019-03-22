using System.Collections;
using System.Collections.Generic;
using Model;
using Model.Redux;
using Model.State;
using UnityEngine;
using UnityEngine.UI;

public class MachineController : MonoBehaviour, Subscriber<MachineState> {
    private Vector2 machineLocation;
    private GameObject machineInventoryCanvas;
    private GameObject InputSlot0;
    private GameObject InputSlot1;
    
    // Start is called before the first frame update
    void Start() {
        machineInventoryCanvas = GameObject.Find("MachineInventoryCanvas");
        machineInventoryCanvas.GetComponent<CanvasScaler>().scaleFactor = 0.7f;
        machineInventoryCanvas.transform.position = new Vector2(Screen.width/3, Screen.height/2); 
    }

    public void SetMachineLocation(Vector2 machineLocation) {
        this.machineLocation = machineLocation;
    }
    
    public void StateDidUpdate(MachineState state) {
        Machine machine = state.grid[machineLocation];
        // TODO: Layout ui based on machine info
        // TODO: Check if any of the inputs make an output
        // TODO: show output in output cell, fade opacity to 50% of inputs
        // TODO: Do the inputs to GOH need to also specify fuel? Or is that machine enforced?
    }
    
    // TODO
    // Inv -> Machine : RemoveFromInv ] - This should be called from MachineSlotController
    // Machine -> Inv : AddToInventoryPosition 
    // On drag result : change opacity to 1, 'consume' inv inputs
    // Machine destruction w/ items, AddToInv
    // MachineInputSlotController & MachineOutputSlotController
    
    
}
