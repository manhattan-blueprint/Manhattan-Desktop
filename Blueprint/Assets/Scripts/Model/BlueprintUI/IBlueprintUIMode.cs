using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Model.BlueprintUI {
    // Holds interface and generic abstract functions for blueprint UI
    public interface IBlueprintUIMode {
        Canvas BlueprintUICanvas { get; set; }          // The whole canvas for all menus.
        GameObject Title { get; set; }                  //
        String TitleStr { get; set; }
        List<GameObject> CanvasObjects { get; set; }

        void OnInitialize();
        void OnShow();
        void OnHide();
    }

    // Implements abstract class for the IBLueprintUIMode interface
    public static class IBlueprintUIModeHelper {
        public static void Initialize(this IBlueprintUIMode bUIMode, Canvas blueprintUICanvas, string screenName) {
            bUIMode.BlueprintUICanvas = blueprintUICanvas;
            bUIMode.CanvasObjects = new List<GameObject>();
            bUIMode.OnInitialize();
            bUIMode.TitleStr = screenName;
        }

        public static void Show(this IBlueprintUIMode bUIMode) {
            // Spawns that are congruent between all screens
            bUIMode.Title = new GameObject("Title", typeof(RectTransform));
            var newText = bUIMode.Title.AddComponent<Text>();
            newText.text = bUIMode.TitleStr;
            newText.alignment = TextAnchor.MiddleCenter;
            newText.fontSize = 200;
            // title.font = fontMessage;
            newText.color = new Color(254f, 152f, 203f);
            newText.transform.position = new Vector2(200.0f, 300.0f);
            bUIMode.CanvasObjects.Add(bUIMode.Title);

            // BlueprintUICanvas.interactable = true;
            // animationManager.StartAppearanceAnimation(obj.gameObject, Anim.Appear, 0.3f, false, 0.0f, 0.2f);
            // animationManager.StartMovementAnimation(obj.gameObject, Anim.MoveToDecelerate, sp.ToV(new Vector3(0.0f, 0.9f, 0.0f)), 0.4f, false);

            bUIMode.OnShow();
        }

        public static void Hide(this IBlueprintUIMode bUIMode) {
            foreach(GameObject gameObject in bUIMode.CanvasObjects) {
                MonoBehaviour.Destroy(gameObject);
            }

            // BlueprintUICanvas.interactable = false;
            // animationManager.StartAppearanceAnimation(obj.gameObject, Anim.Dissappear, 0.3f, false, 0.0f, 0.0f);
            // animationManager.StartMovementAnimation(obj.gameObject, Anim.MoveToDecelerate, sp.ToV(new Vector3(0.0f, -0.9f, 0.0f)), 0.4f, false);

            bUIMode.OnHide();
        }
    }
}
