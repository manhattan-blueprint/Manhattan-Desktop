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
            BlueprintUITools.NewMachine(BlueprintUICanvas.transform, CanvasObjects, 0.23f, 0.75f, 0.1f, 12, 11, 1, 1);
            BlueprintUITools.NewMachine(BlueprintUICanvas.transform, CanvasObjects, 0.37f, 0.6f, 0.1f, 13, 11, 12, 1);
            BlueprintUITools.NewMachine(BlueprintUICanvas.transform, CanvasObjects, 0.23f, 0.45f, 0.1f, 14, 11, 5, 1);
            BlueprintUITools.NewMachine(BlueprintUICanvas.transform, CanvasObjects, 0.37f, 0.3f, 0.1f, 15, 11, 4, 1);

            BlueprintUITools.NewMachine(BlueprintUICanvas.transform, CanvasObjects, 0.63f, 0.75f, 0.1f, 16, 11, 8, 1);
            BlueprintUITools.NewMachine(BlueprintUICanvas.transform, CanvasObjects, 0.77f, 0.6f, 0.1f, 17, 11, 9, 1);
            BlueprintUITools.NewMachine(BlueprintUICanvas.transform, CanvasObjects, 0.63f, 0.45f, 0.1f, 30, 29, 18, 1, 5, 1);
            BlueprintUITools.NewMachine(BlueprintUICanvas.transform, CanvasObjects, 0.77f, 0.3f, 0.1f, 21, 20, 5, 1);

            BlueprintUITools.NewMachine(BlueprintUICanvas.transform, CanvasObjects, 0.48f, 0.15f, 0.1f, 27, 26, 13, 10);

            BlueprintUITools.CreateInfoText(BlueprintUICanvas.transform, CanvasObjects,
                "Craft these using the shown machines ʘ‿ʘ");
        }

        void IBlueprintUIMode.OnHide() {
        }
    }
}
