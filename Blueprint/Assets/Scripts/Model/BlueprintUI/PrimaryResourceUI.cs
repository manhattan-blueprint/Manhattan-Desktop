using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Controller;

namespace Model.BlueprintUI {
    public class PrimaryResourceUI : IBlueprintUIMode {
        public BlueprintUIController blueprintUIController { get; set; }
        public Canvas BlueprintUICanvas { get; set; }
        public GameObject Title { get; set; }
        public String TitleStr { get; set; }
        public List<GameObject> CanvasObjects { get; set; }
        public ManhattanAnimation animationManager { get; set; }

        void IBlueprintUIMode.OnInitialize() {
        }

        void IBlueprintUIMode.OnShow() {
            BlueprintUITools.NewResource(BlueprintUICanvas.transform, CanvasObjects, 0.3f, 0.7f, 0.2f, 1);
            BlueprintUITools.NewResource(BlueprintUICanvas.transform, CanvasObjects, 0.5f, 0.7f, 0.2f, 2);
            BlueprintUITools.NewResource(BlueprintUICanvas.transform, CanvasObjects, 0.7f, 0.7f, 0.2f, 3);

            BlueprintUITools.NewResource(BlueprintUICanvas.transform, CanvasObjects, 0.2f, 0.5f, 0.2f, 4);
            BlueprintUITools.NewResource(BlueprintUICanvas.transform, CanvasObjects, 0.4f, 0.5f, 0.2f, 5);
            BlueprintUITools.NewResource(BlueprintUICanvas.transform, CanvasObjects, 0.6f, 0.5f, 0.2f, 6);
            BlueprintUITools.NewResource(BlueprintUICanvas.transform, CanvasObjects, 0.8f, 0.5f, 0.2f, 7);

            BlueprintUITools.NewResource(BlueprintUICanvas.transform, CanvasObjects, 0.3f, 0.3f, 0.2f, 8);
            BlueprintUITools.NewResource(BlueprintUICanvas.transform, CanvasObjects, 0.5f, 0.3f, 0.2f, 9);
            BlueprintUITools.NewResource(BlueprintUICanvas.transform, CanvasObjects, 0.7f, 0.3f, 0.2f, 10);
        }

        void IBlueprintUIMode.OnHide() {

        }

    }
}
