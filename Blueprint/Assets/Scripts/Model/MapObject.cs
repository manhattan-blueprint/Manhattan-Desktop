using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Controller;
using Model;

/* Contans all useful data for the state store

   If you want to retrieve all info on the GameObject then use:
   MapObject mapObject = objectGrid[xCo, yCo].GetComponent(typeof(MapObject)) as MapObject;

   For lightweight uses, for example seeing what type of object are adjacent
   then you should be able to do:
   MapResource mapResource = objectGrid[xCo, yCo].GetComponent(typeof(MapResource)) as MapResource;

   for example */
namespace Model {
    public class MapObject : MonoBehaviour {
        public MapResource mapResource;

        // Whenever the holder is freshly created, as defined by MonoBehaviour
        void Start() {
        }

        // Attach useful info, such as unique behaviours for this object. May be
        // useful later, so good to have here
        void attach() {
        }
    }
}
