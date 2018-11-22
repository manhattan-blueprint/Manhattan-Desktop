using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickObject : MonoBehaviour {

	public Vector3 direction = Vector3.forward;
	public RaycastHit hit;
	public float maxDistance = 10;
	public LayerMask layermask;
	Camera cam;

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
				Debug.Log(hit.transform.gameObject);
				SetFocus(hit.collider.GetComponent<InventoryItem>());
				Debug.Log(inventory.itemSlots.Length);
				inventory.AddItem(focus);
			}	
		}
	}
}
