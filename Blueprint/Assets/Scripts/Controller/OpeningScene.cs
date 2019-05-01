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
  private GameObject dishBase;

  void Start() {
    dishBase = GameObject.Find("Beacon");
  }

  private void introAnimation() {
       IEnumerator timedCoroutine = worldPanning();
       StartCoroutine(timedCoroutine);
  }

  private IEnumerator worldPanning() {
        float spinSpeed = 0.0f;

        ManhattanAnimation animationManager = this.gameObject.AddComponent<ManhattanAnimation>();

        GameObject signalSent = GameObject.Find("Story");
        animationManager.StartAppearanceAnimation(signalSent, Anim.Appear, 3.0f, true, 0.0f, 13.0f);
        animationManager.StartAppearanceAnimation(signalSent, Anim.Disappear, 3.0f, true, 0.0f, 30.0f);

        GameObject blackOverlay = GameObject.Find("FadeOverlay");
        animationManager.StartAppearanceAnimation(blackOverlay, Anim.Appear, 3.0f, true, 0.0f, 13.0f);

        // Create astronaut.
        // Vector3 astronoautPos = Camera.main.transform.position - Camera.main.transform.forward * 0.8f;
        // astronoautPos += new Vector3(0.0f, -astronoautPos.y, 0.0f);
        // GameObject astronaut = Instantiate(Resources.Load("Astronaut") as GameObject, astronoautPos, Quaternion.identity);
        // astronaut.transform.LookAt(Vector3.zero);

        // while (true) {
        //     if (spinSpeed < 6.0f)
        //         spinSpeed += 0.005f;
        //
        //     // Make dish spin.
        //     yield return new WaitForSeconds(1.0f / 60.0f);
        //     dish.transform.RotateAround(Vector3.zero, Vector3.up, spinSpeed);
        //     dishHolder.transform.RotateAround(Vector3.zero, Vector3.up, spinSpeed);
        //
        //     // Make camera zoom out and spin.
        //     Camera.main.transform.position += new Vector3(0.0f, spinSpeed / 40.0f, 0.0f);
        //     Camera.main.transform.position -= Camera.main.transform.forward * spinSpeed / 30.0f;
        //     Camera.main.transform.LookAt(new Vector3(0.0f, 2.0f, 0.0f));
        //     Camera.main.transform.RotateAround(Vector3.zero, Vector3.up, - spinSpeed / 5.0f);
        //
        //     // Make Gameover overlay etc follow in front of camera
        //     blackOverlay.transform.position = Camera.main.transform.position + Camera.main.transform.forward;
        //     blackOverlay.transform.LookAt(Camera.main.transform.position);
        //     signalSent.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 0.9f;
        //     signalSent.transform.LookAt(Camera.main.transform.position + Camera.main.transform.forward * 2.0f);
        //     willRespond.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 0.9f;
        //     willRespond.transform.LookAt(Camera.main.transform.position + Camera.main.transform.forward * 2.0f);
        //     names.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 0.9f;
        //     names.transform.LookAt(Camera.main.transform.position + Camera.main.transform.forward * 2.0f);
        //     blueprintText.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 0.9f;
        //     blueprintText.transform.LookAt(Camera.main.transform.position + Camera.main.transform.forward * 2.0f);
        // }
    }
}
