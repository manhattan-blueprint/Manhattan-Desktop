using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model.BlueprintUI;
using UnityEngine.UI;
using Utils;

namespace Controller {
    public class GoalUIController : MonoBehaviour {
        private Boolean visible;

        void Start() {
            ScreenProportions sp = GameObject.Find("ScreenProportions").GetComponent<ScreenProportions>();
            Vector2 relativePosition = sp.ToV(new Vector2(0.5f, 0.1f));

            SetLowAlpha("TopItem");
            SetLowAlpha("MidItem");
            SetLowAlpha("BotItem");

            // Extra comment for no reason.
        }

        void Update() {
        }

        // Finds an object by name, sets its alpha low.
        private void SetLowAlpha(string name) {
            Image image = GameObject.Find(name).GetComponent<Image>();
            var color = image.color;
            color.a = 0.2f;
            image.color = color;
        }
    }
}
