using UnityEngine;

namespace Model {
    public class WirePath {
        public Vector2 start;
        public Vector2 end;

        public WirePath(Vector2 start, Vector2 end) {
            this.start = start;
            this.end = end;
        } 
    }
}