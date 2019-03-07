using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Model {
    public class HexCell: MonoBehaviour {
        public Vector3 position;

        public void setPosition(Vector3 position) {
            this.position = position;
        }

        public Vector3 getPosition() {
            return position;
        }
    }
}