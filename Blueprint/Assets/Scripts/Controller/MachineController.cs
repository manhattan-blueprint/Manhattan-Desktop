using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MachineController : MonoBehaviour {
    private GameObject machineInventoryCanvas;
    private GameObject InputSlot0;
    private GameObject InputSlot1;
    
    // Start is called before the first frame update
    void Start() {
        machineInventoryCanvas = GameObject.Find("MachineInventoryCanvas");
        machineInventoryCanvas.GetComponent<CanvasScaler>().scaleFactor = 0.7f;
        machineInventoryCanvas.transform.position = new Vector2(Screen.width/3, Screen.height/2); 
    }
    
    // TODO
    // MachineState: *hexCoord -> [Objects in machine]
    // Inv -> Machine : RemoveFromInv ]
    // Machine -> Inv : AddToInventoryPosition
    // Each Machine object attaches MachineController, does interface w/ schema
    // On drag result : change opacity to 1, 'consume' inv inputs
    // fuel on MachineID, fuel vs electricity
    // Machine destruction w/ items, AddToInv
    // MachineInputSlotController & MachineOutputSlotController
}
