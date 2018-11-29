using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickObject : MonoBehaviour {

	public Vector3 direction = Vector3.forward;
	public RaycastHit hit;
	public float maxDistance = 10;
	public LayerMask layermask;
	public GameObject itemButton;
	Camera cam;
	int length;
	Text txt;

	private Inventory inventory;
	public InventoryItem focus;

	// Use this for initialization
	void Start () {
		inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
	}

	void SetFocus (InventoryItem newFocus) {
		focus = newFocus;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(1)){
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			hit = new RaycastHit();

			if (Physics.Raycast(ray, out hit)){
				Debug.Log("Game object hit: " + hit.transform.gameObject);
				SetFocus(hit.collider.GetComponent<InventoryItem>());
				int nextSlot = inventory.GetNextFreeSlot();
				Debug.Log("Next free slot: "+ nextSlot);

				if (focus != null && length <= nextSlot) {
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
}
