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
        private GameObject dish;
        private GameObject antenna1;
        private GameObject antenna2;
        private GameObject antenna3;
        private GameObject antenna4;
        private GameObject antenna5;
        private GameObject antenna6;
        private GameObject dishHolder1;
        private GameObject dishHolder2;
        private GameObject dishHolder3;

        void Start() {
            ScreenProportions sp = GameObject.Find("ScreenProportions").GetComponent<ScreenProportions>();
            Vector2 relativePosition = sp.ToV(new Vector2(0.5f, 0.1f));

            SetAlpha("TopItem", 0.2f);
            SetAlpha("MidItem", 0.2f);
            SetAlpha("BotItem", 0.2f);

            dish        = GameObject.Find("BigDish");
            dishHolder1 = GameObject.Find("DishHolder1");
            dishHolder2 = GameObject.Find("DishHolder2");
            dishHolder3 = GameObject.Find("DishHolder3");
            antenna1    = GameObject.Find("Antenna1");
            antenna2    = GameObject.Find("Antenna2");
            antenna3    = GameObject.Find("Antenna3");
            antenna4    = GameObject.Find("Antenna4");
            antenna5    = GameObject.Find("Antenna5");
            antenna6    = GameObject.Find("Antenna6");

            HideDish();
            HideAntenna();
            // No hiding required for transmitter.

            // Color in middle slot as available if top slot used already.
            GameObject topSlot = GameObject.Find("TopSlot");
            if (topSlot == null)
                Debug.Log("Error finding slot in inventory");
            InventorySlotController topSlotController = topSlot.GetComponent<InventorySlotController>();
            if (topSlotController.storedItem != null) {
                SetSlotActive("MidSlot");
            }
        }

        void Update() {

        }

        public void HideDish() {
            dish.GetComponent<MeshRenderer>().enabled        = false;
            dishHolder1.GetComponent<MeshRenderer>().enabled = false;
            dishHolder2.GetComponent<MeshRenderer>().enabled = false;
            dishHolder3.GetComponent<MeshRenderer>().enabled = false;
        }

        public void HideAntenna() {
            antenna1.GetComponent<MeshRenderer>().enabled = false;
            antenna2.GetComponent<MeshRenderer>().enabled = false;
            antenna3.GetComponent<MeshRenderer>().enabled = false;
            antenna4.GetComponent<MeshRenderer>().enabled = false;
            antenna5.GetComponent<MeshRenderer>().enabled = false;
            antenna6.GetComponent<MeshRenderer>().enabled = false;
        }

        public void ActivateDish() {
            dish.GetComponent<MeshRenderer>().enabled        = true;
            dishHolder1.GetComponent<MeshRenderer>().enabled = true;
            dishHolder2.GetComponent<MeshRenderer>().enabled = true;
            dishHolder3.GetComponent<MeshRenderer>().enabled = true;
        }

        public void ActivateAntenna() {
            antenna1.GetComponent<MeshRenderer>().enabled = true;
            antenna2.GetComponent<MeshRenderer>().enabled = true;
            antenna3.GetComponent<MeshRenderer>().enabled = true;
            antenna4.GetComponent<MeshRenderer>().enabled = true;
            antenna5.GetComponent<MeshRenderer>().enabled = true;
            antenna6.GetComponent<MeshRenderer>().enabled = true;
        }

        public void ActivateTransmitter() {
            
        }

        // Finds an object by name, sets its alpha low.
        public void SetAlpha(string name, float level) {
            Image image = GameObject.Find(name).GetComponent<Image>();
            var color = image.color;
            color.a = level;
            image.color = color;
        }

        public void SetSlotActive(string name) {
            Debug.Log("Activating Mid Slot");
            Image image = GameObject.Find(name).GetComponent<Image>();
            // White give sprite default appearance.
            image.color = Color.white;
        }
    }
}
