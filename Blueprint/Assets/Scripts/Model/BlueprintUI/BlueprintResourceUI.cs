using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Controller;

namespace Model.BlueprintUI {
    public class BlueprintResourceUI : IBlueprintUIMode {
        public BlueprintUIController blueprintUIController { get; set; }
        public Canvas BlueprintUICanvas { get; set; }
        public GameObject Title { get; set; }
        public String TitleStr { get; set; }
        public List<GameObject> CanvasObjects { get; set; }

        void IBlueprintUIMode.OnInitialize() {
        }

        void IBlueprintUIMode.OnShow() {
            BlueprintUITools.NewBlueprint(BlueprintUICanvas.transform, CanvasObjects, 0.275f, 0.7f, 0.12f, 19, 13, 4);
            BlueprintUITools.NewBlueprint(BlueprintUICanvas.transform, CanvasObjects, 0.275f, 0.4f, 0.12f, 23, 21, 2);
            BlueprintUITools.NewBlueprint(BlueprintUICanvas.transform, CanvasObjects, 0.275f, 0.1f, 0.12f, 24, 15, 3, 23, 1);

            BlueprintUITools.NewBlueprint(BlueprintUICanvas.transform, CanvasObjects, 0.675f, 0.7f, 0.12f, 18, 17, 1, 16, 1);
            BlueprintUITools.NewBlueprint(BlueprintUICanvas.transform, CanvasObjects, 0.675f, 0.4f, 0.12f, 31, 30, 1, 10, 2);
            BlueprintUITools.NewBlueprint(BlueprintUICanvas.transform, CanvasObjects, 0.675f, 0.1f, 0.12f, 28, 27, 1, 24, 1, 18, 6);

            BlueprintUITools.CreateInfoText(BlueprintUICanvas.transform, CanvasObjects,
                "Click on the highlighted items to craft them once you have the required ingredients");
        }

        void IBlueprintUIMode.OnHide() {
        }
    }
}
