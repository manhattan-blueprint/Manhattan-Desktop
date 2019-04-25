using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Model.State {
    [Serializable]
    public class MachineState {
        public Dictionary<Vector2, Machine> grid;
        public List<List<Vector2>> electricityPaths;

        public MachineState() {
            grid = new Dictionary<Vector2, Machine>(); 
            electricityPaths = new List<List<Vector2>>();
        }
    }
}