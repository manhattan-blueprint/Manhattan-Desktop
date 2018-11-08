using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateHex : MonoBehaviour {

	// TODO: put these into the array. Static maybe?
	// private Int width = 10;
	// private Int Height = 10;

	public GameObject p_hex_tile;
  public Vector3 spawnSpot = new Vector3(1,1,0);

	// Grid containing coordinates of hexagon map.
	private Vector3[,] spawnGrid = new Vector3[20, 20];

	// Grid containing objects of hexagon map.
	private GameObject[,] hexGrid = new GameObject[20, 20];


	// Creates a grid of number coordinates, same reference as to the hexgrid of objects.
	void CreateGrid() {
		for (int i = 0; i < 20; i++) {
			for (int j = 0; j < 20; j++) {
				spawnGrid[i, j] = new Vector3((float)(-20 + i * 1.73205080 + (0.86602540 * (j%2))), -1.0f, -20 + (float)(1.5 * j));
				// spawnGrid[i, j] = new Vector3(0f, 0f, 0f);
			}
		}
	}

	void Start () {
		CreateGrid();

		Quaternion rotation = Quaternion.Euler(0, 90, 0);

		for (int i = 0; i < 20; i++) {
			for (int j = 0; j < 20; j++) {
				hexGrid[i, j] = (GameObject)Instantiate(p_hex_tile, spawnGrid[i, j], rotation);
			}
		}

	}

	// Update is called once per frame
	void Update () {

	}
}
