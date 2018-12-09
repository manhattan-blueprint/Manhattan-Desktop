
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Pick up items
public class ClickObject : MonoBehaviour {

	private Vector3 direction = Vector3.forward;
	private RaycastHit hit;
	private float maxDistance = 10;
	private Text txt;
	private Inventory inventory;
	private InventoryItem focus;
    private const int LeftButton = 1;

    public GameObject itemButton;
    public Transform cube;
	public Transform cubeLarge;
	public Transform capsule;

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

			if (focus != null) {
				inventory.AddItem(focus);
				txt = itemButton.GetComponent<Text>();
				txt.text = hit.transform.gameObject.name;
				Instantiate(itemButton, inventory.itemSlots[nextSlot].transform, false);
				hit.transform.gameObject.SetActive(false);
			} else {
				Debug.Log("No inventory items hit");
			}
		}	
	}
}
