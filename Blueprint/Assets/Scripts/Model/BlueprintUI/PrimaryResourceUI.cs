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
            CanvasObjects.Add(NewPrimaryResource(500, 500, "Logs"));
        }

        void IBlueprintUIMode.OnHide() {

        }

        private GameObject NewPrimaryResource(float x, float y, string resourcePath) {
            // GameObject hex = new GameObject();
            // hex.transform.SetParent(BlueprintUICanvas.transform);
            // hex.transform.position = new Vector2(x, y);
            //
            // Image hexBackground = hex.AddComponent<Image>();
            // hexBackground.sprite = Resources.Load("inventory_slot", typeof(Sprite)) as Sprite;

            GameObject hexResource = new GameObject();
            hexResource.transform.SetParent(BlueprintUICanvas.transform);
            hexResource.transform.position = new Vector2(x, y);

            Image resourceImage = hexResource.AddComponent<Image>();
            resourceImage.sprite = Resources.Load<Sprite>("Logs.jpeg");

            // (svgChild.transform as RectTransform).sizeDelta = new Vector2(slotDimension, slotDimension);
            // (svgChild.transform as RectTransform).localScale = new Vector3(1.05f, 1.05f, 0.0f);

            Debug.Log(resourceImage.sprite);

            return hexResource;
        }
    }
}
