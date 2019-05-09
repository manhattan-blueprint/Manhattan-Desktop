using Controller;
using Model;
using Model.Action;
using UnityEngine;
using UnityEngine.UI;

public class MachineSlotController : InventorySlotController {
    [SerializeField] private SlotType SlotType;
    internal MachineController MachineController = null;
    private string InputSlot0 = "InputSlot0";
    private string InputSlot1 = "InputSlot1";
    private string FuelSlot = "FuelSlot";
    private string OutputSlot = "OutputSlot";

    public new void OnDrop(GameObject droppedObject, bool splitting, int newSplitQuantity) {
        RectTransform invPanel = transform as RectTransform;
        if (MachineController == null) loadMachineController();

        InventorySlotController source = droppedObject.GetComponent<InventorySlotController>(); 
        InventorySlotController destination = gameObject.GetComponent<InventorySlotController>();
        
        if (source == destination) {
                // Dragging to same slot
                if (splitting) {
                    if (source.name == InputSlot0 || source.name == InputSlot1 || source.name == FuelSlot) 
                        storedItem.Get().AddQuantity(newSplitQuantity);
                }
                
                if (source.name == InputSlot0) GameManager.Instance().machineStore.Dispatch(new SetLeftInput(MachineController.machineLocation, storedItem.Get()));
                if (source.name == InputSlot1) GameManager.Instance().machineStore.Dispatch(new SetRightInput(MachineController.machineLocation, storedItem.Get()));
                if (source.name == FuelSlot) GameManager.Instance().machineStore.Dispatch(new SetFuel(MachineController.machineLocation, Optional<InventoryItem>.Of(storedItem.Get())));
                if (source.name == OutputSlot) (this as InventorySlotController).SetStoredItem((this as InventorySlotController).storedItem);
                
                return;
        }
        
        int originalDestinationQuantity = 0, originalSourceQuantity = 0;
        if (destination.storedItem.IsPresent()) originalDestinationQuantity = destination.storedItem.Get().GetQuantity();
        if (source.storedItem.IsPresent()) originalSourceQuantity = source.storedItem.Get().GetQuantity();
        
        // Cancel split into output slot
        if (SlotType == SlotType.output) {

            if (splitting) {
                if (source.name != InputSlot0 && source.name != InputSlot1 && source.name != FuelSlot) {
                    GameManager.Instance().inventoryStore.Dispatch(new AddItemToInventoryAtHex( 
                        source.storedItem.Get().GetId(), source.storedItem.Get().GetQuantity(), source.storedItem.Get().GetName(), source.id));
                } else {
                    InventoryItem refillItem = new InventoryItem(source.storedItem.Get().GetName(), source.storedItem.Get().GetId(), originalSourceQuantity + newSplitQuantity);
                        
                    if (source.name == InputSlot0) GameManager.Instance().machineStore.Dispatch(new SetLeftInput(MachineController.machineLocation, refillItem));
                    if (source.name == InputSlot1) GameManager.Instance().machineStore.Dispatch(new SetRightInput(MachineController.machineLocation, refillItem));
                    if (source.name == FuelSlot) GameManager.Instance().machineStore.Dispatch(new SetFuel(MachineController.machineLocation, Optional<InventoryItem>.Of(refillItem)));
                }
            }
        }
        

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
            if (droppedObject.GetComponent<MachineSlotController>() == null) {
                if (source.storedItem.IsPresent()) {
                    GameManager.Instance().inventoryStore.Dispatch(new AddItemToInventoryAtHex(source.storedItem.Get().GetId(),
                        source.storedItem.Get().GetQuantity(), source.storedItem.Get().GetName(), source.id));
                }
                
                if (!splitting) {
                    GameManager.Instance().inventoryStore.Dispatch(new RemoveItemFromStackInventory(
                    destination.storedItem.Get().GetId(), destination.storedItem.Get().GetQuantity(), source.id));
                }
            }

            // Add to correct element of machine
            if (SlotType == SlotType.leftInput) {
                InventorySlotController fuelSlot = null; 
                if (!MachineController.isElectrical) fuelSlot = GameObject.Find(FuelSlot).GetComponent<InventorySlotController>();
                InventorySlotController rightSlot = GameObject.Find(InputSlot1).GetComponent<InventorySlotController>();


                if (!splitting) {
                    // Default case
                    
                    // Into stack of same type
                    if (source.storedItem.IsPresent() && storedItem.Get().GetId() == source.storedItem.Get().GetId()) {
                        // Add to existing stack of correct item
                        InventoryItem temp = storedItem.Get();
                        temp.AddQuantity(source.storedItem.Get().GetQuantity());
                        
                        // Dragged from...
                        if (source.name == InputSlot1) {
                            GameManager.Instance().machineStore.Dispatch(new ClearRightInput(MachineController.machineLocation));
                        } else if (source.name == FuelSlot) {
                            GameManager.Instance().machineStore.Dispatch(new ClearFuel(MachineController.machineLocation));
                        } else {
                            // ...inventory, add to input slot
                            GameManager.Instance().inventoryStore.Dispatch(
                                new RemoveItemFromStackInventory(source.storedItem.Get().GetId(), source.storedItem.Get().GetQuantity(), source.id));
                        }
                        
                        GameManager.Instance().machineStore.Dispatch(new SetLeftInput(MachineController.machineLocation, temp));

                    } else {
                        if (MachineController.isElectrical) {
                            GameManager.Instance().machineStore.Dispatch(new SetAll(MachineController.machineLocation, storedItem, 
                                rightSlot.storedItem, Optional<InventoryItem>.Empty()));
                        } else {
                            GameManager.Instance().machineStore.Dispatch(new SetAll(MachineController.machineLocation, storedItem, 
                                rightSlot.storedItem, fuelSlot.storedItem));
                        }
                    }
                    
                } else {
                        
                    // Splitting case
                    int initialQuantity;
                    if (source.storedItem.IsPresent()) initialQuantity = source.storedItem.Get().GetQuantity() - storedItem.Get().GetQuantity();
                    else initialQuantity = storedItem.Get().GetQuantity();

                    InventoryItem temp = storedItem.Get();
                    //temp.SetQuantity(newSplitQuantity);
                    InventoryItem addition = new InventoryItem(temp.GetName(), temp.GetId(), originalDestinationQuantity + newSplitQuantity);

                    if (source.storedItem.IsPresent() && storedItem.Get().GetId() == source.storedItem.Get().GetId()) {
                        // Split into stack of same type
                        
                        // If split from other input slot
                        if (source.name == InputSlot1) {
                            InventoryItem initial = new InventoryItem(temp.GetName(), temp.GetId(), originalSourceQuantity);
                            GameManager.Instance().machineStore.Dispatch(new SetRightInput(MachineController.machineLocation, initial));
                            
                        } else if (source.name == FuelSlot) {
                            InventoryItem initial = new InventoryItem(temp.GetName(), temp.GetId(), originalSourceQuantity);
                            GameManager.Instance().machineStore.Dispatch(new SetFuel(MachineController.machineLocation, Optional<InventoryItem>.Of(initial)));
                            
                        } else {
                            GameManager.Instance().inventoryStore.Dispatch(new RemoveItemFromStackInventory(temp.GetId(), initialQuantity, source.id));
                        }
                        
                        GameManager.Instance().machineStore.Dispatch(new SetLeftInput(MachineController.machineLocation, addition));
                        
                    } else if (source.storedItem.IsPresent() && storedItem.Get().GetId() != source.storedItem.Get().GetId()) {
                        // Split into stack of other type
                        InventoryItem nonMatchingSplit = new InventoryItem(source.storedItem.Get().GetName(), source.storedItem.Get().GetId(), originalDestinationQuantity);
                        GameManager.Instance().machineStore.Dispatch(new SetLeftInput(MachineController.machineLocation, nonMatchingSplit));

                        if (source.name != InputSlot1 && source.name != FuelSlot) {
                            GameManager.Instance().inventoryStore.Dispatch(new RemoveItemFromStackInventory(destination.storedItem.Get().GetId(), originalDestinationQuantity, source.id));
                            GameManager.Instance().inventoryStore.Dispatch(new AddItemToInventoryAtHex(source.storedItem.Get().GetId(), originalSourceQuantity, source.storedItem.Get().GetName(), source.id));
                        } else if (source.name == InputSlot1) {
                            InventoryItem nonMatchingMachineSplit = new InventoryItem(source.storedItem.Get().GetName(), source.storedItem.Get().GetId(), rightSlot.storedItem.Get().GetQuantity() + originalSourceQuantity);
                            GameManager.Instance().machineStore.Dispatch(new SetRightInput(MachineController.machineLocation, nonMatchingMachineSplit));
                        } else if (source.name == FuelSlot) {
                            InventoryItem nonMatchingMachineSplit = new InventoryItem(source.storedItem.Get().GetName(), source.storedItem.Get().GetId(), fuelSlot.storedItem.Get().GetQuantity() + originalSourceQuantity);
                            GameManager.Instance().machineStore.Dispatch(new SetFuel(MachineController.machineLocation, Optional<InventoryItem>.Of(nonMatchingMachineSplit)));
                        }

                    } else {
                        GameManager.Instance().machineStore.Dispatch(new SetLeftInput(MachineController.machineLocation, addition));
                    }
                    
                }
            } else if (SlotType == SlotType.rightInput) {
                InventorySlotController leftSlot = GameObject.Find(InputSlot0).GetComponent<InventorySlotController>();
                InventorySlotController fuelSlot = null; 
                if (!MachineController.isElectrical) fuelSlot = GameObject.Find(FuelSlot).GetComponent<InventorySlotController>();
                

                if (!splitting) {
                    // Default case
                    
                    // Into stack of same type
                    if (source.storedItem.IsPresent() && storedItem.Get().GetId() == source.storedItem.Get().GetId()) {
                        // Add to existing stack of correct item
                        InventoryItem temp = storedItem.Get();
                        temp.AddQuantity(source.storedItem.Get().GetQuantity());
                        
                        if (source.name == InputSlot0) {
                            GameManager.Instance().machineStore.Dispatch(new ClearLeftInput(MachineController.machineLocation));
                        } else if (source.name == FuelSlot) {
                            GameManager.Instance().machineStore.Dispatch(new ClearFuel(MachineController.machineLocation));
                        } else {
                            // Remove from inventory, add to input slot
                            GameManager.Instance().inventoryStore.Dispatch(
                                new RemoveItemFromStackInventory(source.storedItem.Get().GetId(), source.storedItem.Get().GetQuantity(), source.id));
                        }
                        
                        GameManager.Instance().machineStore.Dispatch(new SetRightInput(MachineController.machineLocation, temp));
                        
                    } else {
                        if (MachineController.isElectrical) {
                            GameManager.Instance().machineStore.Dispatch(new SetAll(MachineController.machineLocation, leftSlot.storedItem, 
                                storedItem, Optional<InventoryItem>.Empty()));
                        } else {
                            GameManager.Instance().machineStore.Dispatch(new SetAll(MachineController.machineLocation, leftSlot.storedItem, 
                                storedItem, fuelSlot.storedItem));
                        }
                    }
                    
                } else {
                    // Splitting case
                    int initialQuantity;
                    if (source.storedItem.IsPresent()) initialQuantity = source.storedItem.Get().GetQuantity() - storedItem.Get().GetQuantity();
                    else initialQuantity = storedItem.Get().GetQuantity();
                    
                    InventoryItem temp = storedItem.Get();
                    //temp.SetQuantity(newSplitQuantity);
                    InventoryItem addition = new InventoryItem(temp.GetName(), temp.GetId(), originalDestinationQuantity + newSplitQuantity);

                    if (source.storedItem.IsPresent() && storedItem.Get().GetId() == source.storedItem.Get().GetId()) {
                        // Split into stack of same type

                        // If split from other input slot
                        if (source.name == InputSlot0) {
                            InventoryItem initial = new InventoryItem(temp.GetName(), temp.GetId(), originalSourceQuantity);
                            GameManager.Instance().machineStore.Dispatch(new SetLeftInput(MachineController.machineLocation, initial));
                            
                        } else if (source.name == FuelSlot) {
                            InventoryItem initial = new InventoryItem(temp.GetName(), temp.GetId(), originalSourceQuantity);
                            GameManager.Instance().machineStore.Dispatch(new SetFuel(MachineController.machineLocation, Optional<InventoryItem>.Of(initial)));
                         
                        } else {
                            GameManager.Instance().inventoryStore
                                .Dispatch(new RemoveItemFromStackInventory(temp.GetId(), initialQuantity, source.id));
                        }

                        GameManager.Instance().machineStore.Dispatch(new SetRightInput(MachineController.machineLocation, addition));
                        
                    } else if (source.storedItem.IsPresent() && storedItem.Get().GetId() != source.storedItem.Get().GetId()) {
                        // Split into stack of other type
                        InventoryItem nonMatchingSplit = new InventoryItem(source.storedItem.Get().GetName(), source.storedItem.Get().GetId(), originalDestinationQuantity);
                        GameManager.Instance().machineStore.Dispatch(new SetRightInput(MachineController.machineLocation, nonMatchingSplit));

                        if (source.name != InputSlot0 && source.name != FuelSlot) {
                            GameManager.Instance().inventoryStore.Dispatch(new RemoveItemFromStackInventory(destination.storedItem.Get().GetId(), originalDestinationQuantity, source.id));
                            GameManager.Instance().inventoryStore.Dispatch(new AddItemToInventoryAtHex(source.storedItem.Get().GetId(), originalSourceQuantity, source.storedItem.Get().GetName(), source.id));
                        } else if (source.name == InputSlot0){
                            InventoryItem nonMatchingMachineSplit = new InventoryItem(source.storedItem.Get().GetName(), source.storedItem.Get().GetId(), leftSlot.storedItem.Get().GetQuantity() + originalSourceQuantity);
                            GameManager.Instance().machineStore.Dispatch(new SetLeftInput(MachineController.machineLocation, nonMatchingMachineSplit));
                        } else if (source.name == FuelSlot) {
                            InventoryItem nonMatchingMachineSplit = new InventoryItem(source.storedItem.Get().GetName(), source.storedItem.Get().GetId(), fuelSlot.storedItem.Get().GetQuantity() + originalSourceQuantity);
                            GameManager.Instance().machineStore.Dispatch(new SetFuel(MachineController.machineLocation, Optional<InventoryItem>.Of(nonMatchingMachineSplit)));
                        }

                    } else {
                        GameManager.Instance().machineStore.Dispatch(new SetRightInput(MachineController.machineLocation, addition));
                    }
                }
                
            } else if (SlotType == SlotType.fuel) {
                InventorySlotController leftSlot = GameObject.Find(InputSlot0).GetComponent<InventorySlotController>();
                InventorySlotController rightSlot = GameObject.Find(InputSlot1).GetComponent<InventorySlotController>();
                
                if (!splitting) {
                    // Default case
                    
                    // Into stack of same type
                    if (source.storedItem.IsPresent() && storedItem.Get().GetId() == source.storedItem.Get().GetId()) {
                        // Add to existing stack of correct item
                        InventoryItem temp = storedItem.Get();
                        temp.AddQuantity(source.storedItem.Get().GetQuantity());
                        
                        if (source.name == InputSlot0) {
                            GameManager.Instance().machineStore.Dispatch(new ClearLeftInput(MachineController.machineLocation));
                        } else if (source.name == InputSlot1) {
                            GameManager.Instance().machineStore.Dispatch(new ClearRightInput(MachineController.machineLocation));
                        } else {
                            // Remove from inventory, add to input slot
                            GameManager.Instance().inventoryStore.Dispatch(
                                new RemoveItemFromStackInventory(source.storedItem.Get().GetId(), source.storedItem.Get().GetQuantity(), source.id));
                        }
                        
                        GameManager.Instance().machineStore.Dispatch(new SetFuel(MachineController.machineLocation, Optional<InventoryItem>.Of(temp)));
                        
                    } else {
                        GameManager.Instance().machineStore.Dispatch(new SetAll(MachineController.machineLocation, leftSlot.storedItem, 
                            rightSlot.storedItem, storedItem));
                    }
                    
                } else {
                    // Splitting case
                    int initialQuantity;
                    if (source.storedItem.IsPresent()) initialQuantity = source.storedItem.Get().GetQuantity() - storedItem.Get().GetQuantity();
                    else initialQuantity = storedItem.Get().GetQuantity();
                    
                    InventoryItem temp = storedItem.Get();
                    //temp.SetQuantity(newSplitQuantity);
                    InventoryItem addition = new InventoryItem(temp.GetName(), temp.GetId(), originalDestinationQuantity + newSplitQuantity);

                    if (source.storedItem.IsPresent() && storedItem.Get().GetId() == source.storedItem.Get().GetId()) {
                        // Split into stack of same type

                        // If split from other input slot
                        if (source.name == InputSlot0) {
                            InventoryItem initial = new InventoryItem(temp.GetName(), temp.GetId(), originalSourceQuantity);
                            GameManager.Instance().machineStore.Dispatch(new SetLeftInput(MachineController.machineLocation, initial));
                            
                        } else if (source.name == InputSlot1) {
                            InventoryItem initial = new InventoryItem(temp.GetName(), temp.GetId(), originalSourceQuantity);
                            GameManager.Instance().machineStore.Dispatch(new SetRightInput(MachineController.machineLocation, initial));
                         
                        } else {
                            GameManager.Instance().inventoryStore
                                .Dispatch(new RemoveItemFromStackInventory(temp.GetId(), initialQuantity, source.id));
                        }

                        GameManager.Instance().machineStore.Dispatch(new SetFuel(MachineController.machineLocation, Optional<InventoryItem>.Of(addition)));
                        
                    } else if (source.storedItem.IsPresent() && storedItem.Get().GetId() != source.storedItem.Get().GetId()) {
                        // Split into stack of other type
                        InventoryItem nonMatchingSplit = new InventoryItem(source.storedItem.Get().GetName(), source.storedItem.Get().GetId(), originalDestinationQuantity);
                        GameManager.Instance().machineStore.Dispatch(new SetFuel(MachineController.machineLocation, Optional<InventoryItem>.Of(nonMatchingSplit)));

                        if (source.name != InputSlot0 && source.name != InputSlot1) {
                            GameManager.Instance().inventoryStore.Dispatch(new RemoveItemFromStackInventory(destination.storedItem.Get().GetId(), originalDestinationQuantity, source.id));
                            GameManager.Instance().inventoryStore.Dispatch(new AddItemToInventoryAtHex(source.storedItem.Get().GetId(), originalSourceQuantity, source.storedItem.Get().GetName(), source.id));
                        } else if (source.name == InputSlot0){
                            InventoryItem nonMatchingMachineSplit = new InventoryItem(source.storedItem.Get().GetName(), source.storedItem.Get().GetId(), leftSlot.storedItem.Get().GetQuantity() + originalSourceQuantity);
                            GameManager.Instance().machineStore.Dispatch(new SetLeftInput(MachineController.machineLocation, nonMatchingMachineSplit));
                        } else if (source.name == InputSlot1) {
                            InventoryItem nonMatchingMachineSplit = new InventoryItem(source.storedItem.Get().GetName(), source.storedItem.Get().GetId(), rightSlot.storedItem.Get().GetQuantity() + originalSourceQuantity);
                            GameManager.Instance().machineStore.Dispatch(new SetRightInput(MachineController.machineLocation, nonMatchingMachineSplit));
                        }

                    } else {
                        GameManager.Instance().machineStore.Dispatch(new SetFuel(MachineController.machineLocation, Optional<InventoryItem>.Of(addition)));
                    }
                }
                
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
