
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
    private const int LeftButton = 1;

    public float maxDistance;
    public GameObject itemButton;
    public GameObject dropButton;
    public Transform cube;
    public Transform cubeLarge;
    public Transform capsule;
    public Transform machinery;
    private string index;

    // Use this for initialization
    void Start() {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    void SetFocus(InventoryItem newFocus) {
        focus = newFocus;
    }
    
    // Update is called once per frame
    void Update() {
        if (!Input.GetMouseButtonDown(LeftButton)) return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit)){
            SetFocus(hit.collider.GetComponent<InventoryItem>());
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
    }
}
