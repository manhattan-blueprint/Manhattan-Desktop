using UnityEngine;
using UnityEngine.UI;
using Controller;
using Model;
using Model.Action;
using Model.Redux;
using Model.State;
using View;

/* Attached to PlayerCamera and controls mouse actions */
namespace Controller {
    public class MouseController: MonoBehaviour {
        public Transform cube;
        public Transform cubeLarge;
        public Transform capsule;
        public Transform machinery;
        private const float maxDistance = 10;
        private const float holdLength = 0.5f;
        private const int rightButton = 1;
        private const int leftButton = 0;

        private RaycastHit hit;
        private Text txt;
        private InventoryController inventory;
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

        void Update() {
            // Scrolling
            float threshold = 0f;
            float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
            if (scrollDelta > threshold) {
                // Scroll Up 
                GameManager.Instance().store.Dispatch(new RotateHeldItemForward());
            }
            else if (scrollDelta < -threshold) {
                // Scroll down
                GameManager.Instance().store.Dispatch(new RotateHeldItemBackward());
            }
            
            
            // Put down held item
            if (Input.GetMouseButtonDown(rightButton)) {
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
                hit = new RaycastHit();

                if (!Physics.Raycast(ray, out hit)) return;
                HexCell hc = hit.transform.gameObject.GetComponent<HexCell>();
                if (hc == null) return;
                GameManager.Instance().store.Dispatch(new RemoveHeldItem(hc.getPosition()));
            } 
            
            
            // Pick up item
            if (Input.GetMouseButton(leftButton) && timer > holdLength && holdInitiated) {
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
                hit = new RaycastHit();
                
                if (!Physics.Raycast(ray, out hit)) return;
                Placeable p = hit.transform.gameObject.GetComponent<Placeable>();
                if (p == null) return;
                HexCell hc = p.transform.parent.gameObject.GetComponent<HexCell>();
                if (hc == null) return;
                GameManager.Instance().store.Dispatch(new CollectItem(hc.getPosition()));
                holdInitiated = false;
            } else if (Input.GetMouseButtonDown(leftButton)) {
                holdInitiated = true;
            } else if (Input.GetMouseButton(leftButton)) {
                timer += Time.deltaTime;
            } else if (Input.GetMouseButtonUp(leftButton)) {
                timer = 0.0f;
            }
        }
    }
}
