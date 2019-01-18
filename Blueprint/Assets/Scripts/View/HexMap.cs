using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using Model;

namespace View {
    public class HexMap {
        private Dictionary<MapResource, GameObject> objects;
        private static int mapSize = 50;

        // Value of the distance from the origin of a hexmap to centre point
        // of each edge, given a hexagon where the distance of the origin
        // to each corner is one. This is equivalent to sqrt(3)/2.
        private const float hexH = 0.86602540378f;

        // This is the height of the block of grass from the midpoint, which
        // currently depends on the height of the prefab used for grass. Past
        // the MVP this will likely be made taller but can probably remain
        // hardcoded, depending on the direction we go with the terrain.
        private const float grassTopHeight = 1.5f;

        // Grid containing coordinates of hexagon map.
        private Vector3[,] mapGrid;

        // Grid containing hex tile objects of hexagon map.
        private GameObject[,] hexGrid;

        // Grid containing objects placed on hexagon map.
        private MapObject[,] objectGrid;

        public HexMap(Dictionary<MapResource, GameObject> resourceToObjectMap) {
            objects = resourceToObjectMap;
            hexGrid = new GameObject[mapSize, mapSize];
            objectGrid = new MapObject[mapSize, mapSize];
            mapGrid = CreateGrid();
            Quaternion rotation = Quaternion.Euler(0, 90, 0);

            // Generate Construction Area
            int bumpyWidth = 10;
            for (int i = 0; i < mapSize; i++) {
                for (int j = 0; j < mapSize; j++) {
                    if (i > bumpyWidth && i < mapSize - bumpyWidth && j > bumpyWidth && j < mapSize - bumpyWidth) {
                        mapGrid[i, j].y = UnityEngine.Random.Range(-1.1f, -1.0f);
                        hexGrid[i, j] = MonoBehaviour.Instantiate(objects[MapResource.Grass], mapGrid[i, j], rotation);
                    } else {
                        // Bumpy outer grass on the edges of the map.
                        mapGrid[i, j].y = UnityEngine.Random.Range(-5.0f, .0f);
                        hexGrid[i, j] = MonoBehaviour.Instantiate(objects[MapResource.Rock], mapGrid[i, j], rotation);
                    }
                }
            }

            // Randomly generate machinery
            int numOfMachines = 20;
            for (int i = 0; i < numOfMachines; i++) {
                PlaceOnGrid(UnityEngine.Random.Range(10, 40),
                UnityEngine.Random.Range(10, 40),
                Quaternion.Euler(0, 0, 0), MapResource.Machinery);
            }
        }

        // Creates a grid of number coordinates, same reference as to the hexgrid of objects.
        private Vector3[,] CreateGrid() {
            Vector3[,] newMapGrid = new Vector3[mapSize, mapSize];
            for (int i = 0; i < mapSize; i++) {
                for (int j = 0; j < mapSize; j++) {
                    newMapGrid[i, j] = new Vector3((float)(i * hexH * 2.0f + (hexH * (j % 2))), 0.0f, (float)(1.5f * j));
                }
            }
            return newMapGrid;
        }

        // Converts in game coordinates to nearest x grid coordinate.
        private int XToCo(float xPos, float yPos) {
            return (int)Math.Round((xPos - (YToCo(xPos, yPos) % 2) * hexH)/ (hexH * 2));
        }

        // Converts in game coordinates to nearest y grid coordinate.
        private int YToCo(float xPos, float yPos) {
            return (int)Math.Round(yPos / 1.5f);
        }

        // Places an object on the grid according to placement system of ints and map
        public void PlaceOnGrid(int xCo, int yCo, Quaternion rot, MapResource objectCode) {
            Vector3 objPos = new Vector3(mapGrid[xCo, yCo][0], mapGrid[xCo, yCo][1] + grassTopHeight, mapGrid[xCo, yCo][2]);
            objectGrid[xCo, yCo] = new MapObject(objectCode, objPos, rot);
        }

        // Places an object on the grid using floats
        public void PlaceOnGrid(float fxCo, float fyCo, Quaternion rot, MapResource objectCode) {
            int xCo = XToCo(fxCo, fyCo);
            int yCo = YToCo(fxCo, fyCo);
            Vector3 objPos = new Vector3(mapGrid[xCo, yCo][0], mapGrid[xCo, yCo][1] + grassTopHeight, mapGrid[xCo, yCo][2]);
            objectGrid[xCo, yCo] = new MapObject(objectCode, objPos, rot);
        }

        // Removes an object from the grid according to placement system of ints
        // and map
        public void RemoveFromGrid(int xCo, int yCo) {
            objectGrid[xCo, yCo].Remove();
        }

        // Removes an object on the grid using floats
        public void RemoveFromGrid(float fxCo, float fyCo) {
            int xCo = XToCo(fxCo, fyCo);
            int yCo = YToCo(fxCo, fyCo);
            objectGrid[xCo, yCo].Remove();
        }

        // Returns all of the objects and their associated attributes.
        // TODO: Check that this should is not editable once passed to other
        // classes
        public MapObject[,] RetrieveGrid() {
            return objectGrid;
        }

        // Returns true if an object already exists on the map at coordinate
        // input location
        public bool objectPresent(int xCo, int yCo)
        {
            return objectGrid[xCo, yCo].instantiated;
        }
    }

}
