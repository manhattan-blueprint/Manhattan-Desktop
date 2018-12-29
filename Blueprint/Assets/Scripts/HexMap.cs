using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

public class HexMap
{
    // This class holds the map that is used across the whole world.

    // Objects used in map
    private Dictionary<GenerateHex.Resource, GameObject> objects;

    private static int mapSize = 50;

    // Value of the distance from the origin of a hexmap to centre point
    // of each edge, given a hexagon where the distance of the origin
    // to each corner is one. This is equivalent to sqrt(3)/2.
    private static float hexH = 0.86602540378f;

    // Grid containing coordinates of hexagon map.
    private Vector3[,] mapGrid;

    // Grid containing hex tile objects of hexagon map.
    private static GameObject[,] hexGrid = new GameObject[mapSize, mapSize];

    // Grid containing objects placed on hexagon map.
    private static GameObject[,] objectGrid = new GameObject[mapSize, mapSize];

    public void Create(Dictionary<GenerateHex.Resource, GameObject> resourceToObjectMap)
    {
        objects = resourceToObjectMap;

        mapGrid = CreateGrid();

        Quaternion rotation = Quaternion.Euler(0, 90, 0);

        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                if (i > 10 && i < mapSize - 10 && j > 10 && j < mapSize - 10)
                {
                    // Bumpy grass outer
                    mapGrid[i, j].y = UnityEngine.Random.Range(-1.1f, -1.0f);
                    hexGrid[i, j] = MonoBehaviour.Instantiate(objects[GenerateHex.Resource.Grass], mapGrid[i, j], rotation);
                }
                else
                {
                    mapGrid[i, j].y = UnityEngine.Random.Range(-5.0f, .0f);
                    hexGrid[i, j] = MonoBehaviour.Instantiate(objects[GenerateHex.Resource.Rock], mapGrid[i, j], rotation);
                }
            }
        }

        // First outer ring, bumpy grass
        for (int i = 10; i < 15; i++)
        {
            for (int j = 10; j < 15; j++)
            {
                mapGrid[i, j].y = UnityEngine.Random.Range(-1.0f, -0.8f);
                hexGrid[i, j] = MonoBehaviour.Instantiate(objects[GenerateHex.Resource.Grass], mapGrid[i, j], rotation);
                mapGrid[mapSize - i, mapSize - j].y = UnityEngine.Random.Range(-1.0f, -0.8f);
                hexGrid[mapSize - i, mapSize - j] = MonoBehaviour.Instantiate(objects[GenerateHex.Resource.Grass], mapGrid[i, j], rotation);
            }
        }

        for (int i = 0; i < 20; i++)
        {
            PlaceOnGrid(UnityEngine.Random.Range(10, 40), UnityEngine.Random.Range(10, 40), Quaternion.Euler(0, 0, 0), GenerateHex.Resource.Machinery);
        }

        // PrintTests();
    }

    // Places an object on the grid according to placement system of ints and map
    public void PlaceOnGrid(int xCo, int yCo, Quaternion rot, GenerateHex.Resource objectCode )
    {
        Vector3 objPos = new Vector3(mapGrid[xCo, yCo][0], mapGrid[xCo, yCo][1] + 1.5f, mapGrid[xCo, yCo][2]);
        objectGrid[xCo, yCo] = GameObject.Instantiate(objects[objectCode], objPos, rot);
    }

    // Places an object on the grid using floats
    public void PlaceOnGrid(float fxCo, float fyCo, Quaternion rot, GenerateHex.Resource objectCode)
    {
        int xCo = XToCo(fxCo, fyCo);
        int yCo = YToCo(fxCo, fyCo);
        Vector3 objPos = new Vector3(mapGrid[xCo, yCo][0], mapGrid[xCo, yCo][1] + 1.5f, mapGrid[xCo, yCo][2]);
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
    private static Vector3[,] CreateGrid()
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
