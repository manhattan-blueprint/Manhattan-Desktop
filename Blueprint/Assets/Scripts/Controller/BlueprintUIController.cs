using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model.BlueprintUI;

namespace Controller {
    public class BlueprintUIController : MonoBehaviour {
        private IBlueprintUIMode primaryResourceUI;
        private IBlueprintUIMode machineryResourceUI;
        private IBlueprintUIMode blueprintResourceUI;
        private IBlueprintUIMode goalResourceUI;

        private Boolean visible;
        private CurrentMenu currentMenu;

        private enum CurrentMenu {
            Primary,
            Machinery,
            Blueprint,
            Goal
        }

        void Start() {
            primaryResourceUI = new PrimaryResourceUI();
            primaryResourceUI.Initialize(gameObject.GetComponent<Canvas>(), "Primary Resources");
            currentMenu = CurrentMenu.Primary;
            visible = false;
        }

        void Update() {
            if (gameObject.GetComponent<Canvas>().enabled && !visible) {
                switch (currentMenu) {
                    case CurrentMenu.Primary: primaryResourceUI.Show(); break;
                    case CurrentMenu.Machinery: machineryResourceUI.Show(); break;
                    case CurrentMenu.Blueprint: blueprintResourceUI.Show(); break;
                    case CurrentMenu.Goal: goalResourceUI.Show(); break;
                }
                visible = true;
            }

            // All menus need to be rehidden for correct recreation.
            if (!gameObject.GetComponent<Canvas>().enabled && visible) {
                if (visible) {
                    switch (currentMenu) {
                        case CurrentMenu.Primary: primaryResourceUI.Hide(); break;
                        case CurrentMenu.Machinery: machineryResourceUI.Hide(); break;
                        case CurrentMenu.Blueprint: blueprintResourceUI.Hide(); break;
                        case CurrentMenu.Goal: goalResourceUI.Hide(); break;
                    }
                }
                visible = false;
            }
        }

        // Swap to the next menu in menu progression.
        private void NextMenu(GameObject obj) {
            switch (currentMenu) {
                case CurrentMenu.Primary:
                    primaryResourceUI.Hide();
                    machineryResourceUI.Show();
                    currentMenu = CurrentMenu.Machinery;
                    break;

                case CurrentMenu.Machinery:
                    machineryResourceUI.Hide();
                    blueprintResourceUI.Show();
                    currentMenu = CurrentMenu.Blueprint;
                    break;

                case CurrentMenu.Blueprint:
                    blueprintResourceUI.Hide();
                    goalResourceUI.Show();
                    currentMenu = CurrentMenu.Goal;
                    break;

                case CurrentMenu.Goal:
                    goalResourceUI.Hide();
                    primaryResourceUI.Show();
                    currentMenu = CurrentMenu.Primary;
                    break;

                default:
                    throw new Exception("Attempting to swap menu while in unexpected state.");
                    break;
            }
        }

        // Swap to the previous menu in menu progression.
        private void PreviousMenu(GameObject obj) {
            switch (currentMenu) {
                case CurrentMenu.Primary:
                    primaryResourceUI.Hide();
                    goalResourceUI.Show();
                    currentMenu = CurrentMenu.Goal;
                    break;

                case CurrentMenu.Machinery:
                    machineryResourceUI.Hide();
                    primaryResourceUI.Show();
                    currentMenu = CurrentMenu.Primary;
                    break;

                case CurrentMenu.Blueprint:
                    blueprintResourceUI.Hide();
                    machineryResourceUI.Show();
                    currentMenu = CurrentMenu.Machinery;
                    break;

                case CurrentMenu.Goal:
                    goalResourceUI.Hide();
                    blueprintResourceUI.Show();
                    currentMenu = CurrentMenu.Blueprint;
                    break;

                default:
                    throw new Exception("Attempting to swap menu while in unexpected state.");
                    break;
            }
        }
    }
}
