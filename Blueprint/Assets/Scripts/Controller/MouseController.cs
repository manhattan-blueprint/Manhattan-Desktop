using UnityEngine;
using UnityEngine.UI;
using Controller;
using Model;
using View;

/* Attached to PlayerCamera and controls mouse actions */
namespace Controller {
    public class MouseController: MonoBehaviour {
        public Transform cube;
        public Transform cubeLarge;
        public Transform capsule;
        public Transform machinery;
        private const float maxDistance = 10;
        private const float holdLength = 1.0f;
        private const int rightButton = 1;
        private const int leftButton = 0;

        private RaycastHit hit;
        private Text txt;
        private InventoryController inventory;
        private Interactable focus;
        private float timer;
        private string index;
        private HexMapController hexMapController;
        private GameObject drop;
        private bool holdInitiated;

        void Start() {
            inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryController>();
            hexMapController = GameObject.FindGameObjectWithTag("Map").GetComponent<HexMapController>();
            timer = 0.0f;
            holdInitiated = false;
        }

        private void SetFocus(Interactable newFocus) {
            focus = newFocus;
        }

        void Update() {
            // Pick up an object
            if (Input.GetMouseButton(leftButton) && timer > holdLength && holdInitiated) {
                collectItem();
            } else if (Input.GetMouseButtonDown(leftButton)) {
                holdInitiated = true;
            } else if (Input.GetMouseButton(leftButton)) {
                timer += Time.deltaTime;
            } else if (Input.GetMouseButtonUp(leftButton)) {
                timer = 0.0f;
            }

            // Place an object
            if (Input.GetMouseButtonDown(rightButton)) {
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
                hit = new RaycastHit();

                // If a GameObject is hit
                if (!Physics.Raycast(ray, out hit)) return;
                SetFocus(hit.collider.GetComponent<Interactable>());
                hexMapController.hexMap.PlaceOnGrid(hit.transform.position.x, hit.transform.position.z,
                Quaternion.Euler(0, 0, 0), Resources.Load(inventory.GetItemName(inventory.GetCurrentHeld())) as GameObject);
            }
        }

        private void collectItem() {
            // Cast ray from the cursor through the centre of the viewport (what's the mouse hovering over?)
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
            hit = new RaycastHit();

            // If a GameObject is hit
            if (!Physics.Raycast(ray, out hit)) return;
            SetFocus(hit.collider.GetComponent<Interactable>());

            float dist = Vector3.Distance(hit.transform.position, Camera.main.transform.position);

            if (focus != null && dist < maxDistance) {
                Renderer rend = hit.collider.GetComponent<Renderer>();
                Highlight hi = hit.collider.GetComponent<Highlight>();
                rend.material.color = hi.tempColor;
                
                inventory.CollectItem(focus, hit.transform.gameObject);
            } else {
                Debug.Log("No inventory items hit");
            }
            holdInitiated = false;
        }
    }
}
