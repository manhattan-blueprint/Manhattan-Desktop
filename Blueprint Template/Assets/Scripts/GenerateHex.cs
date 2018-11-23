 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GenerateHex : MonoBehaviour {

    public HexMap hexmap = new HexMap();

    private Dictionary<string, GameObject> stringToObject = new Dictionary<string, GameObject>();

    void Start ()
    {
        stringToObject["grass"] = Resources.Load("p_hex_tile") as GameObject;
        stringToObject["machinery"] = Resources.Load("p_machinery") as GameObject;
        stringToObject["rock"] = Resources.Load("p_rock") as GameObject;
        hexmap.Create(stringToObject);
	}

	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown("e"))
        {
            hexmap.PlaceOnGrid(UnityEngine.Random.Range(10, 40), UnityEngine.Random.Range(10, 40), Quaternion.Euler(0, 0, 0), "machinery");
        }
    }
}
