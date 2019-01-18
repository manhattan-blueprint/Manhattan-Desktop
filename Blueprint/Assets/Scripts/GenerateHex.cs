using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class GenerateHex : MonoBehaviour {
    // This class instantiates the HexMap class

    public HexMap hexmap;

    public static Dictionary<MapResource, GameObject> resourceDict  = new Dictionary<MapResource, GameObject>();

    void Start ()
    {
        resourceDict[MapResource.Grass] = Resources.Load("Hex_Tile") as GameObject;
        resourceDict[MapResource.Machinery] = Resources.Load("Machinery") as GameObject;
        resourceDict[MapResource.Rock] = Resources.Load("Rock") as GameObject;

        hexmap = new HexMap();

        hexmap.Create(resourceDict);
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyDown("'"))
        {
            hexmap.PlaceOnGrid(UnityEngine.Random.Range(10, 40), UnityEngine.Random.Range(10, 40), Quaternion.Euler(0, 0, 0), MapResource.Machinery);
        }
    }
}
