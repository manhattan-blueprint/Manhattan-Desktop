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
        public ManhattanAnimation animationManager { get; set; }

        void IBlueprintUIMode.OnInitialize() {
        }

        void IBlueprintUIMode.OnShow() {
            BlueprintUITools.NewCraftable(BlueprintUICanvas.transform, CanvasObjects, 0.225f, 0.7f, 0.12f, 19, 13);
            BlueprintUITools.NewCraftable(BlueprintUICanvas.transform, CanvasObjects, 0.325f, 0.45f, 0.12f, 23, 21);
            BlueprintUITools.NewCraftable(BlueprintUICanvas.transform, CanvasObjects, 0.225f, 0.2f, 0.12f, 24, 15, 23);

            BlueprintUITools.NewCraftable(BlueprintUICanvas.transform, CanvasObjects, 0.575f, 0.7f, 0.12f, 18, 17, 16);
            BlueprintUITools.NewCraftable(BlueprintUICanvas.transform, CanvasObjects, 0.675f, 0.45f, 0.12f, 31, 30, 10);
            BlueprintUITools.NewCraftable(BlueprintUICanvas.transform, CanvasObjects, 0.575f, 0.2f, 0.12f, 28, 27, 24, 18);

            BlueprintUITools.CreateInfoText(BlueprintUICanvas.transform, CanvasObjects,
                "Craft these to progress on the leaderboard (~˘▾˘)~");
        }

        void IBlueprintUIMode.OnHide() {
        }
    }
}
