using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Controller;

namespace Model.BlueprintUI {
    public class CraftableResourceUI : IBlueprintUIMode {
        public BlueprintUIController blueprintUIController { get; set; }
        public Canvas BlueprintUICanvas { get; set; }
        public GameObject Title { get; set; }
        public String TitleStr { get; set; }
        public List<GameObject> CanvasObjects { get; set; }
        public ManhattanAnimation animationManager { get; set; }

        void IBlueprintUIMode.OnInitialize() {
        }

        void IBlueprintUIMode.OnShow() {
            BlueprintUITools.NewCraftable(BlueprintUICanvas.transform, CanvasObjects, 0.225f, 0.7f, 0.12f, 11, 2, 4, 3, 4);
            BlueprintUITools.NewCraftable(BlueprintUICanvas.transform, CanvasObjects, 0.325f, 0.45f, 0.12f, 20, 11, 1, 13, 2);
            BlueprintUITools.NewCraftable(BlueprintUICanvas.transform, CanvasObjects, 0.225f, 0.2f, 0.12f, 26, 19, 1, 23, 1, 22, 2);

            BlueprintUITools.NewCraftable(BlueprintUICanvas.transform, CanvasObjects, 0.575f, 0.7f, 0.12f, 22, 21, 1, 6, 1);
            BlueprintUITools.NewCraftable(BlueprintUICanvas.transform, CanvasObjects, 0.675f, 0.45f, 0.12f, 29, 19, 1, 22, 2, 7, 1);
            BlueprintUITools.NewCraftable(BlueprintUICanvas.transform, CanvasObjects, 0.575f, 0.2f, 0.12f, 25, 19, 1, 16, 6, 17, 6);

            BlueprintUITools.CreateInfoText(BlueprintUICanvas.transform, CanvasObjects,
                "Craft these when you have the required resources (ʘᗩʘ')");
        }

        void IBlueprintUIMode.OnHide() {
        }
    }
}
