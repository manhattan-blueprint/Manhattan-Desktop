using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model.BlueprintUI;

namespace Controller {
    public class BlueprintUIController : MonoBehaviour {
        private IBlueprintUIMode primaryResourceUI;
        private IBlueprintUIMode craftableResourceUI;
        private IBlueprintUIMode machineryResourceUI;
        private IBlueprintUIMode blueprintResourceUI;
        private IBlueprintUIMode goalResourceUI;

        private Boolean visible;
        private CurrentMenu currentMenu;

        private enum CurrentMenu {
            Primary,
            Craftable,
            Machinery,
            Blueprint,
            Goal
        }

        void Start() {
            primaryResourceUI = new PrimaryResourceUI();
            craftableResourceUI = new CraftableResourceUI();
            machineryResourceUI = new MachineryResourceUI();
            blueprintResourceUI = new BlueprintResourceUI();
            goalResourceUI = new GoalResourceUI();
            primaryResourceUI.Initialize(gameObject, "Primary Resources");
            craftableResourceUI.Initialize(gameObject, "Craftable Resources");
            machineryResourceUI.Initialize(gameObject, "Machine Craftable Resources");
            blueprintResourceUI.Initialize(gameObject, "Blueprints");
            goalResourceUI.Initialize(gameObject, "Final Goal");
            currentMenu = CurrentMenu.Primary;
            visible = false;
        }

        void Update() {
            // Need to make a menu visible if no menu is currently visible.
            if (gameObject.GetComponent<Canvas>().enabled && !visible) {
                switch (currentMenu) {
                    case CurrentMenu.Primary: primaryResourceUI.Show(); break;
                    case CurrentMenu.Craftable: craftableResourceUI.Show(); break;
                    case CurrentMenu.Machinery: machineryResourceUI.Show(); break;
                    case CurrentMenu.Blueprint: blueprintResourceUI.Show(); break;
                    case CurrentMenu.Goal: goalResourceUI.Show(); break;
                }
                visible = true;
            }
        }

        // Swap to the next menu in menu progression.
        public void NextMenu() {
            switch (currentMenu) {
                case CurrentMenu.Primary:
                    primaryResourceUI.Hide();
                    craftableResourceUI.Show();
                    currentMenu = CurrentMenu.Craftable;
                    break;

                case CurrentMenu.Craftable:
                    craftableResourceUI.Hide();
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
        public void PreviousMenu() {
            switch (currentMenu) {
                case CurrentMenu.Primary:
                    primaryResourceUI.Hide();
                    goalResourceUI.Show();
                    currentMenu = CurrentMenu.Goal;
                    break;

                case CurrentMenu.Craftable:
                    craftableResourceUI.Hide();
                    primaryResourceUI.Show();
                    currentMenu = CurrentMenu.Primary;
                    break;

                case CurrentMenu.Machinery:
                    machineryResourceUI.Hide();
                    craftableResourceUI.Show();
                    currentMenu = CurrentMenu.Craftable;
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
                    throw new Exception("Attempting to swap menu while in an unexpected state.");
                    break;
            }
        }
    }
}
