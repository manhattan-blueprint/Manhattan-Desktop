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

        private void Start(){
            var resourceMap = new Dictionary<MapResource, GameObject> {
                [MapResource.Grass] = Resources.Load("Hex_Tile") as GameObject,
                [MapResource.Machinery] = Resources.Load("Machinery") as GameObject,
                [MapResource.Rock] = Resources.Load("Rock") as GameObject
            };
            this.hexMap = new HexMap(resourceMap);
        }

        private void Update() {
            if (Input.GetKeyDown(KeyMapping.PlaceRandom)) {
                hexMap.PlaceOnGrid(UnityEngine.Random.Range(10, 40), UnityEngine.Random.Range(10, 40), Quaternion.Euler(0, 0, 0), MapResource.Machinery);
            }
        }
    }
}