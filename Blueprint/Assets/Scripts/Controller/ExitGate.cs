using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;
using Model.Action;
using Model.Redux;
using Model.State;

public class ExitGate : MonoBehaviour {
  GameObject player;
  float minDistance = 7;

  void Start() {
    player = GameObject.Find("Player");
  }

  void OnMouseOver() {
      float dist = Vector3.Distance(player.transform.position, transform.position);
      if (dist < minDistance && GameManager.Instance().uiStore.GetState().Selected == UIState.OpenUI.Playing) {
          GameManager.Instance().uiStore.Dispatch(new OpenMouseUI());
      } else if (dist > minDistance && GameManager.Instance().uiStore.GetState().Selected == UIState.OpenUI.Mouse) {
          GameManager.Instance().uiStore.Dispatch(new CloseUI());
      }
  }

  void OnMouseExit() {
      float dist = Vector3.Distance(player.transform.position, transform.position);
      if (GameManager.Instance().uiStore.GetState().Selected == UIState.OpenUI.Mouse) {
          GameManager.Instance().uiStore.Dispatch(new CloseUI());
      }
  }

  // void OnTriggerEnter(Collider other) {
  //     GameManager.Instance().uiStore.Dispatch(new OpenMouseUI());
  // }
  // void OnTriggerExit(Collider other) {
  //     GameManager.Instance().uiStore.Dispatch(new CloseUI());
  // }
}
