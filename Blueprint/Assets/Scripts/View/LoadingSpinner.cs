using UnityEngine;

namespace View {
    public class LoadingSpinner : MonoBehaviour {
        private RectTransform transform;
        private float rotateSpeed = 300f;

        void Start() {
            transform = GetComponent<RectTransform>();
        }

        void Update() {
            transform.Rotate(0f, 0f, -rotateSpeed * Time.deltaTime);
        }
    }
}
