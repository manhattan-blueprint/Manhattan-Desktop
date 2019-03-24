using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Model.State {
    [Serializable]
    public class MapState {
        [SerializeField] private SerializableDictionary<Vector2, MapObject> grid;

        public MapState() {
            grid = new SerializableDictionary<Vector2, MapObject>();
        }

        public void addObject(Vector2 position, int id) {
            grid[position] = new MapObject(id);
        }

        public void removeObject(Vector2 position) {
            grid.Remove(position);
        }

        public SerializableDictionary<Vector2, MapObject> getObjects() {
            return grid;
        }

    }
}