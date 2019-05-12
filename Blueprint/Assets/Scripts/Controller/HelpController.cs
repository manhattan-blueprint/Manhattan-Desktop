using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Controller {
    public class HelpController : MonoBehaviour {

        [SerializeField] private TextMeshProUGUI helpTitle;
        [SerializeField] private Button leftArrow;
        [SerializeField] private Button exit;
        [SerializeField] private Button rightArrow;

        [SerializeField] private GameObject KeyBindingsGroup;
        [SerializeField] private GameObject InventoryGroup;
        [SerializeField] private GameObject PlacementGroup;
        [SerializeField] private GameObject MachinesGroup;
        [SerializeField] private GameObject BlueprintGroup;
        [SerializeField] private GameObject ExitGroup;
        private SoundController soundController;
        private HelpScreens currentScreen;

        enum HelpScreens {
            KeyBindings,
            Inventory,
            Placement,
            Machines,
            Blueprint,
            Exit
        };

        void Start() {
            soundController = GameObject.Find("SoundController").GetComponent<SoundController>();
            SetFirstScreen();
        }

        void Update() {

        }

        public void NextScreen() {
            soundController.PlayButtonPressSound();
            EventSystem.current.SetSelectedGameObject(null);
            switch (currentScreen) {
                case HelpScreens.KeyBindings:
                    KeyBindingsGroup.SetActive(false);
                    currentScreen = HelpScreens.Inventory;
                    helpTitle.text = "Inventory";
                    InventoryGroup.SetActive(true);
                    leftArrow.gameObject.SetActive(true);
                    break;
                case HelpScreens.Inventory:
                    InventoryGroup.SetActive(false);
                    currentScreen = HelpScreens.Placement;
                    helpTitle.text = "Placement";
                    PlacementGroup.SetActive(true);
                    break;
                case HelpScreens.Placement:
                    PlacementGroup.SetActive(false);
                    currentScreen = HelpScreens.Machines;
                    helpTitle.text = "Machines";
                    MachinesGroup.SetActive(true);
                    break;
                case HelpScreens.Machines:
                    MachinesGroup.SetActive(false);
                    currentScreen = HelpScreens.Blueprint;
                    helpTitle.text = "Blueprint Tree";
                    BlueprintGroup.SetActive(true);
                    break;
                case HelpScreens.Blueprint:
                    BlueprintGroup.SetActive(false);
                    currentScreen = HelpScreens.Exit;
                    helpTitle.text = "Collecting Resources";
                    ExitGroup.SetActive(true);
                    rightArrow.gameObject.SetActive(false);
                    break;
                default:
                    break;
            }
        }

        public void PreviousScreen() {
            soundController.PlayButtonPressSound();
            EventSystem.current.SetSelectedGameObject(null);
            switch (currentScreen) {
                case HelpScreens.Inventory:
                    InventoryGroup.SetActive(false);
                    currentScreen = HelpScreens.KeyBindings;
                    helpTitle.text = "Key Bindings";
                    KeyBindingsGroup.SetActive(true);
                    leftArrow.gameObject.SetActive(false);
                    break;
                case HelpScreens.Placement:
                    PlacementGroup.SetActive(false);
                    currentScreen = HelpScreens.Inventory;
                    helpTitle.text = "Inventory";
                    InventoryGroup.SetActive(true);
                    break;
                case HelpScreens.Machines:
                    MachinesGroup.SetActive(false);
                    currentScreen = HelpScreens.Placement;
                    helpTitle.text = "Placement";
                    PlacementGroup.SetActive(true);
                    break;
                case HelpScreens.Blueprint:
                    BlueprintGroup.SetActive(false);
                    currentScreen = HelpScreens.Machines;
                    helpTitle.text = "Machines";
                    MachinesGroup.SetActive(true);
                    break;
                case HelpScreens.Exit:
                    ExitGroup.SetActive(false);
                    currentScreen = HelpScreens.Blueprint;
                    helpTitle.text = "Blueprint Tree";
                    BlueprintGroup.SetActive(true);
                    rightArrow.gameObject.SetActive(true);
                    break;
                default:
                    break;
            }
        }

        public void SetFirstScreen() {
            currentScreen = HelpScreens.KeyBindings;
            leftArrow.gameObject.SetActive(false);
            KeyBindingsGroup.SetActive(true);
            InventoryGroup.SetActive(false);
            PlacementGroup.SetActive(false);
            MachinesGroup.SetActive(false);
            BlueprintGroup.SetActive(false);
            ExitGroup.SetActive(false);
            exit.gameObject.SetActive(false);
            rightArrow.gameObject.SetActive(true);
            leftArrow.gameObject.SetActive(false);
        }
    }
}
