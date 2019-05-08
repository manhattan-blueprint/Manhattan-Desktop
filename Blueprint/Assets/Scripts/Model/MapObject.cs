﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Controller;
using Model;

namespace Model {
    [Serializable]
    public class MapObject {
        [SerializeField] private int id;
        [SerializeField] private float rotation;

        public MapObject(int id, float rotation) {
            this.id = id;
            this.rotation = rotation;
        }

        public int GetID() {
            return id;
        }

        public float GetRotation() {
            return rotation;
        }

        public void Rotate() {
            rotation = (rotation + 60) % 360;
        }
    }
}
