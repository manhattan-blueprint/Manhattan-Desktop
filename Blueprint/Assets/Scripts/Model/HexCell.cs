using System.Numerics;
using System.Runtime.InteropServices;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Model {
    public class HexCell: MonoBehaviour {
        public Vector2 position;

        public void setPosition(Vector2 position) {
            this.position = position;
        }

        public Vector2 getPosition() {
            return position;
        }

    }
}