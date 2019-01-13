using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controller {
    public class PlacementController : MonoBehaviour {
        [SerializeField] private GameObject item;
        [SerializeField] private GameObject tempParent;
        [SerializeField] private Transform guide;
        private bool moved;

        private void Start() {
            item.GetComponent<Rigidbody>().useGravity = true;
        }

        // TODO: Notify object that is has been dropped from inventory
        // TODO: Notify holding object that this object has been dropped
        // TODO: Notify holding object that object has been placed
        private void OnMouseDown() {
            float dist = Vector3.Distance(item.transform.position, guide.transform.position);
            if (dist < 2) {
                moved = true;
                item.GetComponent<Rigidbody>().useGravity = false;
                item.GetComponent<Rigidbody>().isKinematic = true;
                item.transform.position = guide.transform.position;
                item.transform.parent = tempParent.transform;
            }
        }

        private void OnMouseUp() {
            if (moved) {
                item.GetComponent<Rigidbody>().useGravity = true;
                item.GetComponent<Rigidbody>().isKinematic = false;
                item.transform.parent = null;
                item.transform.position = guide.transform.position;
                moved = !moved;
            }
        }
    }
}