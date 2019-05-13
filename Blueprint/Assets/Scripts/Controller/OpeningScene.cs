using System;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Model;
using View;
using Model.Action;
using Model.Redux;
using Model.State;
using TMPro;
using Controller;
using UnityEngine.Experimental.UIElements;

public class OpeningScene : MonoBehaviour, Subscriber<UIState> {
    private GameObject dishBase;
    private bool introCompletionCheck;
    private GameObject mountainPath;
    private GameObject forestPath;
    private GameObject pondPath;
    private GameObject mountainWater;
    private GameObject deleteTrees;
    private GameObject blackOverlay;
    private GameObject grid;
    private Material grass;
    private ManhattanAnimation animationManager;
    private int mountainSceneTime = 12;
    private int forestSceneTime = 8;
    private int pondSceneTime = 15;
    private int beaconSceneTime = 25;
    private int totalIntroTime;

    private bool play = true;
    private GameObject player;
    private float currCountdownValue;
    private GameObject story;
    // Mountains
    private String story1 = "You have crash landed on a distant planet...";
    // Trees
    private String story2 = "The world feels familiar, yet you could not be further from home...";
    // Water
    private String story3 = "The landscape is untouched, dense with resources...";
    private String story4 = "While exploring this unfamiliar world, you stumble across a fenced off clearing...";
    // Pan Clearing
    private String story5 = "At the centre is an abandoned structure surrounded by mysterious documents...";
    private String story6 = "Upon further inspection, you realise they are blueprints for a communication beacon...";
    private String story7 = "Could this be your way home?";

    void Start() {
        totalIntroTime = mountainSceneTime + forestSceneTime + pondSceneTime + beaconSceneTime;

        dishBase = GameObject.Find("Beacon");
        mountainWater = GameObject.Find("MountainWater");
        deleteTrees = GameObject.Find("DeleteTrees");
        player = GameObject.Find("Player");
        blackOverlay = GameObject.Find("blackOverlay");
        grid = GameObject.Find("Grid");
        grass = Resources.Load<Material>("Grass");
        // blackOverlay.GetComponent<Image>().enabled = false;

        // Create astronaut.
        //Vector3 astronoautPos = player.transform.position;
        //astronoautPos += new Vector3(0.0f, -astronoautPos.y, 0.0f);
        //GameObject astronaut = Instantiate(Resources.Load("Astronaut") as GameObject, astronoautPos, Quaternion.identity);
        //astronaut.transform.LookAt(Vector3.zero);

        mountainPath = GameObject.Find("MountainPath");
        forestPath = GameObject.Find("ForestPath");
        pondPath = GameObject.Find("PondPath");

        story = GameObject.Find("Story");

        Invoke("intro",0.01f);
        Invoke("disableGridBehaviour", 1f);
        animationManager = this.gameObject.AddComponent<ManhattanAnimation>();
       
        // TODO: REMOVE THIS!
        SceneManager.LoadScene(SceneMapping.Tutorial);
//        GameManager.Instance().uiStore.Subscribe(this);
    }

    private void intro() {
        introAnimation();
    }

