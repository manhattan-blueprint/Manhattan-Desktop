using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using View;

public class DecorCollision : MonoBehaviour {
    void OnTriggerEnter(Collider other) {
        if (((other.gameObject.GetComponent("Placeable") as Placeable) != null) || ((other.gameObject.GetComponent("MachinePlaceable") as MachinePlaceable) != null)) {
            Destroy(gameObject);
        }
    }
}
