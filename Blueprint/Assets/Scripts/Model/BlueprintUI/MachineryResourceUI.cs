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

        void IBlueprintUIMode.OnInitialize() {
        }

        void IBlueprintUIMode.OnShow() {
            BlueprintUITools.NewMachine(BlueprintUICanvas.transform, CanvasObjects, 0.175f, 0.7f, 0.1f, 13, 11, 12, 1, 4, 1);
            BlueprintUITools.NewMachine(BlueprintUICanvas.transform, CanvasObjects, 0.475f, 0.7f, 0.1f, 12, 11, 1, 1);
            BlueprintUITools.NewMachine(BlueprintUICanvas.transform, CanvasObjects, 0.775f, 0.7f, 0.1f, 30, 29, 18, 1, 5, 1);

            BlueprintUITools.NewMachine(BlueprintUICanvas.transform, CanvasObjects, 0.325f, 0.5f, 0.1f, 15, 11, 4, 1);
            BlueprintUITools.NewMachine(BlueprintUICanvas.transform, CanvasObjects, 0.625f, 0.5f, 0.1f, 16, 11, 8, 1);

            BlueprintUITools.NewMachine(BlueprintUICanvas.transform, CanvasObjects, 0.175f, 0.3f, 0.1f, 17, 11, 9, 1);
            BlueprintUITools.NewMachine(BlueprintUICanvas.transform, CanvasObjects, 0.475f, 0.3f, 0.1f, 14, 11, 5, 1);
            BlueprintUITools.NewMachine(BlueprintUICanvas.transform, CanvasObjects, 0.775f, 0.3f, 0.1f, 21, 20, 14, 1);

            BlueprintUITools.NewMachine(BlueprintUICanvas.transform, CanvasObjects, 0.475f, 0.1f, 0.1f, 27, 26, 13, 10);

            BlueprintUITools.CreateInfoText(BlueprintUICanvas.transform, CanvasObjects,
                "Craft these using the highlighted machines you've placed on the map");
        }

        void IBlueprintUIMode.OnHide() {
        }
    }
}
