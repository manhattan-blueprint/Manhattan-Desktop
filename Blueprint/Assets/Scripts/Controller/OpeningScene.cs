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

public class OpeningScene : MonoBehaviour
{
  private GameObject ;

  private IEnumerator SpinDish() {
            float spinSpeed = 0.0f;

            ManhattanAnimation animationManager = this.gameObject.AddComponent<ManhattanAnimation>();

            GameObject blackOverlay = GameObject.Find("GameoverOverlay");
            animationManager.StartAppearanceAnimation(blackOverlay, Anim.Appear, 3.0f, true, 0.0f, 13.0f);
            GameObject signalSent = GameObject.Find("SignalSent");
            animationManager.StartAppearanceAnimation(signalSent, Anim.Appear, 3.0f, true, 0.0f, 13.0f);
            animationManager.StartAppearanceAnimation(signalSent, Anim.Disappear, 3.0f, true, 0.0f, 30.0f);
            GameObject willRespond = GameObject.Find("WillRespond");
            animationManager.StartAppearanceAnimation(willRespond, Anim.Appear, 3.0f, true, 0.0f, 20.0f);
            animationManager.StartAppearanceAnimation(willRespond, Anim.Disappear, 3.0f, true, 0.0f, 30.0f);

            GameObject names = GameObject.Find("Names");
            animationManager.StartAppearanceAnimation(names, Anim.Appear, 3.0f, true, 0.0f, 34.0f);
            animationManager.StartAppearanceAnimation(names, Anim.Disappear, 3.0f, true, 0.0f, 47.0f);

            GameObject blueprintText = GameObject.Find("GameoverBlueprintText");
            animationManager.StartAppearanceAnimation(blueprintText, Anim.Grow, 3.0f, false, 1.0f, 53.0f);
            blueprintText.transform.localScale = Vector3.zero;

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
                blueprintText.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 0.9f;
                blueprintText.transform.LookAt(Camera.main.transform.position + Camera.main.transform.forward * 2.0f);
            }
        }
    }
}
