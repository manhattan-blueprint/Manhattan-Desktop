using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Model.BlueprintUI {
    public class PrimaryResourceUI : IBlueprintUIMode {
        public Canvas BlueprintUICanvas { get; set; }
        public GameObject Title { get; set; }
        public List<GameObject> CanvasObjects { get; set; }
        public Boolean Visible { get; set; }
        public String TitleStr { get; set; }

        void IBlueprintUIMode.OnInitialize() {

        }

        void IBlueprintUIMode.OnShow() {
            CanvasObjects.Add(NewPrimaryResource(100, 100));
        }

        void IBlueprintUIMode.OnHide() {

        }

        private GameObject NewPrimaryResource(float x, float y) {
            GameObject hex = new GameObject();
            hex.transform.SetParent(BlueprintUICanvas.transform);
            hex.transform.position = new Vector2(x, y);

            Image background = hex.AddComponent<Image>();
            background.sprite = AssetManager.Instance().backgroundSprite;

            return hex;
        }
    }
}
