using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Model {
    public class MapState {
        private Dictionary<Vector2, bool> grid;

        public MapState() {
            grid = new Dictionary<Vector2, bool>(); 

            grid.Add(new Vector2(0, 0), true);
            grid.Add(new Vector2(1, 1), true);
            grid.Add(new Vector2(2, -1), true);
        }

        public void addObject(Vector2 position) {
            grid[position] = true;
        }

        public void removeObject(Vector2 position) {
            grid.Remove(position);
        }

        public Dictionary<Vector2, bool> getObjects() {
            return grid;
        }

    }
}