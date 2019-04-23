
using System.Collections.Generic;
using UnityEngine;

namespace Utils {
    public static class Vector2Extensions {
        public static List<Vector2> HexNeighbours(this Vector2 position) {
             return new List<Vector2> {
                // Upper left
                new Vector2(position.x - 1, position.y + 1),
                // Upper right
                new Vector2(position.x, position.y + 1),
                // Right
                new Vector2(position.x + 1, position.y),
                // Lower right
                new Vector2(position.x + 1, position.y - 1),
                // Lower left
                new Vector2(position.x, position.y - 1),
                // Left
                new Vector2(position.x - 1, position.y)
            };
        }
    }
}