using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class GenerateHex : MonoBehaviour {

  public HexMap hexmap;

  public enum Resource { Grass, Machinery, Rock };

  private Dictionary<Resource, GameObject> resourceMap;

  void Start ()
  {
      hexmap = new HexMap();

      resourceMap = new Dictionary<Resource, GameObject>();

      resourceMap[Resource.Grass] = Resources.Load("Hex_Tile") as GameObject;
      resourceMap[Resource.Machinery] = Resources.Load("Machinery") as GameObject;
      resourceMap[Resource.Rock] = Resources.Load("Rock") as GameObject;
      hexmap.Create(resourceMap);
  }

  // Update is called once per frame
  void Update ()
  {
      if (Input.GetKeyDown("'"))
      {
          hexmap.PlaceOnGrid(UnityEngine.Random.Range(10, 40), UnityEngine.Random.Range(10, 40), Quaternion.Euler(0, 0, 0), Resource.Machinery);
      }
  }
}
