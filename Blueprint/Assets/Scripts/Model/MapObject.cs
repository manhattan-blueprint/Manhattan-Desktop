using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Controller;
using Model;

namespace Model {
    [Serializable]
    public class MapObject {
        [SerializeField] private int id;
        [SerializeField] private int rotation;

        public MapObject(int id, int rotation) {
            this.id = id;
            this.rotation = rotation;
        }

        public int GetID() {
            return id;
        }

        public int GetRotation() {
            return rotation;
        }

        public void Rotate() {
            rotation = (rotation + 60) % 360;
        }
    }
}
