using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View {
    public class Highlight : MonoBehaviour {
        [SerializeField] private Color highlightColor;
        [SerializeField] private bool holdable;
        [SerializeField] public Color tempColor;
        private Renderer rend;

        void Start () {
            rend = GetComponent<Renderer>();
        }
        void OnMouseEnter() {
            tempColor = rend.material.color;
            rend.material.color = highlightColor;
        }

        private void OnMouseDown() {
            if (!holdable) return;
            rend.material.color = Color.yellow;
        }

        void OnMouseExit() {
            rend.material.color = tempColor;
        }
    }
}
