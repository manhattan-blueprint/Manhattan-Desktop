using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Controller;

namespace Model.BlueprintUI {
    public class MachineryResourceUI : IBlueprintUIMode {
        public BlueprintUIController blueprintUIController { get; set; }
        public Canvas BlueprintUICanvas { get; set; }
        public GameObject Title { get; set; }
        public String TitleStr { get; set; }
        public List<GameObject> CanvasObjects { get; set; }
        public ManhattanAnimation animationManager { get; set; }

        void IBlueprintUIMode.OnInitialize() {
        }

        void IBlueprintUIMode.OnShow() {
        }

        void IBlueprintUIMode.OnHide() {
        }
    }
}
