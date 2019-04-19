using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;
using Model.Action;
using Model.Redux;
using Model.State;

public class ExitGate : MonoBehaviour
{
  void OnTriggerEnter(Collider other) {
          GameManager.Instance().uiStore.Dispatch(new OpenGateUI());
       }
  void OnTriggerExit(Collider other) {
       GameManager.Instance().uiStore.Dispatch(new CloseUI());
    }

}
