using System.Collections.Generic;
using Controller;
using Model;
using Model.Action;
using Model.Redux;
using Model.State;
using Service.Response;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Tutorial Summary
// - Player starts in the world
// - Must press one movement key, then a 10s timer is started
// - After the timer expires, shown a message about inventory
// - Opens inventory
// - Moves Item
// - Splits item 
// - Collects resources from backpack
// - Show held item UI
// - Close inventory
// - Open blueprint UI
// - Show primary resources
// - Show furnace and click on
// - Explain materials to build
// - Explain things can make with furnace
// - Explain notes

public class TutorialController : MonoBehaviour, 
    Subscriber<TutorialState>, Subscriber<InventoryState>, Subscriber<UIState>, Subscriber<HeldItemState>, Subscriber<MapState>, Subscriber<MachineState> {
    [SerializeField] private Image cursor;
    [SerializeField] private SVGImage rmb;
    [SerializeField] private Canvas alertCanvas;
    [SerializeField] private Canvas inventoryCanvas;
    [SerializeField] private Canvas machineCanvas;
    [SerializeField] private Canvas machineInventoryCanvas;
    [SerializeField] private Canvas blueprintCanvas;
    [SerializeField] private Canvas blueprintTemplateCanvas;
    [SerializeField] private Canvas heldItemCanvas;
    [SerializeField] private Canvas inventoryMask;
    [SerializeField] private Canvas backpackMask;
    [SerializeField] private Canvas heldItemMask;
    [SerializeField] private Canvas primaryResourceMask;
    [SerializeField] private Canvas furnaceMask;
    [SerializeField] private Canvas componentsMask;
    [SerializeField] private Canvas recipeMask;
    [SerializeField] private Canvas notesMask;
    [SerializeField] private Canvas blueprintBackMask;
    [SerializeField] private Canvas gridMask;
    [SerializeField] private Canvas machineInputMask;
    [SerializeField] private Canvas machineOutputMask;
    [SerializeField] private Canvas machineGeneralMask;
    [SerializeField] private Canvas finishingCanvas;

    private List<InventoryEntry> mockBackpackContents;
    private int scrollCount;

    void Start() {
        rmb.enabled = false;
        alertCanvas.enabled = false;
        inventoryCanvas.enabled = false;
        machineCanvas.enabled = false;
        machineInventoryCanvas.enabled = false;
        blueprintCanvas.enabled = false;
        blueprintTemplateCanvas.enabled = false;
        heldItemCanvas.enabled = false;
        inventoryMask.enabled = false;
        backpackMask.enabled = false;
        heldItemMask.enabled = false;
        primaryResourceMask.enabled = false;
        furnaceMask.enabled = false;
        componentsMask.enabled = false;
        recipeMask.enabled = false;
        notesMask.enabled = false;
        blueprintBackMask.enabled = false;
        gridMask.enabled = false;
        machineInputMask.enabled = false;
        machineOutputMask.enabled = false;
        machineGeneralMask.enabled = false;
        finishingCanvas.enabled = false;
        inventoryCanvas.gameObject.GetComponent<InventoryController>().backpackButton.enabled = false;
        machineInventoryCanvas.transform.position = new Vector2(Screen.width/3, Screen.height/2);
        
        GameManager.Instance().tutorialStore.Subscribe(this);
        
        // Give some mock data
        GameManager.Instance().inventoryStore.Dispatch(new AddItemToInventoryAtHex(1, 10, "Wood", 6));
        mockBackpackContents = new List<InventoryEntry> {
            new InventoryEntry(2, 4), new InventoryEntry(3, 4)
        };
    }

    private void Update() {
        // If in welcome, check if they've moved and have dismissed the canvas
        if (GameManager.Instance().tutorialStore.GetState().stage == TutorialState.TutorialStage.Welcome && !alertCanvas.enabled) {
            // If press one of the 4 movement commands
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)) {
                GameManager.Instance().tutorialStore.Dispatch(new StartMoving());
            }
        }
        
        // If waiting to go to inventory, check if they have done so
        if (GameManager.Instance().tutorialStore.GetState().stage == TutorialState.TutorialStage.ShouldOpenInventory) {
            if (Input.GetKeyDown(KeyMapping.Inventory)) {
                alertCanvas.enabled = false;
                GameManager.Instance().tutorialStore.Dispatch(new InsideInventory());
            }  
        }
        
        // If waiting to close inventory, check they have done so
        if (GameManager.Instance().tutorialStore.GetState().stage == TutorialState.TutorialStage.ShouldCloseInventory) {
            if (Input.GetKeyDown(KeyMapping.Inventory) || Input.GetKeyDown(KeyCode.Escape)) {
                alertCanvas.enabled = false;
                GameManager.Instance().tutorialStore.Dispatch(new ClosedInventory());
            }
        }
        
        // If waiting to open blueprint, check they have done so
        if (GameManager.Instance().tutorialStore.GetState().stage == TutorialState.TutorialStage.ShouldOpenBlueprint) {
            if (Input.GetKeyDown(KeyMapping.Blueprint)) {
                alertCanvas.enabled = false;
                GameManager.Instance().tutorialStore.Dispatch(new InsideBlueprint());
            }
        }

        if (GameManager.Instance().tutorialStore.GetState().stage == TutorialState.TutorialStage.ShouldCloseBlueprint) {
            if (Input.GetKeyDown(KeyMapping.Blueprint) || Input.GetKeyDown(KeyCode.Escape)) {
                alertCanvas.enabled = false;
                GameManager.Instance().tutorialStore.Dispatch(new ClosedBlueprint());
            }
        }
        
        // If left mouse button
        if (Input.GetMouseButtonDown(0)) {
            switch (GameManager.Instance().tutorialStore.GetState().stage) {
                case TutorialState.TutorialStage.InsideBlueprint:
                    GameManager.Instance().tutorialStore.Dispatch(new HighlightFurnace());
                    break;
                case TutorialState.TutorialStage.CraftedFurnace:
                    GameManager.Instance().tutorialStore.Dispatch(new HighlightBlueprintNotes());
                    break;
                case TutorialState.TutorialStage.HighlightBlueprintNotes:
                    GameManager.Instance().tutorialStore.Dispatch(new ReturnToProgression());
                    break;
                case TutorialState.TutorialStage.ShouldOpenMachine:
                    GameManager.Instance().tutorialStore.Dispatch(new ShowMachineOutputSlots());
                    break;
                case TutorialState.TutorialStage.ShowMachineOutputSlots:
                    GameManager.Instance().tutorialStore.Dispatch(new ShowAllMachine());
                    break;
            }
        }
    }

    public void StateDidUpdate(TutorialState state) {
        switch (state.stage) {
            case TutorialState.TutorialStage.Welcome:
                // Show welcome message
                ShowMessage("Welcome to Blueprint!", "To move around the world, use:\n W - Forwards\nS - Backwards\nA - Left\nD - Right\n\n You can use the mouse to change the direction you are facing.");
                break;
            case TutorialState.TutorialStage.Moving:
                // Allow some exploration before continuing
                Invoke(nameof(StartInventoryFlow), 5);
                break;
            case TutorialState.TutorialStage.ShouldOpenInventory:
                ShowMessage("Inventory", "To view your inventory, press the E key");
                break;
            case TutorialState.TutorialStage.InsideInventory:
                GameManager.Instance().inventoryStore.Subscribe(this);
                inventoryCanvas.enabled = true;
                inventoryMask.enabled = true;
                DisablePlayer();
                break;
            case TutorialState.TutorialStage.DidMoveInventoryItem:
                inventoryMask.GetComponentInChildren<TextMeshProUGUI>().text = "Right click on an item to split it into smaller quantities";
                break;
            case TutorialState.TutorialStage.DidSplitInventoryItem:
                inventoryMask.enabled = false; 
                backpackMask.enabled = true;
                inventoryCanvas.gameObject.GetComponentInChildren<InventoryController>().backpackButton.enabled = true;
                break;
            case TutorialState.TutorialStage.DidCollectFromBackpack:
                backpackMask.enabled = false;
                GameManager.Instance().tutorialStore.Dispatch(new ShouldCloseInventory()); 
                break;
            case TutorialState.TutorialStage.ShouldCloseInventory:
                ShowMessage("Close Inventory", "Tap E or ESC to close your inventory");
                break;
            case TutorialState.TutorialStage.ClosedInventory:
                heldItemMask.enabled = false;
                inventoryCanvas.enabled = false; 
                EnablePlayer();
                Invoke(nameof(StartBlueprintFlow), 0.5f);
                break;
            case TutorialState.TutorialStage.ShouldOpenBlueprint:
                ShowMessage("Blueprints", "To view the mysterious blueprints, press the Q key");
                break; 
            case TutorialState.TutorialStage.InsideBlueprint:
                blueprintCanvas.enabled = true;
                primaryResourceMask.enabled = true;
                DisablePlayer();
                GameManager.Instance().uiStore.Subscribe(this);
                break; 
            case TutorialState.TutorialStage.HighlightFurnace:
                primaryResourceMask.enabled = false;
                furnaceMask.enabled = true;
                break;
            case TutorialState.TutorialStage.OpenFurnace:
                blueprintCanvas.enabled = false;
                furnaceMask.enabled = false;
                blueprintTemplateCanvas.enabled = true;
                componentsMask.enabled = true;
                break;
            case TutorialState.TutorialStage.CraftedFurnace:
                componentsMask.enabled = false;
                recipeMask.enabled = true;
                break;
            case TutorialState.TutorialStage.HighlightBlueprintNotes:
                recipeMask.enabled = false;
                notesMask.enabled = true;
                break;
            case TutorialState.TutorialStage.ReturnToProgression:
                notesMask.enabled = false;
                blueprintBackMask.enabled = true;
                break;
            case TutorialState.TutorialStage.ShouldCloseBlueprintTemplate:
                blueprintBackMask.enabled = false;
                blueprintTemplateCanvas.enabled = false;
                blueprintCanvas.enabled = true;
                GameManager.Instance().tutorialStore.Dispatch(new ShouldCloseBlueprint());
                break;
            case TutorialState.TutorialStage.ShouldCloseBlueprint:
                ShowMessage("Close Blueprint", "Tap Q or ESC to close the Blueprints");
                break;
            case TutorialState.TutorialStage.ClosedBlueprint:
                blueprintCanvas.enabled = false;
                heldItemCanvas.enabled = true;
                GameManager.Instance().uiStore.Dispatch(new OpenIntroUI());
                EnablePlayer();
                Invoke(nameof(StartGridFlow), 2);
                break;
            case TutorialState.TutorialStage.ShowHeldItemScroll:
                heldItemMask.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Scroll the scroll wheel to select an item to place";
                heldItemMask.enabled = true;
                heldItemCanvas.enabled = true;
                GameManager.Instance().heldItemStore.Subscribe(this);
                break;
            case TutorialState.TutorialStage.ScrolledHeldItem:
                heldItemMask.enabled = false;
                gridMask.enabled = true;
                GameManager.Instance().mapStore.Subscribe(this);
                break;
            case TutorialState.TutorialStage.PlacedFurnace:
                gridMask.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "To interact with a machine, point your cursor at it and right click";
                break; 
            case TutorialState.TutorialStage.ShouldOpenMachine:
                DisablePlayer();
                gridMask.enabled = false;
                machineCanvas.enabled = true;
                machineInventoryCanvas.enabled = true;
                machineInputMask.enabled = true;
                break;
            case TutorialState.TutorialStage.ShowMachineOutputSlots:
                machineInputMask.enabled = false;
                machineOutputMask.enabled = true;
                break;
            case TutorialState.TutorialStage.ShowAllMachine:
                machineOutputMask.enabled = false;
                machineGeneralMask.enabled = true;
                GameManager.Instance().machineStore.Subscribe(this);
                break;
            case TutorialState.TutorialStage.DidCraftInMachine:
                machineGeneralMask.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "Nice job! Now move the item back to your inventory...";
                break;
            case TutorialState.TutorialStage.DidMoveCraftToInventory:
                GameManager.Instance().uiStore.Unsubscribe(this);
                GameManager.Instance().machineStore.Unsubscribe(this);
                GameManager.Instance().inventoryStore.Unsubscribe(this);
                GameManager.Instance().heldItemStore.Unsubscribe(this);
                GameManager.Instance().mapStore.Unsubscribe(this);
                finishingCanvas.enabled = true;
                Time.timeScale = 1;
                Invoke(nameof(ToWorld), 6);
                break;
            default:
                Debug.Log("UNHANDLED CASE IN STATE DID UPDATE");
                break;
        }
    }

    public void StateDidUpdate(InventoryState state) {
        TutorialState.TutorialStage tutorialState = GameManager.Instance().tutorialStore.GetState().stage;
        
        if (tutorialState == TutorialState.TutorialStage.InsideInventory) {
            // Check have moved the wood
            if (state.inventoryContents[1].FindAll(location => location.hexID != 6).Count > 0) {
                GameManager.Instance().tutorialStore.Dispatch(new DidMoveInventoryItem());
            }
        } else if (tutorialState == TutorialState.TutorialStage.DidMoveInventoryItem) {
           // Check have more than 1 stack of wood
           if (state.inventoryContents[1].Count > 1) {
               foreach (InventoryEntry entry in mockBackpackContents) {
                   inventoryCanvas.gameObject.GetComponentInChildren<InventoryController>().backpackContents.Add(entry);
               }
               inventoryCanvas.gameObject.GetComponentInChildren<InventoryController>().SetBackpackState();
               GameManager.Instance().tutorialStore.Dispatch(new DidSplitInventoryItem());
           }
        } else if (tutorialState == TutorialState.TutorialStage.DidSplitInventoryItem) {
            // Check inventory for stone and clay
            if (state.inventoryContents.ContainsKey(2) && state.inventoryContents.ContainsKey(3)) {
                GameManager.Instance().tutorialStore.Dispatch(new DidCollectFromBackpack());
            }
        } else if (tutorialState == TutorialState.TutorialStage.OpenFurnace) {
            // Check inventory for crafted furnace
            if (state.inventoryContents.ContainsKey(11)) {
                GameManager.Instance().tutorialStore.Dispatch(new CraftedFurnace()); 
            }
        } else if (tutorialState == TutorialState.TutorialStage.DidCraftInMachine) {
            if (state.inventoryContents.ContainsKey(12)) {
                GameManager.Instance().tutorialStore.Dispatch(new DidMoveCraftToInventory());
            }
            
        }
    }

    public void StateDidUpdate(UIState state) {
        if (state.Selected == UIState.OpenUI.BlueprintTemplate) {
            // If tapped furnace AND are at right stage in tutorial
            if (state.SelectedBlueprintID == 11 && GameManager.Instance().tutorialStore.GetState().stage == TutorialState.TutorialStage.HighlightFurnace) {
                GameManager.Instance().tutorialStore.Dispatch(new OpenFurnaceBlueprint());
            } else {
                // Invalid click, go back to blueprint
                GameManager.Instance().uiStore.GetState().Selected = UIState.OpenUI.Blueprint;
            }
        }

        if (state.Selected == UIState.OpenUI.Blueprint) {
            if (GameManager.Instance().tutorialStore.GetState().stage == TutorialState.TutorialStage.ReturnToProgression) {
                GameManager.Instance().tutorialStore.Dispatch(new ShouldCloseBlueprintTemplate());
            } else if (GameManager.Instance().tutorialStore.GetState().stage != TutorialState.TutorialStage.InsideBlueprint) {
                GameManager.Instance().uiStore.Dispatch(new OpenBlueprintTemplateUI(11));
            }
        }

        if (state.Selected == UIState.OpenUI.Machine && GameManager.Instance().tutorialStore.GetState().stage == TutorialState.TutorialStage.PlacedFurnace) {
            GameManager.Instance().tutorialStore.Dispatch(new ShouldOpenMachine());
        }
        
    }

    public void StateDidUpdate(HeldItemState state) {
        if (GameManager.Instance().tutorialStore.GetState().stage == TutorialState.TutorialStage.ShowHeldItemScroll) {
            if (scrollCount == 5) {
                GameManager.Instance().tutorialStore.Dispatch(new DidScrollHeldItem());
            }
            scrollCount++;
        }
    }

    public void StateDidUpdate(MapState state) {
        if (GameManager.Instance().tutorialStore.GetState().stage == TutorialState.TutorialStage.ScrolledHeldItem) {
            foreach (MapObject mapObject in state.GetObjects().Values) {
                if (mapObject.GetID() == 11) {
                    GameManager.Instance().tutorialStore.Dispatch(new DidPlaceFurnace());
                } 
            }
        }
    }

    public void StateDidUpdate(MachineState state) {
        if (GameManager.Instance().tutorialStore.GetState().stage == TutorialState.TutorialStage.ShowAllMachine) {
            foreach (Machine machine in state.grid.Values) {
                if (machine.id == 11 && machine.output.IsPresent()) {
                    GameManager.Instance().tutorialStore.Dispatch(new DidCraftInMachine());
                }
            }
        }
    }

    private void ShowMessage(string title, string description) {
        alertCanvas.GetComponentInChildren<AlertController>().SetAlert(title, description);
        alertCanvas.GetComponentInChildren<AlertController>().ShowAlertView();
    }

    private void StartInventoryFlow() {
        GameManager.Instance().tutorialStore.Dispatch(new ShouldOpenInventory());
    }
    
    private void StartBlueprintFlow() {
        GameManager.Instance().tutorialStore.Dispatch(new ShouldOpenBlueprint());
    }

    private void StartGridFlow() {
        GameManager.Instance().tutorialStore.Dispatch(new ShowHeldItemScroll());
    }

    private void ToWorld() {
        GameManager.Instance().ResetGame();
        GameManager.Instance().uiStore.Dispatch(new OpenPlayingUI());
        SceneManager.LoadScene(SceneMapping.World);
    }

    private void DisablePlayer() {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        cursor.enabled = false;
        GameObject.Find("Player").GetComponent<PlayerMoveController>().enabled = false;
        GameObject.Find("PlayerCamera").GetComponent<PlayerLookController>().enabled = false;
    }
    
    private void EnablePlayer() {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cursor.enabled = true;
        GameObject.Find("Player").GetComponent<PlayerMoveController>().enabled = true;
        GameObject.Find("PlayerCamera").GetComponent<PlayerLookController>().enabled = true;
    }
}
