using Controller;
using Model.Action;
using Model.Redux;
using Model.State;
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

public class TutorialController : MonoBehaviour, Subscriber<TutorialState>, Subscriber<InventoryState> {
    [SerializeField] private Image cursor;
    [SerializeField] private SVGImage rmb;
    [SerializeField] private Canvas alertCanvas;
    [SerializeField] private Canvas inventoryCanvas;
    [SerializeField] private Canvas machineCanvas;
    [SerializeField] private Canvas machineInventoryCanvas;
    [SerializeField] private Canvas inventoryMask;
    [SerializeField] private Canvas backpackMask;

    void Start() {
        rmb.enabled = false;
        alertCanvas.enabled = false;
        inventoryCanvas.enabled = false;
        machineCanvas.enabled = false;
        machineInventoryCanvas.enabled = false;
        inventoryMask.enabled = false;
        backpackMask.enabled = false;
        GameManager.Instance().tutorialStore.Subscribe(this);
        
        // Give some mock data
        GameManager.Instance().inventoryStore.Dispatch(new AddItemToInventoryAtHex(1, 10, "Wood", 6));
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
               inventoryMask.enabled = false; 
               backpackMask.enabled = true;
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

    private void DisablePlayer() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        cursor.enabled = false;
        GameObject.Find("Player").GetComponent<PlayerMoveController>().enabled = false;
        GameObject.Find("PlayerCamera").GetComponent<PlayerLookController>().enabled = false;
    }
}
