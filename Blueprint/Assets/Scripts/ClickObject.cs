
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Pick up items
public class ClickObject : MonoBehaviour {

    private RaycastHit hit;
    private Text txt;
    private Inventory inventory;
    private InventoryItem focus;
    private const int RightButton = 1;

    public float maxDistance;
    public GameObject itemButton;
    public GameObject dropButton;
    public Transform cube;
    public Transform cubeLarge;
    public Transform capsule;
    public Transform machinery;
    private string index;
    private GenerateHex generateHex;
    private bool pickUp;
    private GameObject drop;

    void Start() {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        generateHex = GameObject.FindGameObjectWithTag("Map").GetComponent<GenerateHex>();
        pickUp = false;
    }

    void SetFocus(InventoryItem newFocus) {
        focus = newFocus;
    }
    
    void Update() {
        if (!Input.GetMouseButtonDown(RightButton)) return;
        // Cast ray from the cursor through the centre of the viewport (what's the mouse hovering over?)
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        hit = new RaycastHit();
        
        // If a GameObject is hit
        if (Physics.Raycast(ray, out hit)){
            SetFocus(hit.collider.GetComponent<InventoryItem>());
            
            if (pickUp) {
                int nextSlot = inventory.GetNextFreeSlot();
                float dist = Vector3.Distance(hit.transform.position, Camera.main.transform.position);

                if (focus != null && dist < maxDistance) {
                    inventory.AddItem(focus);
                    txt = itemButton.GetComponent<Text>();
                    txt.text = hit.transform.gameObject.name;
                    hit.transform.gameObject.SetActive(false);
                    Instantiate(itemButton, inventory.itemSlots[nextSlot].transform, false);
                    index = "Button" + (nextSlot + 1);
                    dropButton = GameObject.Find(index);
                    itemButton.transform.SetSiblingIndex(0);
                    dropButton.transform.SetSiblingIndex(1);
                } else {
                    Debug.Log("No inventory items hit");
                }
            }
            else {
                generateHex.hexmap.PlaceOnGrid((float) hit.transform.position.x, (float) hit.transform.position.z, Quaternion.Euler(0, 0, 0), GenerateHex.Resource.Machinery);
                // GameObject.Destroy(hit.transform.gameObject);
            }
            
        }   
    }
}
