using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Model;
using View;

/* Attached to MapGenerator and spawns map onto scene */
namespace Controller {
    public class HexMapController : MonoBehaviour {
        public HexMap hexMap;
        public static Dictionary<MapResource, GameObject> resourceMap = new Dictionary<MapResource, GameObject>();

        private void Start() {
            resourceMap[MapResource.Grass] = Resources.Load("Grass") as GameObject;
            resourceMap[MapResource.Mud] = Resources.Load("Mud") as GameObject;
            resourceMap[MapResource.Rock] = Resources.Load("Rock") as GameObject;
            resourceMap[MapResource.Machinery] = Resources.Load("Stone") as GameObject;
            resourceMap[MapResource.Tree1] = Resources.Load("Tree1") as GameObject;
            this.hexMap = new HexMap(resourceMap);
            Cursor.visible = false;
        }

/*        private void Update() {
            if (Input.GetKeyDown(KeyMapping.PlaceRandom)) {
                hexMap.PlaceOnGrid(UnityEngine.Random.Range(10, 40), UnityEngine.Random.Range(10, 40), Quaternion.Euler(0, 0, 0), MapResource.Machinery);
            }
        }*/
    }
}
