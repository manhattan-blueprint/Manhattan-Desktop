using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using Model;
using Controller;

namespace View {
    public class HexMap {
        private Dictionary<MapResource, GameObject> objects;
        private static int mapSize = 65;

        // Value of the distance from the origin of a hexmap to centre point
        // of each edge, given a hexagon where the distance of the origin
        // to each corner is one. This is equivalent to sqrt(3)/2.
        private const float hexH = 0.86602540378f;

        // This is the height of the block of grass from the midpoint, which
        // currently depends on the height of the prefab used for grass. Past
        // the MVP this will likely be made taller but can probably remain
        // hardcoded, depending on the direction we go with the terrain.
        private const float grassTopHeight = 1.5f;

        // Encapsulates the general randomness created in the procedurally
        // generated map.
        private const float randomness = 0.0f;

        // Default rotation due to desire for squares to fit nicer in the
        // hexagons
        Quaternion rotation = Quaternion.Euler(0, 90, 0);

        // Grid containing coordinates of hexagon map.
        private Vector3[,] mapGrid;

        // Grid containing hex tile objects of hexagon map.
        private GameObject[,] hexGrid;

        // Grid containing objects placed on hexagon map.
        private GameObject[,] objectGrid;

        public HexMap(Dictionary<MapResource, GameObject> resourceToObjectMap) {
            objects = resourceToObjectMap;
            hexGrid = new GameObject[mapSize, mapSize];
            objectGrid = new GameObject[mapSize, mapSize];
            mapGrid = CreateGrid();

            GenerateMap();
        }

        // Generates a map based off of the Diamond-Square algorithm.
        private void GenerateMap() {
            // TODO: Make this seed off of username
            UnityEngine.Random.seed = 42;
            float bump = randomness;

            // Seed the four corners
            SetAndInstantiate(0, 0, UnityEngine.Random.Range(-bump, bump));
            SetAndInstantiate(0, mapSize-1, UnityEngine.Random.Range(-bump, bump));
            SetAndInstantiate(mapSize-1, 0, UnityEngine.Random.Range(-bump, bump));
            SetAndInstantiate(mapSize-1, mapSize-1, UnityEngine.Random.Range(-bump, bump));

            int size = mapSize - 1;
            while(size > 1) {
                for (int i = 0; i < mapSize - 1; i += size) {
                    for (int j = 0; j < mapSize - 1; j += size) {
                        SquareStep(i, j, size, bump);
                    }
                }

                // Same as SquareStep but shifted up one step
                for (int i = 0; i < mapSize - 1; i += size) {
                    for (int j = -size/2; j < mapSize - 1; j += size) {
                        DiamondStep(i, j, size, bump);
                    }
                }

                // Same as SquareStep but shifted left one step
                for (int i = -size/2; i < mapSize - 1; i += size) {
                    for (int j = 0; j < mapSize - 1; j += size) {
                        DiamondStep(i, j, size, bump);
                    }
                }

                bump /= 2;
                size /= 2;
            }
        }

        // Performs the square step of the Diamond-Square algorithm.
        private void SquareStep(int x, int y, int size, float bump) {
            // Debug.Log("Performing SquareStep on X: " + x + " Y: " + y + " size: " + size);

            // Need to find the average height of available coordinates first.
            float count = 0.0f;
            float sum = 0.0f;
            int ms = mapSize - 1;
            if (0 <= x && x <= ms) {
                if (0 <= y && y <= ms) {
                    sum += mapGrid[x, y].y;
                    count += 1.0f;
                }
                if (0 <= y + size && y + size <= ms) {
                    sum += mapGrid[x, y + size].y;
                    count += 1.0f;
                }
            }
            if (0 <= x + size && x + size <= ms) {
                if (0 <= y && y <= ms) {
                    sum += mapGrid[x + size, y].y;
                    count += 1.0f;
                }
                if (0 <= y + size && y + size <= ms) {
                    sum += mapGrid[x + size, y + size].y;
                    count += 1.0f;
                }
            }

            float height = (sum/count) + UnityEngine.Random.Range(-bump, bump);
            SetAndInstantiate(x + size/2, y + size/2, height);
        }

