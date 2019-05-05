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
    private GameObject deleteTrees;

    private int mountainSceneTime = 18;
    private int forestSceneTime = 10;
    private int pondSceneTime = 14;
    private int beaconSceneTime = 15;
    private int totalIntroTime;

    private bool play = true;
    private GameObject player;
    private float currCountdownValue;
    private GameObject story;
    private String story1 = "The last thing I remember I was on the ship, the engines were failing...";
    private String story2 = "Now I'm not quite sure where I am...";
    private String story3 = "This place is much like home, and yet...";
    private String story4 = "I haven't found anyone else here, not a soul...";
    private String story5 = "Just this clearing...";
    private String story6 = "And the blueprints within. I think they might have something to do with this strange structure...";
    private String story7 = "Regardless, I need to send a message for help...";

    void Start() {
        totalIntroTime = mountainSceneTime + forestSceneTime + pondSceneTime + beaconSceneTime;

        dishBase = GameObject.Find("Beacon");
        mountainWater = GameObject.Find("MountainWater");
        deleteTrees = GameObject.Find("DeleteTrees");
        player = GameObject.Find("Player");

        mountainPath = GameObject.Find("MountainPath");
        forestPath = GameObject.Find("ForestPath");
        pondPath = GameObject.Find("PondPath");

        story = GameObject.Find("Story");

        introCompletionCheck = GameManager.Instance().uiStore.GetState().IntroComplete;
        if (!introCompletionCheck) {
            Invoke("intro",0.001f);
        }
    }

    private void intro()
    {
        GameManager.Instance().uiStore.Dispatch(new OpenIntroUI());
        introAnimation();
    }

    public int GetIntroTime() {
        return this.totalIntroTime;
    }

    private void introAnimation() {
        StartCoroutine(StartCountdown(totalIntroTime + 0.5f));
        StartCoroutine(sceneRunner());
    }

    private void setText(GameObject story, String text)
    {
        story.GetComponent<TextMeshProUGUI>().SetText(text);
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
        mountainPath.GetComponent<CPC_CameraPath>().PlayPath(mountainSceneTime);
        yield return new WaitForSeconds(mountainSceneTime/3);
        setText(story, story1);
        yield return new WaitForSeconds(2*(mountainSceneTime/3));
        Destroy(mountainWater);
        Destroy(deleteTrees);

        setText(story, story2);
        forestPath.GetComponent<CPC_CameraPath>().PlayPath(forestSceneTime);
        yield return new WaitForSeconds(forestSceneTime);

        pondPath.GetComponent<CPC_CameraPath>().PlayPath(pondSceneTime);
        setText(story, story3);
        yield return new WaitForSeconds(pondSceneTime/2f);
        setText(story, story4);
        yield return new WaitForSeconds((pondSceneTime/2f) + 0.1f);


        // Reset camera to be within hex grid
        Camera.main.transform.position = player.transform.position + new Vector3(15,4,15);
        StartCoroutine(beaconTextRunner());

        // Create astronaut.
        // Vector3 astronoautPos = Camera.main.transform.position - Camera.main.transform.forward * 0.8f;
        // astronoautPos += new Vector3(0.0f, -astronoautPos.y, 0.0f);
        // GameObject astronaut = Instantiate(Resources.Load("Astronaut") as GameObject, astronoautPos, Quaternion.identity);
        // astronaut.transform.LookAt(Vector3.zero);
        while (play)
        {
            yield return new WaitForSeconds(1.0f / 60.0f);
            Camera.main.transform.LookAt(new Vector3(0.0f, 2.0f, 0.0f));
            Camera.main.transform.RotateAround(Vector3.zero, Vector3.up, 0.15f);    
        }
        cameraReset();
        GameManager.Instance().uiStore.Dispatch(new CloseUI());
    }

    private IEnumerator beaconTextRunner()
    {
        setText(story, story5);
        yield return new WaitForSeconds(beaconSceneTime/3);
        setText(story, story6);
        yield return new WaitForSeconds(beaconSceneTime/3);
        setText(story, story7);
        yield return new WaitForSeconds(beaconSceneTime/3);
        ManhattanAnimation animationManager = this.gameObject.AddComponent<ManhattanAnimation>();
        setText(story, "");
        GameObject fadeOverlay = GameObject.Find("FadeOverlay");
        animationManager.StartAppearanceAnimation(fadeOverlay, Anim.Appear, 1.0f, true, 0.0f, 0.0f);
        animationManager.StartAppearanceAnimation(fadeOverlay, Anim.Disappear, 1.0f, true, 0.0f, 2.0f);
        while (play)
        {
            yield return new WaitForSeconds(1.0f / 60.0f);
            fadeOverlay.transform.position = Camera.main.transform.position + Camera.main.transform.forward;
            fadeOverlay.transform.LookAt(Camera.main.transform.position);
        }
    }
}
