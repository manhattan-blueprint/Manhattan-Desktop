using System.Collections.Generic;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Model {
    public class HexCell: MonoBehaviour {
        private Vector2 position;

        public void SetPosition(Vector2 position) {
            this.position = position;
        }

        public Vector2 GetPosition() {
            return position;
        }

    }
}