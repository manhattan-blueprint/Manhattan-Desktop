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
  private bool introCompletionCheck;

  void Start() {
    dishBase = GameObject.Find("Beacon");
    introCompletionCheck = GameManager.Instance().uiStore.GetState().IntroComplete;
    Debug.Log(introCompletionCheck);
    Debug.Log("Hello");
    if (!introCompletionCheck) {
      GameManager.Instance().uiStore.Dispatch(new OpenIntroUI());
      introAnimation();
    }
  }

  private void introAnimation() {
       IEnumerator timedCoroutine = worldPanning();
       StartCoroutine(timedCoroutine);
  }

  private IEnumerator worldPanning() {
        float spinSpeed = 0.0f;

        ManhattanAnimation animationManager = this.gameObject.AddComponent<ManhattanAnimation>();

        GameObject story1 = GameObject.Find("Story1");
        animationManager.StartAppearanceAnimation(story1, Anim.Appear, 3.0f, true, 0.0f, 13.0f);
        animationManager.StartAppearanceAnimation(story1, Anim.Disappear, 3.0f, true, 0.0f, 30.0f);

        GameObject fadeOverlay = GameObject.Find("FadeOverlay");
        animationManager.StartAppearanceAnimation(fadeOverlay, Anim.Appear, 3.0f, true, 0.0f, 13.0f);

        // Create astronaut.
        // Vector3 astronoautPos = Camera.main.transform.position - Camera.main.transform.forward * 0.8f;
        // astronoautPos += new Vector3(0.0f, -astronoautPos.y, 0.0f);
        // GameObject astronaut = Instantiate(Resources.Load("Astronaut") as GameObject, astronoautPos, Quaternion.identity);
        // astronaut.transform.LookAt(Vector3.zero);

        while (true) {

            yield return new WaitForSeconds(1.0f / 60.0f);

            // Make camera zoom out and spin.
            Camera.main.transform.position += new Vector3(0.0f, 0.1f, 0.0f);
            Camera.main.transform.position -= Camera.main.transform.forward * 0.15f;
            Camera.main.transform.LookAt(new Vector3(0.0f, 2.0f, 0.0f));
            Camera.main.transform.RotateAround(Vector3.zero, Vector3.up, - 0.15f);

            // Make Gameover overlay etc follow in front of camera
            story1.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 0.9f;
            story1.transform.LookAt(Camera.main.transform.position + Camera.main.transform.forward * 2.0f);

            fadeOverlay.transform.position = Camera.main.transform.position + Camera.main.transform.forward;
            fadeOverlay.transform.LookAt(Camera.main.transform.position);
        }
    }
}
