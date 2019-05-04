using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorCollision : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
