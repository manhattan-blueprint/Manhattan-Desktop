using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Controller;

namespace Model.BlueprintUI {
    // Holds interface and generic abstract functions for blueprint UI
    public interface IBlueprintUIMode {
        BlueprintUIController blueprintUIController { get; set; }   // The controller for all canvases.
        Canvas BlueprintUICanvas { get; set; }                      // The drawn canvas for this mode.
        GameObject Title { get; set; }                              // The title for this mode.
        String TitleStr { get; set; }                               // The string to set the title to show for this mode.
        List<GameObject> CanvasObjects { get; set; }                // All objects in this canvas.
        ManhattanAnimation animationManager { get; set; }           // A utility for animation.

        void OnInitialize();
        void OnShow();
        void OnHide();
    }

    // Implements abstract class for the IBLueprintUIMode interface
    public static class IBlueprintUIModeHelper {
        public static void Initialize(this IBlueprintUIMode bUIMode, GameObject controller, string screenName) {
            bUIMode.blueprintUIController = controller.GetComponent<BlueprintUIController>();
            bUIMode.BlueprintUICanvas = controller.GetComponent<Canvas>();
            bUIMode.CanvasObjects = new List<GameObject>();
            bUIMode.OnInitialize();
            bUIMode.TitleStr = screenName;
            bUIMode.animationManager = bUIMode.BlueprintUICanvas.gameObject.AddComponent<ManhattanAnimation>();
        }

        // Show spawns that are congruent between all screens.
        public static void Show(this IBlueprintUIMode bUIMode) {
            bUIMode.Title = BlueprintUITools.CreateTitle(bUIMode.BlueprintUICanvas.transform, bUIMode.TitleStr);
            bUIMode.CanvasObjects.Add(bUIMode.Title);

            Button leftBtn = BlueprintUITools.CreateButton(bUIMode.BlueprintUICanvas.transform, bUIMode.CanvasObjects, new Vector2(0.075f, 0.5f), "LeftArrow");
            leftBtn.onClick.AddListener(bUIMode.blueprintUIController.PreviousMenu);

            Button rightBtn = BlueprintUITools.CreateButton(bUIMode.BlueprintUICanvas.transform, bUIMode.CanvasObjects, new Vector2(0.925f, 0.5f), "RightArrow");
            rightBtn.onClick.AddListener(bUIMode.blueprintUIController.NextMenu);

            // BlueprintUICanvas.interactable = true;
            // bUIMode.animationManager.StartAppearanceAnimation(bUIMode.BlueprintUICanvas.gameObject, Anim.Appear, 0.5f, false, 0.0f, 0.2f);
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
