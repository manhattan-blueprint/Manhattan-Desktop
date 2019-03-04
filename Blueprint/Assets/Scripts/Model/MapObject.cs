using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Controller;
using Model;

namespace Model {
    public class MapObject {
        private int id;
        private List<int> input;
        private List<int> output;

        public MapObject(int id) {
            this.id = id;
            this.input = new List<int>();
            this.output = new List<int>();
        }
    }
}
