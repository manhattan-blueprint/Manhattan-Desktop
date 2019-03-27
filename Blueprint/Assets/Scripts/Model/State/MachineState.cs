using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Model.State {
    [Serializable]
    public class MachineState {
        public Dictionary<Vector2, Machine> grid;

        public MachineState() {
            grid = new Dictionary<Vector2, Machine>(); 
        }
    }
}