using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model.BlueprintUI;
using UnityEngine.UI;
using Utils;

namespace Controller {
    public class BlueprintUIController : MonoBehaviour {
        private IBlueprintUIMode primaryResourceUI;
        private IBlueprintUIMode craftableResourceUI;
        private IBlueprintUIMode machineryResourceUI;
        private IBlueprintUIMode blueprintResourceUI;
        private IBlueprintUIMode goalResourceUI;

        private Boolean visible;
        private CurrentMenu currentMenu;

        Text infoText;

        private enum CurrentMenu {
            Primary,
            Craftable,
            Machinery,
            Blueprint,
            Goal
        }

        void Start() {
            primaryResourceUI = new PrimaryResourceUI();
            primaryResourceUI.Initialize(gameObject, "Primary Resources");
            craftableResourceUI = new CraftableResourceUI();
            craftableResourceUI.Initialize(gameObject, "Craftable Resources");
            machineryResourceUI = new MachineryResourceUI();
            machineryResourceUI.Initialize(gameObject, "Machine Craftables");
            blueprintResourceUI = new BlueprintResourceUI();
            blueprintResourceUI.Initialize(gameObject, "Blueprints");
            goalResourceUI = new GoalResourceUI();
            goalResourceUI.Initialize(gameObject, "Final Goal");
            currentMenu = CurrentMenu.Primary;
            visible = false;

            // Create some ifno text between screens for when stuff is crafted.
            ScreenProportions sp = GameObject.Find("ScreenProportions").GetComponent<ScreenProportions>();
            Vector2 relativePosition = sp.ToV(new Vector2(0.5f, 0.1f));

            GameObject obj = new GameObject();
            obj.AddComponent<RectTransform>();
            obj.AddComponent<CanvasRenderer>();
            infoText = obj.AddComponent<Text>();
            infoText.text = "";
            infoText.alignment = TextAnchor.MiddleCenter;
            infoText.fontSize = 20;
            infoText.font = Resources.Load<Font>("Fonts/HelveticaNeueMedium");
            infoText.color = new Color(245.0f/255.0f, 245.0f/255.0f, 245.0f/255.0f);
            obj.transform.position = relativePosition;
            (obj.transform as RectTransform).sizeDelta = new Vector2(sp.ToH(1.0f), sp.ToH(0.2f));
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

            if (!gameObject.GetComponent<Canvas>().enabled && visible)
                visible = false;

            if (visible) {
                if (Input.GetKeyDown(KeyCode.RightArrow)) NextMenu();
                if (Input.GetKeyDown(KeyCode.LeftArrow)) PreviousMenu();
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

        public void RefreshMenu(string info) {
            // Reset invoke in case outstanding one.
            CancelInvoke();

            infoText.text = info;
            Invoke("EmptyText", 2.0f);

            switch (currentMenu) {
                case CurrentMenu.Primary:
                    primaryResourceUI.Hide();
                    primaryResourceUI.Show();
                    break;

                case CurrentMenu.Craftable:
                    craftableResourceUI.Hide();
                    craftableResourceUI.Show();
                    break;

                case CurrentMenu.Machinery:
                    machineryResourceUI.Hide();
                    machineryResourceUI.Show();
                    break;

                case CurrentMenu.Blueprint:
                    blueprintResourceUI.Hide();
                    blueprintResourceUI.Show();
                    break;

                case CurrentMenu.Goal:
                    goalResourceUI.Hide();
                    goalResourceUI.Show();
                    break;

                default:
                    throw new Exception("Attempting to refresh menu while in an unexpected state.");
                    break;
            }
        }

        void EmptyText() {
            infoText.text="";
        }
    }
}
