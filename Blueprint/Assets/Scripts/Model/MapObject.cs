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
        [SerializeField] private List<int> input;
        [SerializeField] private List<int> output;

        public MapObject(int id) {
            this.id = id;
            this.input = new List<int>();
            this.output = new List<int>();
        }

        public int GetID() {
            return id;
        }
    }
}
