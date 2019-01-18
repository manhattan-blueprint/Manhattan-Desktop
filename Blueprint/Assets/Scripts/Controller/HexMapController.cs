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

        private void Start(){
            resourceMap[MapResource.Grass] = Resources.Load("Hex_Tile") as GameObject;
            resourceMap[MapResource.Machinery] = Resources.Load("Machinery") as GameObject;
            resourceMap[MapResource.Rock] = Resources.Load("Rock") as GameObject;
            this.hexMap = new HexMap(resourceMap);
        }

        private void Update() {
            if (Input.GetKeyDown(KeyMapping.PlaceRandom)) {
                hexMap.PlaceOnGrid(UnityEngine.Random.Range(10, 40), UnityEngine.Random.Range(10, 40), Quaternion.Euler(0, 0, 0), MapResource.Machinery);
            }
        }
    }
}
