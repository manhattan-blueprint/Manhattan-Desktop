using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Model {
    public class MapState {
        private Dictionary<Vector3, MapObject> grid;

        public MapState() {
            grid = new Dictionary<Vector3, MapObject>(); 

            grid.Add(new Vector3(0, 1, 0), new MapObject(1));
            grid.Add(new Vector3(1, 1, 1), new MapObject(7));
            grid.Add(new Vector3(2, 1, -1), new MapObject(8));
        }

        public void addObject(Vector3 position, int id) {
            grid[position] = new MapObject(id);
        }

        public void removeObject(Vector3 position) {
            grid.Remove(position);
        }

        public Dictionary<Vector3, MapObject> getObjects() {
            return grid;
        }

    }
}