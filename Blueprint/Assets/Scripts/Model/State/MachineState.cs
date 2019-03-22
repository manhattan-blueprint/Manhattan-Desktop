using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Model.State {
    public class MachineState {
        public Dictionary<Vector2, Machine> grid;

        public MachineState() {
            grid = new Dictionary<Vector2, Machine>(); 
        }
    }
}