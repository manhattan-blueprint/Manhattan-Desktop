using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GenerateHex : MonoBehaviour {

	// TODO: put these into the array. Static maybe?
	// private Int width = 10;
	// private Int Height = 10;

	// Placeable Objects
	public GameObject p_hex_grass;
	public GameObject p_machinery;

	private static int mapSize = 20;
	private static float hexH = 0.86602540f;

	// Grid containing coordinates of hexagon map.
	private Vector3[,] mapGrid = new Vector3[mapSize, mapSize];

	// Grid containing hex tile objects of hexagon map.
	private GameObject[,] hexGrid = new GameObject[mapSize, mapSize];

	// Grid containing objects placed on hexagon map.
	private GameObject[,] objectGrid = new GameObject[mapSize, mapSize];

	// Creates a grid of number coordinates, same reference as to the hexgrid of objects.
	void CreateGrid() {
		for (int i = 0; i < 20; i++) {
			for (int j = 0; j < 20; j++) {
				mapGrid[i, j] = new Vector3((float)(i * hexH * 2.0f + (hexH * (j%2))), UnityEngine.Random.Range(-1.2f, -0.8f), (float)(1.5 * j));
			}
		}
	}

	// Places an object on the grid
	void PlaceOnGrid(int xCo, int yCo, Quaternion rot, GameObject obj) {
		Vector3 objPos = new Vector3(mapGrid[xCo, yCo][0], mapGrid[xCo, yCo][1] + 1.5f, mapGrid[xCo, yCo][2]);
		// Debug.Log("Object placed at" + objPos);
		objectGrid[xCo, yCo] = (GameObject)Instantiate(obj, objPos, rot);
	}

	// Converts in game coordinates to nearest x grid coordinate.
	int XToCo(float xPos, float yPos) {
		int xCo = (int)Math.Round(xPos/(hexH * 2) - ((Math.Round(yPos) % 2) * hexH));
		return xCo;
	}

	// Converts in game coordinates to nearest y grid coordinate.
	int YToCo(float xPos, float yPos) {
		int yCo = (int)Math.Round(yPos/1.5f);
		return yCo;
	}

	// Given a player location and direction, places an object if ok
	void PlayerPlace(float xPos, float yPos, float zPos, Quaternion dir) {
		// Add in amount to relevant vectors given player facing direction.
		xPos = xPos + (float)(Math.Cos(dir[1]) * hexH * 2.0f);
		yPos = yPos + (float)(Math.Sin(dir[1] * 1.5f));

		// Find vector in mapGrid given position and direction,
		int nearestX = XToCo(xPos, yPos);
		int nearestY = YToCo(xPos, yPos);

		// Tile should not be allowed to be placed if player is too far away
		// (likely from Z direction than the others due to nature of this script)
		// Checks to see if the player is allowed to place the object type should take
		// place in the player code where the inventory is known.
		if (Math.Abs(zPos - mapGrid[nearestX, nearestY][2]) < 5.0f) {
			PlaceOnGrid(nearestX, nearestY, Quaternion.Euler(0, 0, 0), p_machinery);
		}

	}

	void PrintTests() {
		Debug.Log("XToCo");
		Debug.Log(XToCo(0.0f, 0.0f)); // 0
		Debug.Log(XToCo(1.732f * 1.0f, 0.0f)); // 1
		Debug.Log(XToCo(1.732f * 10.0f, 0.0f)); // 10
		Debug.Log(XToCo(0.866f, 1.5f)); // 0
		Debug.Log(XToCo(0.0f, 1.5f * 9.0f)); // 0

		Debug.Log("YToCo");
		Debug.Log(YToCo(0.0f, 0.0f)); // 0
		Debug.Log(YToCo(8.0f, 1.5f)); // 1
		Debug.Log(YToCo(12.0f, 15.0f)); // 10
	}

	void Start () {
		CreateGrid();

		Quaternion rotation = Quaternion.Euler(0, 90, 0);

		for (int i = 0; i < 20; i++) {
			for (int j = 0; j < 20; j++) {
				hexGrid[i, j] = (GameObject)Instantiate(p_hex_grass, mapGrid[i, j], rotation);
			}
		}

		for (int i = 0; i < 20; i++) {
			PlaceOnGrid(UnityEngine.Random.Range(0, mapSize), UnityEngine.Random.Range(0, mapSize), Quaternion.Euler(0, 0, 0), p_machinery);
		}

		PrintTests();
	}

	// Update is called once per frame
	void Update () {

	}
}