    private void disableGridBehaviour() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Highlight[] highlights = grid.GetComponentsInChildren<Highlight>();
        foreach (Highlight highlight in highlights){
            highlight.enabled = false;
        }
        MeshRenderer[] renderers = grid.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in renderers) {
            renderer.material = grass;
        }
    }

    public int GetIntroTime() {
        return this.totalIntroTime;
    }

    private void introAnimation() {
        StartCoroutine(StartCountdown(totalIntroTime + 0.5f));
        StartCoroutine(sceneRunner());
    }

    private void setText(GameObject story, String text) {
        story.GetComponent<TextMeshProUGUI>().SetText(text);
    }

    private void cameraReset() {
        Camera.main.transform.position = player.transform.position + new Vector3(0f, 0.9f, 0f);
        player.transform.rotation = Quaternion.Euler(0, 180, 0);
        Camera.main.transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    private IEnumerator StartCountdown(float countdownValue = 60) {
        currCountdownValue = countdownValue;
        while (currCountdownValue > 0) {
            yield return new WaitForSeconds(1.0f);
            currCountdownValue--;
        }
        play = false;
    }

    private IEnumerator sceneRunner() {
        Color color = story.GetComponent<TextMeshProUGUI>().color;
        color.a = 0f;

        // Black screen fades away
        animationManager.StartAppearanceAnimation(blackOverlay, Anim.Disappear, 1.0f, true, 0.0f, 0.0f);
        // Camera path begins as screen fades in
        mountainPath.GetComponent<CPC_CameraPath>().PlayPath(mountainSceneTime);
        yield return new WaitForSeconds(mountainSceneTime/3);
        // Text fades in after a short delay
        setText(story, story1);
        animationManager.StartAppearanceAnimation(story, Anim.Appear, 0.5f, true, 0.0f,0f);

        // Text fades out as screen fades out
        animationManager.StartAppearanceAnimation(story, Anim.Disappear, 1f, true, 0.0f, 2*(mountainSceneTime/3) - 2f);
        animationManager.StartAppearanceAnimation(blackOverlay, Anim.Appear, 1f, true, 0.0f, 2*(mountainSceneTime/3) - 2f);

        // Screen fades back in after delay into forest scene
        // Text fades back in after delay
        animationManager.StartAppearanceAnimation(blackOverlay, Anim.Disappear, 1.0f, true, 0.0f, 2*(mountainSceneTime/3) - 0.2f);
        animationManager.StartAppearanceAnimation(story, Anim.Appear, 0.5f, true, 0.0f, 2*(mountainSceneTime/3) - 0.2f);
        yield return new WaitForSeconds(2*(mountainSceneTime/3) - 0.2f);
        setText(story, story2);
        yield return new WaitForSeconds(0.2f);

        // Destroy unnecessary assets
        Destroy(mountainWater);
        Destroy(deleteTrees);

        // Next camera path plays
        forestPath.GetComponent<CPC_CameraPath>().PlayPath(forestSceneTime);
        // fade out of forest scene
        animationManager.StartAppearanceAnimation(story, Anim.Disappear, 1f, true, 0.0f, forestSceneTime - 2f);
        animationManager.StartAppearanceAnimation(blackOverlay, Anim.Appear, 1.0f, true, 0.0f, forestSceneTime - 2f);

        // fade into pond scene
        animationManager.StartAppearanceAnimation(blackOverlay, Anim.Disappear, 1.0f, true, 0.0f, forestSceneTime - 0.2f);
        yield return new WaitForSeconds(forestSceneTime);

        // Next camera path plays
        pondPath.GetComponent<CPC_CameraPath>().PlayPath(pondSceneTime);
        setText(story, story3);
        animationManager.StartAppearanceAnimation(story, Anim.Appear, 1f, true, 0.0f, 0f);
        
        animationManager.StartAppearanceAnimation(story, Anim.Disappear, 0.5f, true, 0.0f, 0.4f * pondSceneTime);
        yield return new WaitForSeconds(0.4f * pondSceneTime + 1f);
        setText(story, story4);
        animationManager.StartAppearanceAnimation(story, Anim.Appear, 0.5f, true, 0.0f, 0.0f);
        animationManager.StartAppearanceAnimation(story, Anim.Disappear, 1.0f, true, 0.0f, 0.6f * pondSceneTime - 2.4f);
        animationManager.StartAppearanceAnimation(blackOverlay, Anim.Appear, 1.0f, true, 0.0f, 0.6f * pondSceneTime - 2.4f);
        animationManager.StartAppearanceAnimation(blackOverlay, Anim.Disappear, 1.0f, true, 0.0f, 0.6f * pondSceneTime - 0.3f);
        yield return new WaitForSeconds(0.6f * pondSceneTime);
        setText(story, story5);
        animationManager.StartAppearanceAnimation(story, Anim.Appear, 1.0f, true, 0.0f, 0f);

        // Reset camera to be within hex grid
        Camera.main.transform.position = player.transform.position + new Vector3(15,4,15);

        StartCoroutine(cameraSpin());
        yield return new WaitForSeconds(beaconSceneTime/3);
        animationManager.StartAppearanceAnimation(story, Anim.Disappear, 0.5f, true, 0.0f, 0.0f);
        yield return new WaitForSeconds(1f);
        setText(story, story6);
        animationManager.StartAppearanceAnimation(story, Anim.Appear, 0.5f, true, 0.0f, 0f);
        yield return new WaitForSeconds(beaconSceneTime/3 - 1f);
        animationManager.StartAppearanceAnimation(story, Anim.Disappear, 0.5f, true, 0.0f, 0.0f);
        yield return new WaitForSeconds(1f);
        setText(story, story7);
        animationManager.StartAppearanceAnimation(story, Anim.Appear, 0.5f, true, 0.0f, 0f);

        animationManager.StartAppearanceAnimation(story, Anim.Disappear, 1f, true, 0.0f, (beaconSceneTime/3f) - 2f);
        animationManager.StartAppearanceAnimation(blackOverlay, Anim.Appear, 1.0f, true, 0.0f, (beaconSceneTime/3f) - 2f);
        yield return new WaitForSeconds((beaconSceneTime/3) + 1);
        setText(story, "");

        cameraReset();
        animationManager.StartAppearanceAnimation(blackOverlay, Anim.Disappear, 1.0f, true, 0.0f, 0.0f);
        GameManager.Instance().uiStore.Dispatch(new CloseUI());
    }

    public void StateDidUpdate(UIState state) {
        if (state.Selected == UIState.OpenUI.Playing) {
            GameManager.Instance().uiStore.Unsubscribe(this);
            SceneManager.LoadScene(SceneMapping.World);
        }
    }

    private IEnumerator cameraSpin() {
        while (play) {
            yield return new WaitForSeconds(1.0f / 60.0f);
            Camera.main.transform.LookAt(new Vector3(0.0f, 2.0f, 0.0f));
            Camera.main.transform.RotateAround(Vector3.zero, Vector3.up, 0.15f);
        }
    }
}
