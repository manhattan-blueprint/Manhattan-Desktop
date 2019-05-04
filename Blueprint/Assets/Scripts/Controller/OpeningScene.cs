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
using TMPro;

public class OpeningScene : MonoBehaviour
{
    private GameObject dishBase;
    private bool introCompletionCheck;
    private GameObject mountainPath;
    private GameObject forestPath;
    private GameObject pondPath;
    private GameObject mountainWater;
    private int mountainSceneTime = 20;
    private int forestSceneTime = 15;
    private int pondSceneTime = 25;
    private int beaconSceneTime = 15;
    private int totalIntroTime;
    private bool play = true;
    private GameObject player;
    private float currCountdownValue;
    private GameObject story1;
    private GameObject story2;
    private GameObject story3;
    private GameObject story4;


    void Start() {
        totalIntroTime = mountainSceneTime + forestSceneTime + pondSceneTime + beaconSceneTime;

        dishBase = GameObject.Find("Beacon");
        mountainWater = GameObject.Find("MountainWater");
        player = GameObject.Find("Player");

        mountainPath = GameObject.Find("MountainPath");
        forestPath = GameObject.Find("ForestPath");
        pondPath = GameObject.Find("PondPath");

        story1 = GameObject.Find("Story1");
        story2 = GameObject.Find("Story2");
        story3 = GameObject.Find("Story3");
        story4 = GameObject.Find("Story4");

        story1.GetComponent<TextMeshProUGUI>().enabled = false;
        story2.GetComponent<TextMeshProUGUI>().enabled = false;
        story3.GetComponent<TextMeshProUGUI>().enabled = false;
        story4.GetComponent<TextMeshProUGUI>().enabled = false;

        introCompletionCheck = GameManager.Instance().uiStore.GetState().IntroComplete;
        if (!introCompletionCheck) {
          GameManager.Instance().uiStore.Dispatch(new OpenIntroUI());
          introAnimation();
        }
    }

    public int GetIntroTime() {
        return this.totalIntroTime;
    }

    private void introAnimation() {
        StartCoroutine(StartCountdown(totalIntroTime));
        StartCoroutine(sceneRunner());
    }

    private void enableText(GameObject text, bool val)
    {
        text.GetComponent<TextMeshProUGUI>().enabled = val;
    }

    private void cameraReset()
    {
        Camera.main.transform.position = player.transform.position + new Vector3(0f, 0.9f, 0f);
        player.transform.rotation = Quaternion.Euler(0, 180, 0);
        Camera.main.transform.rotation = Quaternion.Euler(0, 180, 0);
    }

        private IEnumerator StartCountdown(float countdownValue = 60)
    {
        currCountdownValue = countdownValue;
        while (currCountdownValue > 0)
        {
            yield return new WaitForSeconds(1.0f);
            currCountdownValue--;
        }
        play = false;
    }

    private IEnumerator sceneRunner() {
        enableText(story1, true);
        mountainPath.GetComponent<CPC_CameraPath>().PlayPath(mountainSceneTime);
        yield return new WaitForSeconds(mountainSceneTime);
        enableText(story1, false);

        enableText(story2, true);
        forestPath.GetComponent<CPC_CameraPath>().PlayPath(forestSceneTime);
        yield return new WaitForSeconds(forestSceneTime);
        enableText(story2, false);

        enableText(story3, true);
        pondPath.GetComponent<CPC_CameraPath>().PlayPath(pondSceneTime);
        yield return new WaitForSeconds(pondSceneTime);
        enableText(story3, false);

        // Reset camera to be within hex grid
        Camera.main.transform.position = new Vector3(20, 5, 20);
        while (play)
        {
            yield return new WaitForSeconds(1.0f / 60.0f);
            Camera.main.transform.LookAt(new Vector3(0.0f, 2.0f, 0.0f));
            Camera.main.transform.RotateAround(Vector3.zero, Vector3.up, -0.15f);
            //fadeOverlay.transform.position = Camera.main.transform.position + Camera.main.transform.forward;
            //fadeOverlay.transform.LookAt(Camera.main.transform.position);
        }
        cameraReset();
        GameManager.Instance().uiStore.Dispatch(new CloseUI());
    }

    private IEnumerator worldPanning() {
        float spinSpeed = 0.0f;

        ManhattanAnimation animationManager = this.gameObject.AddComponent<ManhattanAnimation>();

        GameObject fadeOverlay = GameObject.Find("FadeOverlay");
        //Debug.Log(fadeOverlay);
        //// time to do, delay
        //animationManager.StartAppearanceAnimation(fadeOverlay, Anim.Appear, 1.0f, true, 0.0f, 3.0f);
        //animationManager.StartAppearanceAnimation(fadeOverlay, Anim.Disappear, 1.0f, true, 0.0f, 6.0f);

        GameObject story1 = GameObject.Find("Story1");
        //animationManager.StartAppearanceAnimation(story1, Anim.Appear, 3.0f, true, 0.0f, 3.0f);
        //animationManager.StartAppearanceAnimation(story1, Anim.Disappear, 3.0f, true, 0.0f, 30.0f);

        // Create astronaut.
        // Vector3 astronoautPos = Camera.main.transform.position - Camera.main.transform.forward * 0.8f;
        // astronoautPos += new Vector3(0.0f, -astronoautPos.y, 0.0f);
        // GameObject astronaut = Instantiate(Resources.Load("Astronaut") as GameObject, astronoautPos, Quaternion.identity);
        // astronaut.transform.LookAt(Vector3.zero);

        return null;
    }
}
