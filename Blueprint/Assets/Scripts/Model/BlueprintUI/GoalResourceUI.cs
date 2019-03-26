using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Controller;

namespace Model.BlueprintUI {
    public class GoalResourceUI : IBlueprintUIMode {
        public BlueprintUIController blueprintUIController { get; set; }
        public Canvas BlueprintUICanvas { get; set; }
        public GameObject Title { get; set; }
        public String TitleStr { get; set; }
        public List<GameObject> CanvasObjects { get; set; }

        void IBlueprintUIMode.OnInitialize() {
        }

        void IBlueprintUIMode.OnShow() {
            BlueprintUITools.NewGoal(BlueprintUICanvas.transform, CanvasObjects, 0.5f, 0.4f, 0.25f, 32, 28, 30, 31);

            BlueprintUITools.CreateInfoText(BlueprintUICanvas.transform, CanvasObjects,
                "Phone home! Place the required items inside the central frame to win");
        }

        void IBlueprintUIMode.OnHide() {
        }
    }
}
