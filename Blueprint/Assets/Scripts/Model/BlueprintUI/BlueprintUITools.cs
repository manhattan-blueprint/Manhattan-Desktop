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

        public static void NewBorder(Transform parent, List<GameObject> objList, float x, float y, float scale) {
            ScreenProportions sp = GameObject.Find("ScreenProportions").GetComponent<ScreenProportions>();
            float relativeSize = sp.ToH(scale);

            GameObject arrow = new GameObject();
            arrow.transform.SetParent(parent);
            arrow.transform.position = sp.ToV(new Vector2(x, y));
            arrow.AddComponent<RectTransform>();
            arrow.AddComponent<CanvasRenderer>();
            SVGImage arrowImage = arrow.AddComponent<SVGImage>();
            arrowImage.sprite = Resources.Load<Sprite>("slot_border_dark");
            (arrow.transform as RectTransform).sizeDelta = new Vector2(relativeSize * 1.15f, relativeSize * 1.15f);
            objList.Add(arrow);
        }

        // Create a new visual craftable instruction.
        public static void NewCraftable(Transform parent, List<GameObject> objList, float x, float y, float scale, int resultID, int resourceIDA=0, int resourceIDB=0, int resourceIDC=0) {
            ScreenProportions sp = GameObject.Find("ScreenProportions").GetComponent<ScreenProportions>();
            float relativeSize = sp.ToH(scale);
            float niceRatio = 0.75f; // Slightly less than sqrt(3)/2. Hexagon stuff.

            NewResource(parent, objList, x + scale * 1.2f, y, scale, resultID);
            NewCraftArrow(parent, objList, x + scale * 0.6f, y, scale);
            NewResource(parent, objList, x, y, scale, resourceIDA);
            if (resourceIDB > 0)
                NewResource(parent, objList, x - scale * 0.6f, y, scale, resourceIDB);
            if (resourceIDC > 0)
                NewResource(parent, objList, x - scale * 0.3f, y + scale * niceRatio, scale, resourceIDC);
        }

        // Create a new visual craftable instruction.
        public static void NewMachine(Transform parent, List<GameObject> objList, float x, float y, float scale, int resultID, int machineID, int resourceIDA, int resourceIDB=0) {
            ScreenProportions sp = GameObject.Find("ScreenProportions").GetComponent<ScreenProportions>();
            float relativeSize = sp.ToH(scale);
            float niceRatio = 0.8f; // Slightly less than sqrt(3)/2. Hexagon stuff.

            NewResource(parent, objList, x + scale * 1.2f, y, scale, resultID);
            NewCraftArrow(parent, objList, x + scale * 0.6f, y, scale);
            NewResource(parent, objList, x - scale * 0.6f, y, scale, resourceIDA);
            if (resourceIDB > 0)
                NewResource(parent, objList, x - scale * 0.3f, y + scale * niceRatio, scale, resourceIDB);
            NewResource(parent, objList, x, y, scale, machineID);
            NewBorder(parent, objList, x, y, scale);
        }

        // Create a new visual craftable instruction.
        public static void NewBlueprint(Transform parent, List<GameObject> objList, float x, float y, float scale, int resultID, int resourceIDA, int resourceIDB=0, int resourceIDC=0) {
            ScreenProportions sp = GameObject.Find("ScreenProportions").GetComponent<ScreenProportions>();
            float relativeSize = sp.ToH(scale);
            float niceRatio = 0.8f; // Slightly less than sqrt(3)/2. Hexagon stuff.

            NewResource(parent, objList, x + scale * 1.2f, y, scale, resultID);
            NewCraftArrow(parent, objList, x + scale * 0.6f, y, scale);
            NewResource(parent, objList, x - scale * 0.6f, y, scale, resourceIDA);
            if (resourceIDB > 0)
                NewResource(parent, objList, x - scale * 0.3f, y + scale * niceRatio, scale, resourceIDB);
            if (resourceIDC > 0)
                NewResource(parent, objList, x - scale * 0.3f, y + scale * niceRatio, scale, resourceIDC);
        }

        // Create a new visual craftable instruction.
        public static void NewGoal(Transform parent, List<GameObject> objList, float x, float y, float scale, int resultID, int resourceIDA=0, int resourceIDB=0, int resourceIDC=0) {
            ScreenProportions sp = GameObject.Find("ScreenProportions").GetComponent<ScreenProportions>();
            float relativeSize = sp.ToH(scale);
            float niceRatio = 0.8f; // Slightly less than sqrt(3)/2. Hexagon stuff.

            if (resourceIDA > 0)
                NewResource(parent, objList, x - scale * 0.6f, y, scale, resourceIDA);
            if (resourceIDB > 0)
                NewResource(parent, objList, x + scale * 0.3f, y + scale * niceRatio, scale, resourceIDB);
            if (resourceIDC > 0)
                NewResource(parent, objList, x + scale * 0.3f, y - scale * niceRatio, scale, resourceIDC);

            NewResource(parent, objList, x, y, scale, resultID);
            NewBorder(parent, objList, x, y, scale);
        }

        public static void NewCraftArrow(Transform parent, List<GameObject> objList, float x, float y, float scale) {
            ScreenProportions sp = GameObject.Find("ScreenProportions").GetComponent<ScreenProportions>();
            Vector2 position = sp.ToV(new Vector2(x, y));
            float relativeSize = sp.ToH(scale);

            GameObject craftArrow = new GameObject();
            craftArrow.transform.SetParent(parent);
            craftArrow.transform.position = position;
            craftArrow.AddComponent<RectTransform>();
            craftArrow.AddComponent<CanvasRenderer>();
            RawImage craftArrowImage = craftArrow.AddComponent<RawImage>();
            craftArrowImage.texture = Resources.Load<Texture>("CraftArrow");
            (craftArrow.transform as RectTransform).sizeDelta = new Vector2(relativeSize, relativeSize);
            (craftArrow.transform as RectTransform).localScale = new Vector3(0.7f, 0.7f, 0.0f);
            objList.Add(craftArrow);
        }

        // Create a new button that is also an svg image.
        public static Button CreateButton(Transform parent, List<GameObject> objList, Vector2 relativePosition, string name, float scale) {
            ScreenProportions sp = GameObject.Find("ScreenProportions").GetComponent<ScreenProportions>();
            float relativeSize = sp.ToH(scale);

            GameObject arrow = new GameObject();
            arrow.transform.SetParent(parent);
            arrow.transform.position = sp.ToV(relativePosition);
            arrow.AddComponent<RectTransform>();
            arrow.AddComponent<CanvasRenderer>();
            SVGImage arrowImage = arrow.AddComponent<SVGImage>();
            arrowImage.sprite = Resources.Load<Sprite>(name);
            (arrow.transform as RectTransform).sizeDelta = new Vector2(relativeSize, relativeSize);
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
        public static GameObject CreateTitle(Transform parent, List<GameObject> objList, string titleText) {
            ScreenProportions sp = GameObject.Find("ScreenProportions").GetComponent<ScreenProportions>();

            // TODO: Fonts not loading currently for some reason.
            GameObject title = new GameObject();
            title.transform.SetParent(parent);
            title.AddComponent<RectTransform>();
            title.AddComponent<CanvasRenderer>();
            Text newText = title.AddComponent<Text>();
            newText.text = titleText;
            newText.alignment = TextAnchor.MiddleCenter;
            newText.fontSize = 80;
            newText.font = Resources.Load<Font>("Fonts/MeckaBleckaRegular");
            newText.color = new Color(9.0f, 38.0f, 66.0f);
            title.transform.position = sp.ToV(new Vector2(0.5f, 0.9f));
            (title.transform as RectTransform).sizeDelta = new Vector2(sp.ToH(1.0f), sp.ToH(0.2f));
            objList.Add(title);
            return title;
        }

        // Create infomative text for each blueprint UI menu.
        public static GameObject CreateInfoText(Transform parent, List<GameObject> objList, string infoText) {
            ScreenProportions sp = GameObject.Find("ScreenProportions").GetComponent<ScreenProportions>();

            // TODO: Fonts not loading currently for some reason.
            GameObject title = new GameObject();
            title.transform.SetParent(parent);
            title.AddComponent<RectTransform>();
            title.AddComponent<CanvasRenderer>();
            Text newText = title.AddComponent<Text>();
            newText.text = infoText;
            newText.alignment = TextAnchor.MiddleCenter;
            newText.fontSize = 20;
            newText.font = Resources.Load<Font>("Fonts/HelveticaNeueMedium");
            newText.color = new Color(9.0f, 38.0f, 66.0f);
            title.transform.position = sp.ToV(new Vector2(0.5f, 0.05f));
            (title.transform as RectTransform).sizeDelta = new Vector2(sp.ToH(1.0f), sp.ToH(0.2f));
            objList.Add(title);
            return title;
        }
    }
}
