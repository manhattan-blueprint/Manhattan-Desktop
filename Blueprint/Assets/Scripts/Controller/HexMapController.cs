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
            resourceMap[MapResource.TreeA] = Resources.Load("TreeA") as GameObject;
            this.hexMap = new HexMap(resourceMap);
            Cursor.visible = false;
        }
    }
}
