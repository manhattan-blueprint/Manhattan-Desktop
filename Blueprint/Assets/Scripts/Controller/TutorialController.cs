using System.Collections.Generic;
using Controller;
using Model;
using Model.Action;
using Model.Redux;
using Model.State;
using Service.Response;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Tutorial Summary
// - Player starts in the world
// - Must press one movement key, then a 10s timer is started
// - After the timer expires, shown a message about inventory
// - Opens inventory
// - Moves Item
// - Splits item 
// - Collects resources from backpack

public class TutorialController : MonoBehaviour, Subscriber<TutorialState>, Subscriber<InventoryState>, Subscriber<UIState> {
    [SerializeField] private Image cursor;
    [SerializeField] private SVGImage rmb;
    [SerializeField] private Canvas alertCanvas;
    [SerializeField] private Canvas inventoryCanvas;
    [SerializeField] private Canvas machineCanvas;
    [SerializeField] private Canvas machineInventoryCanvas;
    [SerializeField] private Canvas blueprintCanvas;
    [SerializeField] private Canvas blueprintTemplateCanvas;
    [SerializeField] private Canvas inventoryMask;
    [SerializeField] private Canvas backpackMask;
    [SerializeField] private Canvas heldItemMask;

    private List<InventoryEntry> mockBackpackContents;

    void Start() {
        rmb.enabled = false;
        alertCanvas.enabled = false;
        inventoryCanvas.enabled = false;
        machineCanvas.enabled = false;
        machineInventoryCanvas.enabled = false;
        blueprintCanvas.enabled = false;
        blueprintTemplateCanvas.enabled = false;
        inventoryMask.enabled = false;
        backpackMask.enabled = false;
        heldItemMask.enabled = false;
        inventoryCanvas.gameObject.GetComponent<InventoryController>().backpackButton.enabled = false;
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
        if (GameManager.Instance().tutorialStore.GetState().stage == TutorialState.TutorialStage.ShouldOpenInventory && !alertCanvas.enabled) {
            if (Input.GetKeyDown(KeyMapping.Inventory)) {
                GameManager.Instance().tutorialStore.Dispatch(new InsideInventory());
            }  
        }
        
        // If waiting to close inventory, check they have done so
        if (GameManager.Instance().tutorialStore.GetState().stage == TutorialState.TutorialStage.ShouldCloseInventory && !alertCanvas.enabled) {
            if (Input.GetKeyDown(KeyMapping.Inventory) || Input.GetKeyDown(KeyCode.Escape)) {
                GameManager.Instance().tutorialStore.Dispatch(new ClosedInventory());
            }
        }
        
        // If waiting to open blueprint, check they have done so
        if (GameManager.Instance().tutorialStore.GetState().stage == TutorialState.TutorialStage.ShouldOpenBlueprint && !alertCanvas.enabled) {
            if (Input.GetKeyDown(KeyMapping.Blueprint)) {
                GameManager.Instance().tutorialStore.Dispatch(new InsideBlueprint());
            }
        }
    }

    public void StateDidUpdate(TutorialState state) {
        switch (state.stage) {
            case TutorialState.TutorialStage.Welcome:
                // Show welcome message
                ShowMessage("Welcome to Blueprint!", "To move around the world, use:\n W - Forwards\nS - Backwards\nA - Left\nD - Right\n\n You can use the mouse to move the direction you are facing.");
                break;
            case TutorialState.TutorialStage.Moving:
                // Allow some exploration before continuing
                Invoke(nameof(StartInventoryFlow), 10);
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
                heldItemMask.enabled = true;
                Invoke(nameof(FinishInventoryFlow), 3);
                break;
            case TutorialState.TutorialStage.ShouldCloseInventory:
                ShowMessage("Close Inventory", "Tap E or ESC to close your inventory");
                break;
            case TutorialState.TutorialStage.ClosedInventory:
                heldItemMask.enabled = false;
                inventoryCanvas.enabled = false; 
                EnablePlayer();
                Invoke(nameof(StartBlueprintFlow), 10);
                break;
            case TutorialState.TutorialStage.ShouldOpenBlueprint:
                ShowMessage("Blueprints", "To view the blueprints mysteriously left around the beacon, press the Q key");
                break; 
            case TutorialState.TutorialStage.InsideBlueprint:
                GameManager.Instance().uiStore.Subscribe(this);
                blueprintCanvas.enabled = true;
                DisablePlayer();
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
        }
    }

    public void StateDidUpdate(UIState state) {
        if (state.Selected == UIState.OpenUI.BlueprintTemplate) {
            Debug.Log("Complete");
        }
    }

    private void ShowMessage(string title, string description) {
        alertCanvas.GetComponentInChildren<AlertController>().SetAlert(title, description);
        alertCanvas.GetComponentInChildren<AlertController>().ShowAlertView();
    }

    private void StartInventoryFlow() {
        GameManager.Instance().tutorialStore.Dispatch(new ShouldOpenInventory());
    }
    
    private void FinishInventoryFlow() {
        GameManager.Instance().tutorialStore.Dispatch(new ShouldCloseInventory());
    }
    
    private void StartBlueprintFlow() {
        GameManager.Instance().tutorialStore.Dispatch(new ShouldOpenBlueprint());
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
