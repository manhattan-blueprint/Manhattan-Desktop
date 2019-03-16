using System.Collections.Generic;
using System.Threading.Tasks;
using Controller;
using Model;
using Model.Action;
using Model.Redux;
using Model.State;
using Service;
using Service.Response;
using UnityEngine;
using UnityEngine.UI;

public class HeldItemController : MonoBehaviour, Subscriber<GameState> {
    
    private readonly float slotDimension = Screen.width / 15;
    private readonly float tileYOffset = 1.35f;
    private Dictionary<int, HeldItemSlotController> heldItemControllers;
    
    void Start() {
        heldItemControllers = new Dictionary<int, HeldItemSlotController>(); 
       
        generateHotbar(); 
        GameManager.Instance().store.Subscribe(this);
        
        // Since held item is shown before inventory, do the loading here
    }
    
    private void generateHotbar() {
        // 0.5 * slotdimension padding on x
        double hotbarCenterX = Screen.width - 2 * slotDimension;
        // 0.25 * slotdim padding on y
        double hotbarCenterY = 1.75 * slotDimension;
        
        // In order, starting from top left 
        // Draw 0 last so the highlight is on top 
        newSlot(1, (float) hotbarCenterX + slotDimension / 2, (float) hotbarCenterY + (slotDimension / tileYOffset));
        newSlot(2, (float) hotbarCenterX + slotDimension, (float) hotbarCenterY);
        newSlot(3, (float) hotbarCenterX + slotDimension / 2, (float) hotbarCenterY - (slotDimension / tileYOffset));
        newSlot(4, (float) hotbarCenterX - slotDimension / 2, (float) hotbarCenterY - (slotDimension / tileYOffset));
        newSlot(5, (float) hotbarCenterX - slotDimension, (float) hotbarCenterY);
        newSlot(0, (float) hotbarCenterX - slotDimension / 2, (float) hotbarCenterY + (slotDimension / tileYOffset));
    }
    
    private void newSlot(int id, float x, float y) {
        GameObject go = new GameObject("Hotbar" + id);
        go.transform.SetParent(this.transform);
        go.transform.position = new Vector2(x, y);
        
        // Background Image
        Image background = go.AddComponent<Image>();
        background.sprite = Resources.Load("inventory_slot", typeof(Sprite)) as Sprite;
        background.color = new Color32((byte)(background.color.r*255), (byte)(background.color.g*255), 
            (byte)(background.color.b*255), (byte)192);
        background.alphaHitTestMinimumThreshold = 0.5f;
            
        // Border
        GameObject svgChild = new GameObject("HotbarSVG" + id);
        svgChild.transform.SetParent(go.transform);
        svgChild.transform.position = new Vector2(x, y);
        SVGImage border = svgChild.AddComponent<SVGImage>();

        if (id == 0) {
            border.sprite = Resources.Load("slot_border_highlight", typeof(Sprite)) as Sprite;
        } else {
            border.sprite = Resources.Load("slot_border_outer", typeof(Sprite)) as Sprite;
        }

        (svgChild.transform as RectTransform).sizeDelta = new Vector2(slotDimension, slotDimension);
        (svgChild.transform as RectTransform).localScale = new Vector3(1.05f, 1.05f, 0.0f);
        
        HeldItemSlotController controller = go.AddComponent<HeldItemSlotController>();
        controller.id = id;
        controller.border = border;
        heldItemControllers.Add(id, controller);
        
        RectTransform rt = go.transform as RectTransform;
        rt.sizeDelta = new Vector2(slotDimension, slotDimension);
    }
    
    public string GetItemName(int id) {
        GameObjectsHandler goh = GameObjectsHandler.WithRemoteSchema();
        return goh.GameObjs.items[id - 1].name;
    }

    public void StateDidUpdate(GameState state) {
        // Disable canvas if in inventory 
        gameObject.GetComponent<Canvas>().enabled = state.uiState.Selected != UIState.OpenUI.Inventory;
       
        
        // Clear slots
        foreach (KeyValuePair<int, HeldItemSlotController> slot in heldItemControllers) {
            slot.Value.SetStoredItem(Optional<InventoryItem>.Empty());
            slot.Value.border.sprite = Resources.Load("slot_border_outer", typeof(Sprite)) as Sprite;
        }
            
        // Re-populate slots
        Dictionary<int, List<HexLocation>> inventoryContents = state.inventoryState.inventoryContents;
        
        foreach (KeyValuePair<int, List<HexLocation>> element in inventoryContents) {
            foreach(HexLocation loc in element.Value) {
                // Only consider held items
                if (loc.hexID > 5) continue;
                
                InventoryItem item = new InventoryItem("", element.Key, loc.quantity);
                heldItemControllers[loc.hexID].SetStoredItem(Optional<InventoryItem>.Of(item));
            } 
        }
        
        // Set current held
        heldItemControllers[state.inventoryState.indexOfHeldItem].border.sprite =
            Resources.Load("slot_border_highlight", typeof(Sprite)) as Sprite;
        heldItemControllers[state.inventoryState.indexOfHeldItem].transform.SetAsLastSibling();
    }
}
