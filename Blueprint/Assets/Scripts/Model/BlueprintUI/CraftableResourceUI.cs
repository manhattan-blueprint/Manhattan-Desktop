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
            BlueprintUITools.NewCraftable(BlueprintUICanvas.transform, CanvasObjects, 0.25f, 0.7f, 0.12f, 11, 2, 3);
            BlueprintUITools.NewCraftable(BlueprintUICanvas.transform, CanvasObjects, 0.35f, 0.45f, 0.12f, 20, 11, 13);
            BlueprintUITools.NewCraftable(BlueprintUICanvas.transform, CanvasObjects, 0.25f, 0.2f, 0.12f, 26, 19, 23, 22);

            BlueprintUITools.NewCraftable(BlueprintUICanvas.transform, CanvasObjects, 0.6f, 0.7f, 0.12f, 22, 21, 6);
            BlueprintUITools.NewCraftable(BlueprintUICanvas.transform, CanvasObjects, 0.7f, 0.45f, 0.12f, 29, 19, 22, 7);
            BlueprintUITools.NewCraftable(BlueprintUICanvas.transform, CanvasObjects, 0.6f, 0.2f, 0.12f, 25, 19, 16, 17);

            BlueprintUITools.CreateInfoText(BlueprintUICanvas.transform, CanvasObjects,
                "Craft these when you have the required resources (ʘᗩʘ')");
        }

        void IBlueprintUIMode.OnHide() {
        }
    }
}
