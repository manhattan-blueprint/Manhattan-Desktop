using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Model.State {
    public class TutorialState {
        public enum TutorialStage {
            Welcome, 
            Moving,
            ShouldOpenInventory,
            InsideInventory,
            DidMoveInventoryItem
        }
        public TutorialStage stage;

        public TutorialState() {
            this.stage = TutorialStage.Welcome;
        }
    }
}