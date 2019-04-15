using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGate : MonoBehaviour
{
  void OnTriggerEnter(Collider other) {
          Debug.Log("Its a hit!");
       }
}
