using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Model;
using View;

/* Attached to MapGenerator and spawns map onto scene */
namespace Controller {
    public class HexMapController : MonoBehaviour {
        public HexMap hexmap;
        private Dictionary<MapResource, GameObject> resourceMap;

        void Start(){
            hexmap = new HexMap();
            resourceMap = new Dictionary<MapResource, GameObject>();
            resourceMap[MapResource.Grass] = Resources.Load("Hex_Tile") as GameObject;
            resourceMap[MapResource.Machinery] = Resources.Load("Machinery") as GameObject;
            resourceMap[MapResource.Rock] = Resources.Load("Rock") as GameObject;
            hexmap.Create(resourceMap);
        }

        // Update is called once per frame
        void Update() {
            if (Input.GetKeyDown("'")) {
                hexmap.PlaceOnGrid(UnityEngine.Random.Range(10, 40), UnityEngine.Random.Range(10, 40), Quaternion.Euler(0, 0, 0), MapResource.Machinery);
            }
        }
    }
}