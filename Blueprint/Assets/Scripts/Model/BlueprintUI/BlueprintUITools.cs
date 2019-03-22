using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Model.BlueprintUI {
    public static class BlueprintUITools {
        // Create a new resource shown in a hex tile.
        public static void NewResource(Transform parent, List<GameObject> objList, float x, float y, float scale, int id) {
            ScreenProportions sp = GameObject.Find("ScreenProportions").GetComponent<ScreenProportions>();

            Vector2 position = sp.ToV(new Vector2(x, y));
            float relativeSize = sp.ToH(scale);

            // Create hex object with blue hexagon background relative to
            // size of the screen.
            GameObject hex = new GameObject();
            hex.transform.SetParent(parent);
            hex.transform.position = position;
            Image hexBackground = hex.AddComponent<Image>();
            hexBackground.sprite = Resources.Load("inventory_slot", typeof(Sprite)) as Sprite;
            (hex.transform as RectTransform).sizeDelta = new Vector2(relativeSize, relativeSize);
            (hex.transform as RectTransform).localScale = new Vector3(1.0f, 1.0f, 0.0f);
            objList.Add(hex);

            // Load in the resource image and place on top relative to the size
            // of the screen again.
            GameObject hexResource = new GameObject();
            hexResource.transform.SetParent(parent);
            hexResource.transform.position = position;
            hexResource.AddComponent<RectTransform>();
            hexResource.AddComponent<CanvasRenderer>();
            RawImage hexResourceImage = hexResource.AddComponent<RawImage>();
            hexResourceImage.texture = AssetManager.Instance().GetItemTexture(id, "Models/2D/");
            (hexResource.transform as RectTransform).sizeDelta = new Vector2(relativeSize, relativeSize);
            (hexResource.transform as RectTransform).localScale = new Vector3(0.7f, 0.7f, 0.0f);
            objList.Add(hexResource);
        }

        // Create a new visual craftable instruction.
        public static void NewCraftable(Transform parent, List<GameObject> objList, float x, float y, float scale, int machineID, int resultID, int resourceIDA=0, int resourceIDB=0, int resourceIDC=0) {
            ScreenProportions sp = GameObject.Find("ScreenProportions").GetComponent<ScreenProportions>();
            float relativeSize = sp.ToH(scale);

            NewResource(parent, objList, x + scale, y, scale, resultID);
            NewResource(parent, objList, x, y, scale, machineID);

            // if (idA != 0) NewResource(x - )
        }

        // Create a new button that is also an svg image.
        public static Button CreateButton(Transform parent, List<GameObject> objList, Vector2 relativePosition, string name) {
            ScreenProportions sp = GameObject.Find("ScreenProportions").GetComponent<ScreenProportions>();

            GameObject arrow = new GameObject();
            arrow.transform.SetParent(parent);
            arrow.transform.position = sp.ToV(relativePosition);
            arrow.AddComponent<RectTransform>();
            arrow.AddComponent<CanvasRenderer>();
            SVGImage arrowImage = arrow.AddComponent<SVGImage>();
            arrowImage.sprite = Resources.Load<Sprite>(name);
            (arrow.transform as RectTransform).sizeDelta = new Vector2(sp.ToH(0.1f), sp.ToH(0.1f));
            objList.Add(arrow);

            Button arrowButton = arrow.AddComponent<Button>();
            arrowButton.transform.position = sp.ToV(relativePosition);
            ColorBlock colors = arrowButton.colors;
            // colors.normalColor = new Color(255f, 255f, 255f, 0.1f);
            colors.highlightedColor = new Color(235f, 91f, 92f, 0.5f);
            // colors.pressedColor = new Color(122f, 122f, 122f);
            arrowButton.colors = colors;

            return arrowButton;
        }

        // Create the title used for each blueprint UI menu.
        public static GameObject CreateTitle(Transform parent, string titleText) {
            ScreenProportions sp = GameObject.Find("ScreenProportions").GetComponent<ScreenProportions>();

            // TODO: Fonts not loading currently for some reason.
            GameObject title = new GameObject();
            title.transform.SetParent(parent);
            title.AddComponent<RectTransform>();
            title.AddComponent<CanvasRenderer>();
            Text newText = title.AddComponent<Text>();
            newText.text = titleText;
            newText.alignment = TextAnchor.MiddleCenter;
            newText.fontSize = 20;
            newText.font = Resources.Load<Font>("HelveticaNeueBold");
            newText.color = new Color(254f, 152f, 203f);
            title.transform.position = sp.ToV(new Vector2(0.5f, 0.5f));
            return title;
        }
    }
}
