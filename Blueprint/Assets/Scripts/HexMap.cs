using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class HexMap
{
    // This class holds the map that is used across the whole world.

    // Objects used in map
    private Dictionary<GenerateHex.Resource, GameObject> objects;

    // Needs to be static as used in array creation.
    private static int mapSize = 50;

    // Value of the distance from the origin of a hexmap to centre point
    // of each edge, given a hexagon where the distance of the origin
    // to each corner is one. This is equivalent to sqrt(3)/2.
    private float hexH = 0.86602540378f;

    // This is the height of the block of grass from the midpoint, which
    // currently depends on the height of the prefab used for grass. Past
    // the MVP this will likely be made taller but can probably remain
    // hardcoded, depending on the direction we go with the terrain.
    float grassTopHeight = 1.5f;

    // Grid containing coordinates of hexagon map.
    private Vector3[,] mapGrid;

    // Grid containing hex tile objects of hexagon map.
    private GameObject[,] hexGrid;

    // Grid containing objects placed on hexagon map.
    private GameObject[,] objectGrid;

    public void Create(Dictionary<GenerateHex.Resource, GameObject> resourceToObjectMap)
    {
        objects = resourceToObjectMap;

        hexGrid = new GameObject[mapSize, mapSize];

        objectGrid = new GameObject[mapSize, mapSize];

        mapGrid = CreateGrid();

        Quaternion rotation = Quaternion.Euler(0, 90, 0);

        ////////////////////////////////////////////////////////////////////////
        // Basic procedural system for generating the MVP construction area,
        // almost certainly won't be used past the MVP.
        int bumpyWidth = 10;
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                if (i > bumpyWidth && i < mapSize - bumpyWidth && j > bumpyWidth && j < mapSize - bumpyWidth)
                {
                    mapGrid[i, j].y = UnityEngine.Random.Range(-1.1f, -1.0f);
                    hexGrid[i, j] = MonoBehaviour.Instantiate(objects[GenerateHex.Resource.Grass], mapGrid[i, j], rotation);
                }
                else
                {
                    // Bumpy outer grass on the edges of the map.
                    mapGrid[i, j].y = UnityEngine.Random.Range(-5.0f, .0f);
                    hexGrid[i, j] = MonoBehaviour.Instantiate(objects[GenerateHex.Resource.Rock], mapGrid[i, j], rotation);
                }
            }
        }

        // Place some random machinery just to make it feel more dynamic; can
        // remove this bit if deemed unnecessary.
        int numOfMachines = 20;
        Debug.Log(bumpyWidth);
        Debug.Log(mapSize-bumpyWidth);
        for (int i = 0; i < numOfMachines; i++)
        {
            PlaceOnGrid(UnityEngine.Random.Range(10, 40),
              UnityEngine.Random.Range(10, 40),
              Quaternion.Euler(0, 0, 0), GenerateHex.Resource.Machinery);
        }
        // End MVP only area
        ////////////////////////////////////////////////////////////////////////

        Debug.Log(objectGrid[0,0]);
    }

    // Places an object on the grid according to placement system of ints and map
    public void PlaceOnGrid(int xCo, int yCo, Quaternion rot, GenerateHex.Resource objectCode)
    {
        Vector3 objPos = new Vector3(mapGrid[xCo, yCo][0], mapGrid[xCo, yCo][1] + grassTopHeight, mapGrid[xCo, yCo][2]);
        objectGrid[xCo, yCo] = GameObject.Instantiate(objects[objectCode], objPos, rot);
    }

    // Removes an object from the grid according to placement system of ints and map
    public void RemoveFromGrid(int xCo, int yCo, Quaternion rot, GenerateHex.Resource objectCode)
    {
        GameObject.Destroy(objectGrid[xCo,yCo]);
    }

    // Places an object on the grid using floats
    public void PlaceOnGrid(float fxCo, float fyCo, Quaternion rot, GenerateHex.Resource objectCode)
    {
        int xCo = XToCo(fxCo, fyCo);
        int yCo = YToCo(fxCo, fyCo);
        Vector3 objPos = new Vector3(mapGrid[xCo, yCo][0], mapGrid[xCo, yCo][1] + grassTopHeight, mapGrid[xCo, yCo][2]);
        objectGrid[xCo, yCo] = GameObject.Instantiate(objects[objectCode], objPos, rot);
    }

    // Converts in game coordinates to nearest x grid coordinate.
    private int XToCo(float xPos, float yPos)
    {
        return (int)Math.Round(xPos / (hexH * 2) - ((Math.Round(yPos) % 2) * hexH));
    }

    // Converts in game coordinates to nearest y grid coordinate.
    private int YToCo(float xPos, float yPos)
    {
        return (int)Math.Round(yPos / 1.5f);
    }

    // Creates a grid of number coordinates, same reference as to the hexgrid of objects.
    private Vector3[,] CreateGrid()
    {
        Vector3[,] newMapGrid = new Vector3[mapSize, mapSize];
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                newMapGrid[i, j] = new Vector3((float)(i * hexH * 2.0f + (hexH * (j % 2))), 0.0f, (float)(1.5f * j));
            }
        }
        return newMapGrid;
    }
}
