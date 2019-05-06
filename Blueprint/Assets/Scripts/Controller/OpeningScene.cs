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

    // private int mountainSceneTime = 18;
    // private int forestSceneTime = 10;
    // private int pondSceneTime = 14;
    // private int beaconSceneTime = 15;
    private int mountainSceneTime = 15;
    private int forestSceneTime = 10;
    private int pondSceneTime = 20;
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
    private String story6 = "And the blueprints I found there. I think they might have something to do with this strange structure...";
    private String story7 = "Regardless, I need to send a message for help...";

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
        GameManager.Instance().uiStore.Subscribe(this);
    }

    private void intro() {
        introAnimation();
    }

    private void disableGridBehaviour() {
        Cursor.lockState = CursorLockMode.Locked;
        Highlight[] highlights = (Highlight[]) GameObject.FindObjectsOfType (typeof(Highlight));
        foreach (Highlight highlight in highlights){
            highlight.enabled = false;
        }
        MeshRenderer[] renderers = grid.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in renderers)
        {
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
        animationManager.StartAppearanceAnimation(blackOverlay, Anim.Disappear, 1.0f, true, 0.0f, 0.0f);

        mountainPath.GetComponent<CPC_CameraPath>().PlayPath(mountainSceneTime);
        yield return new WaitForSeconds(mountainSceneTime/3);
        setText(story, story1);
        animationManager.StartAppearanceAnimation(blackOverlay, Anim.Appear, 1f, true, 0.0f, 2*(mountainSceneTime/3) - 2f);
        animationManager.StartAppearanceAnimation(blackOverlay, Anim.Disappear, 1.0f, true, 0.0f, 2*(mountainSceneTime/3) - 0.2f);
        yield return new WaitForSeconds(2*(mountainSceneTime/3));
        Destroy(mountainWater);
        Destroy(deleteTrees);

        setText(story, story2);
        forestPath.GetComponent<CPC_CameraPath>().PlayPath(forestSceneTime);
        animationManager.StartAppearanceAnimation(blackOverlay, Anim.Appear, 1.0f, true, 0.0f, forestSceneTime - 2f);
        animationManager.StartAppearanceAnimation(blackOverlay, Anim.Disappear, 1.0f, true, 0.0f, forestSceneTime - 0.2f);
        yield return new WaitForSeconds(forestSceneTime);


        pondPath.GetComponent<CPC_CameraPath>().PlayPath(pondSceneTime);
        setText(story, story3);
        yield return new WaitForSeconds(pondSceneTime/2f);
        setText(story, story4);
        animationManager.StartAppearanceAnimation(blackOverlay, Anim.Appear, 1.0f, true, 0.0f, (pondSceneTime/2f) - 2.2f);
        animationManager.StartAppearanceAnimation(blackOverlay, Anim.Disappear, 1.0f, true, 0.0f, (pondSceneTime/2f) - 0.2f);
        yield return new WaitForSeconds((pondSceneTime/2f));



        // Reset camera to be within hex grid
        Camera.main.transform.position = player.transform.position + new Vector3(15,4,15);

        StartCoroutine(cameraSpin());
        setText(story, story5);
        yield return new WaitForSeconds(beaconSceneTime/3);
        setText(story, story6);
        yield return new WaitForSeconds(beaconSceneTime/3);
        setText(story, story7);
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

    private IEnumerator cameraSpin()
    {
      while (play)
      {
          yield return new WaitForSeconds(1.0f / 60.0f);
          Camera.main.transform.LookAt(new Vector3(0.0f, 2.0f, 0.0f));
          Camera.main.transform.RotateAround(Vector3.zero, Vector3.up, 0.15f);
      }
    }
}
