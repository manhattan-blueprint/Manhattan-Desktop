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
            visible = false;

            // Create some ifno text between screens for when stuff is crafted.
            ScreenProportions sp = GameObject.Find("ScreenProportions").GetComponent<ScreenProportions>();
            Vector2 relativePosition = sp.ToV(new Vector2(0.5f, 0.1f));
        }

        void Update() {
            // Need to make a menu visible if no menu is currently visible.

            if (visible) {
            }
        }
    }
}
