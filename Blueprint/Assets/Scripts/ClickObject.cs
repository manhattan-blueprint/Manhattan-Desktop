
using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.UI;

// Pick up items
public class ClickObject : MonoBehaviour {

    private RaycastHit hit;
    private Text txt;
    private Inventory inventory;
    private InventoryItem focus;
    private const int RightButton = 1;
    private const int LeftButton = 0;

    public float maxDistance;
    private float timer;
    public GameObject itemButton;
    private GameObject dropButton;
    public Transform cube;
    public Transform cubeLarge;
    public Transform capsule;
    public Transform machinery;
    private string index;
    private GenerateHex generateHex;
    private GameObject drop;
    private bool holdInitiated;
    private const float holdLength = 1.0f;

    void Start() {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        generateHex = GameObject.FindGameObjectWithTag("Map").GetComponent<GenerateHex>();
        timer = 0.0f;
        holdInitiated = false;
    }

    private void SetFocus(InventoryItem newFocus) {
        focus = newFocus;
    }
    
    void Update() {
        if (Input.GetMouseButton(LeftButton) && timer > holdLength && holdInitiated) {
            Debug.Log("Hello, you held the button down for longer than two seconds.");
            // Cast ray from the cursor through the centre of the viewport (what's the mouse hovering over?)
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
            hit = new RaycastHit();

            // If a GameObject is hit
            if (!Physics.Raycast(ray, out hit)) return;
            SetFocus(hit.collider.GetComponent<InventoryItem>());

            int nextSlot = inventory.GetNextFreeSlot(hit.collider.GetComponent<InventoryItem>());
            float dist = Vector3.Distance(hit.transform.position, Camera.main.transform.position);

            if (focus != null && dist < maxDistance) {
                // This is not great, help meh fix it pls
                Renderer rend = hit.collider.GetComponent<Renderer>();
                Highlight hi = hit.collider.GetComponent<Highlight>();
                rend.material.color = hi.tempColor;
                // end zone

                // Add to inventory object
                inventory.AddItem(focus);
                // Set to make access unique
                itemButton.name = "InventoryItemSlot " + nextSlot;
                itemButton.GetComponent<Text>().text = hit.transform.gameObject.name;

                // Make game world object invisible and collider inactive
                hit.transform.gameObject.SetActive(false);

                // Create a slot with text in inventory window, or update quantity bracket
                if (inventory.itemSlots[nextSlot].transform.childCount < 2) {
                    Instantiate(itemButton, inventory.itemSlots[nextSlot].transform, false);
                }
                else {
                    GameObject.Find("InventoryItemSlot " + nextSlot + "(Clone)").GetComponentInChildren<Text>().text =
                        focus.name + " (" + inventory.GetItems()[nextSlot].GetQuantity() + ")";
                }

                // Change load order or UI elements for accessible hit-box
                index = "Button" + (nextSlot + 1);
                dropButton = GameObject.Find(index);
                itemButton.transform.SetSiblingIndex(0);
                dropButton.transform.SetSiblingIndex(1);
            }
            else {
                Debug.Log("No inventory items hit");
            }
            holdInitiated = false;
        }
        
        if (Input.GetMouseButtonDown(LeftButton)) {
            holdInitiated = true;
        }

        if (Input.GetMouseButton(LeftButton)) {
            timer += Time.deltaTime;
        }

        if (Input.GetMouseButtonUp(LeftButton)) {
            timer = 0.0f;
        }
        
        if (Input.GetMouseButtonDown(RightButton)) {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
            hit = new RaycastHit();

            // If a GameObject is hit
            if (!Physics.Raycast(ray, out hit)) return;
            Debug.Log("Hit location: " + hit.transform.position.x + ", " + hit.transform.position.z);
            SetFocus(hit.collider.GetComponent<InventoryItem>());
            generateHex.hexmap.PlaceOnGrid(hit.transform.position.x, hit.transform.position.z,
            Quaternion.Euler(0, 0, 0), GenerateHex.Resource.Machinery);
            // GameObject.Destroy(hit.transform.gameObject);
        }
    }
}
