using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Model {
    public class MapState {
        private Dictionary<Vector2, MapObject> grid;

        public MapState() {
            grid = new Dictionary<Vector2, MapObject>(); 

            grid.Add(new Vector2(0, 0), new MapObject(1));
            grid.Add(new Vector2(1, 1), new MapObject(7));
            grid.Add(new Vector2(2, -1), new MapObject(8));
        }

        public void addObject(Vector2 position, int id) {
            grid[position] = new MapObject(id);
        }

        public void removeObject(Vector2 position) {
            grid.Remove(position);
        }

        public Dictionary<Vector2, MapObject> getObjects() {
            return grid;
        }

    }
}