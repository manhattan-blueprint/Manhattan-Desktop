using System;
using UnityEngine;
using UnityEngine.UI;
using Controller;
using Model;
using Model.Action;
using Model.Redux;
using Model.State;
using UnityEditor;
using View;

/* Attached to PlayerCamera and controls mouse actions */
namespace Controller {
    public class MouseController: MonoBehaviour {
        public Transform cube;
        public Transform cubeLarge;
        public Transform capsule;
        public Transform machinery;
        private const float maxDistance = 7;
        private const float holdLength = 0.5f;
        private const int leftButton = 0;
        private const int rightButton = 1;
        private const int middleButton = 2;

        private RaycastHit hit;
        private Text txt;
        private float timer;
        private string index;
        private HexMapController hexMapController;
        private GameObject drop;
        private bool holdInitiated;
        private SoundController soundController;

        void Start() {
            hexMapController = GameObject.FindGameObjectWithTag("Map").GetComponent<HexMapController>();
            timer = 0.0f;
            holdInitiated = false;
            soundController = GameObject.Find("SoundController").GetComponent<SoundController>();
        }

        void Update() {
            // Scrolling
            float threshold = 0f;
            float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
            if (scrollDelta > threshold) {
                // Scroll Up
                GameManager.Instance().heldItemStore.Dispatch(new RotateHeldItemBackward());
            } else if (scrollDelta < -threshold) {
                // Scroll down
                GameManager.Instance().heldItemStore.Dispatch(new RotateHeldItemForward());
            }


            // Put down held item
            if (Input.GetMouseButtonDown(rightButton) && 
                GameManager.Instance().uiStore.GetState().Selected != UIState.OpenUI.Machine && 
                GameManager.Instance().uiStore.GetState().Selected != UIState.OpenUI.Inventory) {
                
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
                hit = new RaycastHit();
                if (!Physics.Raycast(ray, out hit)) return;

                // Check player is not too far away
                float distance = Vector3.Distance(hit.transform.position, transform.position);
                if (distance > maxDistance) return;

                // If we hit a machine, go to machine UI
                MachinePlaceable mp = hit.transform.gameObject.GetComponent<MachinePlaceable>();
                if (mp != null) {
                    // Calculate where the machine is
                    HexCell parentHex = mp.transform.parent.gameObject.GetComponent<HexCell>();
                    GameManager.Instance().uiStore.Dispatch(new OpenMachineUI(parentHex.GetPosition()));
                    return;
                }

                // Otherwise try and place an object in that spot
                HexCell hc = hit.transform.parent.gameObject.GetComponent<HexCell>();
                if (hc != null) {
                    GameManager.Instance().inventoryStore.Dispatch(new RemoveHeldItem(hc.GetPosition()));
                }
            }


            // Pick up item
            if (Input.GetMouseButton(leftButton) && timer > holdLength && holdInitiated) {
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
                hit = new RaycastHit();

                if (!Physics.Raycast(ray, out hit)) return;

                // Check player is not too far away
                float distance = Vector3.Distance(hit.transform.position, transform.position);
                if (distance > maxDistance) return;

                Placeable p = hit.transform.gameObject.GetComponent<Placeable>();
                if (p == null) return;
                HexCell hc = p.transform.parent.gameObject.GetComponent<HexCell>();
                if (hc == null) return;

                // Play sound corresponding to item
                soundController.PlayPickupSound(GameManager.Instance().mapStore.GetState().GetObjects()[hc.GetPosition()].GetID());

                GameManager.Instance().mapStore.Dispatch(new CollectItem(hc.GetPosition()));
                holdInitiated = false;

                if (p is MachinePlaceable) {
                    GameManager.Instance().machineStore.Dispatch(new RemoveMachine(hc.GetPosition()));
                }

            } else if (Input.GetMouseButtonDown(leftButton)) {
                holdInitiated = true;
            } else if (Input.GetMouseButton(leftButton)) {
                timer += Time.deltaTime;
            } else if (Input.GetMouseButtonUp(leftButton)) {
                timer = 0.0f;
            }
            
            // Rotate item
            if (Input.GetMouseButtonDown(middleButton)) {
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
                hit = new RaycastHit();

                if (!Physics.Raycast(ray, out hit)) return;

                // Check player is not too far away
                float distance = Vector3.Distance(hit.transform.position, transform.position);
                if (distance > maxDistance) return;

                Placeable p = hit.transform.gameObject.GetComponent<Placeable>();
                if (p == null) return;
                HexCell hc = p.transform.parent.gameObject.GetComponent<HexCell>();
                if (hc == null) return;
                
                p.transform.localRotation = Quaternion.Euler(0, p.transform.localEulerAngles.y + 60, 0);
                GameManager.Instance().mapStore.Dispatch(new RotateItem(hc.GetPosition()));
            }
        }
    }
}
