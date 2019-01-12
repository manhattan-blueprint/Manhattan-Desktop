using UnityEngine;
using UnityEngine.UI;
using Controller;
using Model;

/* Attached to PlayerCamera and controls mouse actions */
namespace Controller {
    public class MouseController: MonoBehaviour {
        private RaycastHit hit;
        private Text txt;
        private InventoryController inventory;
        private Interactable focus;
        private const int RightButton = 1;
        private const int LeftButton = 0;

        public float maxDistance;
        private float timer;
        public Transform cube;
        public Transform cubeLarge;
        public Transform capsule;
        public Transform machinery;
        private string index;
        private HexMapController hexMapController;
        private GameObject drop;
        private bool holdInitiated;
        private const float holdLength = 1.0f;

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
            if (Input.GetMouseButton(LeftButton) && timer > holdLength && holdInitiated) {
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
                SetFocus(hit.collider.GetComponent<Interactable>());
                hexMapController.hexmap.PlaceOnGrid(hit.transform.position.x, hit.transform.position.z,
                Quaternion.Euler(0, 0, 0), MapResource.Machinery);
            }
        }
    }
}