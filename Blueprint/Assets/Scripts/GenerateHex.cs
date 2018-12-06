using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GenerateHex : MonoBehaviour {

    public HexMap hexmap = new HexMap();

    private Dictionary<string, GameObject> stringToObject = new Dictionary<string, GameObject>();

    void Start ()
    {
        stringToObject["Grass"] = Resources.Load("Hex_Tile") as GameObject;
        stringToObject["Machinery"] = Resources.Load("Machinery") as GameObject;
        stringToObject["Rock"] = Resources.Load("Rock") as GameObject;
        hexmap.Create(stringToObject);
	}

	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown("e"))
        {
            hexmap.PlaceOnGrid(UnityEngine.Random.Range(10, 40), UnityEngine.Random.Range(10, 40), Quaternion.Euler(0, 0, 0), "Machinery");
        }
        if (Input.GetKeyDown("r"))
        {
            hexmap.PlaceOnGrid(UnityEngine.Random.Range(10, 40), UnityEngine.Random.Range(10, 40), Quaternion.Euler(0, 0, 0), "Grass");
        }
    }
}