        // Performs the diamond step of the Diamond-Square algorithm.
        private void DiamondStep(int x, int y, int size, float bump) {
            // Debug.Log("Performing DiamondStep on X: " + x + " Y: " + y + " size: " + size);

            // Find the average height of available coordinates.
            float count = 0.0f;
            float sum = 0.0f;
            int ms = mapSize - 1;
            if (0 <= x + size/2 && x + size/2 <= ms) {
                if (0 <= y + size/2 && y + size/2 <= ms) {
                    sum += mapGrid[x + size/2, y + size/2].y;
                    count += 1.0f;
                }
                if (0 <= y + size && y + size <= ms) {
                    sum += mapGrid[x + size/2, y + size].y;
                    count += 1.0f;
                }
            }
            if (0 <= x + size && x + size <= ms) {
                if (0 <= y + size/2 && y + size/2 <= ms) {
                    sum += mapGrid[x + size, y + size/2].y;
                    count += 1.0f;
                }
                if (0 <= y + size && y + size <= ms) {
                    sum += mapGrid[x + size, y + size].y;
                    count += 1.0f;
                }
            }

            float height = (sum/count) + UnityEngine.Random.Range(-bump, bump);
            SetAndInstantiate(x + size/2, y + size/2, height);
        }

        // Get the distance from the centre (coordinate based rather than actual)
        private float DistFromCentre(int xCo, int yCo) {
            return (float)Math.Sqrt(Math.Pow(xCo - mapSize/2, 2) + Math.Pow(yCo - mapSize/2, 2));
        }

        // Sets the height in the grid and instantiates
        private void SetAndInstantiate(int xCo, int yCo, float height) {
            // Debug.Log("Creating block at xCo: " + xCo + " yCo: " + yCo);
            mapGrid[xCo, yCo].y = height - 10.0f;
            // mapGrid[xCo, yCo].y = -10.0f;

            // Make things near the edges succeptible to additional heigh modifiers
            // float dist = DistFromCentre(xCo, yCo);
            // if (dist >= mapSize/2) {
            //     mapGrid[xCo, yCo].y += UnityEngine.Random.Range(0, (dist * dist) / (mapSize * mapSize));
            // }

            hexGrid[xCo, yCo] = MonoBehaviour.Instantiate(objects[MapResource.Grass], mapGrid[xCo, yCo], rotation);

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
            Vector3 objPos = new Vector3(mapGrid[xCo, yCo][0],
                                         mapGrid[xCo, yCo][1] + grassTopHeight,
                                         mapGrid[xCo, yCo][2]);

            objectGrid[xCo, yCo] = MonoBehaviour.Instantiate(HexMapController.resourceMap[objectCode],
                                                             objPos,
                                                             rot);
        }

        // Places an object on the grid using floats
        public void PlaceOnGrid(float fxCo, float fyCo, Quaternion rot, MapResource objectCode) {
            int xCo = XToCo(fxCo, fyCo);
            int yCo = YToCo(fxCo, fyCo);
            PlaceOnGrid(xCo, yCo, rot, objectCode);
        }

        // Places an object on the grid according to placement system of ints and map
        public void PlaceOnGrid(int xCo, int yCo, Quaternion rot, GameObject item) {
            Vector3 objPos = new Vector3(mapGrid[xCo, yCo][0],
                mapGrid[xCo, yCo][1] + grassTopHeight,
                mapGrid[xCo, yCo][2]);

            objectGrid[xCo, yCo] = MonoBehaviour.Instantiate(item,
                objPos,
                rot);
        }

        // Places an object on the grid using floats
        public void PlaceOnGrid(float fxCo, float fyCo, Quaternion rot, GameObject item) {
            int xCo = XToCo(fxCo, fyCo);
            int yCo = YToCo(fxCo, fyCo);
            PlaceOnGrid(xCo, yCo, rot, item);
        }

        // Removes an object from the grid according to placement system of ints
        // and map
        public void RemoveFromGrid(int xCo, int yCo) {
            MonoBehaviour.Destroy(objectGrid[xCo, yCo]);
        }

        // Removes an object on the grid using floats
        public void RemoveFromGrid(float fxCo, float fyCo) {
            int xCo = XToCo(fxCo, fyCo);
            int yCo = YToCo(fxCo, fyCo);
            RemoveFromGrid(xCo, yCo);
        }

        // Returns all of the objects and their associated attributes.
        // TODO: Check that this is not editable once passed to other classes
        public GameObject[,] RetrieveObjects() {
            return objectGrid;
        }

        // Returns true if an object already exists on the map at coordinate
        // input location
        public bool ObjectPresent(int xCo, int yCo) {
            if (objectGrid[xCo, yCo] != null) { return true; }
            return false;
        }
    }
}
