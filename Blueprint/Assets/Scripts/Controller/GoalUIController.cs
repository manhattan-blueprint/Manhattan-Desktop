using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model.BlueprintUI;
using UnityEngine.UI;
using Utils;
using Model;
using Model.Action;
using Model.Redux;
using Model.State;

namespace Controller {
    public class GoalUIController : MonoBehaviour, Subscriber<MapState> {
        private GameObject dish;
        private GameObject dishHolder;
        private GameObject bigDish;
        private GameObject antenna1;
        private GameObject antenna2;
        private GameObject antenna3;
        private GameObject antenna4;
        private GameObject antenna5;
        private GameObject antenna6;
        private GameObject dishHolder1;
        private GameObject dishHolder2;
        private GameObject dishHolder3;
        private bool transmitterPlaced;
        private Goal goal;

        void Start() {
            ScreenProportions sp = GameObject.Find("ScreenProportions").GetComponent<ScreenProportions>();
            Vector2 relativePosition = sp.ToV(new Vector2(0.5f, 0.1f));

            SetAlpha("TopItem", 0.2f);
            SetAlpha("MidItem", 0.2f);
            SetAlpha("BotItem", 0.2f);

            dish        = GameObject.Find("Dish");
            dishHolder  = GameObject.Find("DishHolder");
            bigDish     = GameObject.Find("BigDish");
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
            HideTransmitter();

            GameManager.Instance().mapStore.Subscribe(this);
        }

        void Update() {

        }

        public void StateDidUpdate(MapState state) {
            goal = state.getGoal();
            if (goal.topInput) {
                SetAlpha("TopSlot/TopItem", 1.0f);
                ActivateDish();
            }
            if (goal.midInput) {
                SetAlpha("MidSlot/MidItem", 1.0f);
                ActivateAntenna();
            }
            if (goal.botInput) {
                SetAlpha("BotSlot/BotItem", 1.0f);
                ActivateTransmitter();
            }
        }

        public bool CheckPlaced(GoalPosition position) {
            if (position == GoalPosition.Top)
                return goal.topInput;
            if (position == GoalPosition.Mid)
                return goal.midInput;
            if (position == GoalPosition.Bot)
                return goal.botInput;
            return false;
        }

        public void HideDish() {
            bigDish.GetComponent<MeshRenderer>().enabled = false;
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

        public void HideTransmitter() {
            transmitterPlaced = false;
        }

        public void ActivateDish() {
            bigDish.GetComponent<MeshRenderer>().enabled = true;
            dishHolder1.GetComponent<MeshRenderer>().enabled = true;
            dishHolder2.GetComponent<MeshRenderer>().enabled = true;
            dishHolder3.GetComponent<MeshRenderer>().enabled = true;
            SetSlotActive("MidSlot");
        }

        public void ActivateAntenna() {
            antenna1.GetComponent<MeshRenderer>().enabled = true;
            antenna2.GetComponent<MeshRenderer>().enabled = true;
            antenna3.GetComponent<MeshRenderer>().enabled = true;
            antenna4.GetComponent<MeshRenderer>().enabled = true;
            antenna5.GetComponent<MeshRenderer>().enabled = true;
            antenna6.GetComponent<MeshRenderer>().enabled = true;
            SetSlotActive("BotSlot");
        }

        public void ActivateTransmitter() {
            transmitterPlaced = true;
        }

        // Finds an object by name, sets its alpha low.
        public void SetAlpha(string name, float level) {
            Image image = GameObject.Find(name).GetComponent<Image>();
            var color = image.color;
            color.a = level;
            image.color = color;
        }

        public void SetSlotActive(string name) {
            Image image = GameObject.Find(name).GetComponent<Image>();
            // White give sprite default appearance.
            image.color = Color.white;
        }

        public void StartWinAnimation() {
            IEnumerator timedCoroutine = SpinDish();
            StartCoroutine(timedCoroutine);
        }

        private IEnumerator SpinDish() {
            float spinSpeed = 0.0f;

            ManhattanAnimation animationManager = this.gameObject.AddComponent<ManhattanAnimation>();

            GameObject blackOverlay = GameObject.Find("GameoverOverlay");
            animationManager.StartAppearanceAnimation(blackOverlay, Anim.Appear, 3.0f, true, 0.0f, 11.0f);
            GameObject signalSent = GameObject.Find("SignalSent");
            animationManager.StartAppearanceAnimation(signalSent, Anim.Appear, 2.0f, true, 0.0f, 12.0f);
            animationManager.StartAppearanceAnimation(signalSent, Anim.Disappear, 1.5f, true, 0.0f, 18.0f);
            GameObject willRespond = GameObject.Find("WillRespond");
            animationManager.StartAppearanceAnimation(willRespond, Anim.Appear, 2.0f, true, 0.0f, 14.0f);
            animationManager.StartAppearanceAnimation(willRespond, Anim.Disappear, 1.5f, true, 0.0f, 18.0f);

            GameObject names = GameObject.Find("Names");
            animationManager.StartAppearanceAnimation(names, Anim.Appear, 3.0f, true, 0.0f, 21.0f);

            // Create astronaut.
            Vector3 astronoautPos = Camera.main.transform.position - Camera.main.transform.forward * 0.8f;
            astronoautPos += new Vector3(0.0f, -astronoautPos.y, 0.0f);
            GameObject astronaut = Instantiate(Resources.Load("Astronaut") as GameObject, astronoautPos, Quaternion.identity);
            astronaut.transform.LookAt(Vector3.zero);

            while (true) {
                if (spinSpeed < 6.0f)
                    spinSpeed += 0.005f;

                // Make dish spin.
                yield return new WaitForSeconds(1.0f / 60.0f);
                dish.transform.RotateAround(Vector3.zero, Vector3.up, spinSpeed);
                dishHolder.transform.RotateAround(Vector3.zero, Vector3.up, spinSpeed);

                // Make camera zoom out and spin.
                Camera.main.transform.position += new Vector3(0.0f, spinSpeed / 40.0f, 0.0f);
                Camera.main.transform.position -= Camera.main.transform.forward * spinSpeed / 30.0f;
                Camera.main.transform.LookAt(new Vector3(0.0f, 2.0f, 0.0f));
                Camera.main.transform.RotateAround(Vector3.zero, Vector3.up, - spinSpeed / 5.0f);

                // Make Gameover overlay etc follow in front of camera
                blackOverlay.transform.position = Camera.main.transform.position + Camera.main.transform.forward;
                blackOverlay.transform.LookAt(Camera.main.transform.position);
                signalSent.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 0.9f;
                signalSent.transform.LookAt(Camera.main.transform.position + Camera.main.transform.forward * 2.0f);
                willRespond.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 0.9f;
                willRespond.transform.LookAt(Camera.main.transform.position + Camera.main.transform.forward * 2.0f);
                names.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 0.9f;
                names.transform.LookAt(Camera.main.transform.position + Camera.main.transform.forward * 2.0f);
            }
        }
    }
}
