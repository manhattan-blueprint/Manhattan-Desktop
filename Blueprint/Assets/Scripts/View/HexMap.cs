using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using Model;
using Controller;

namespace View {
    public class HexMap {
        private Dictionary<MapResource, GameObject> objects;
        private static int mapSize = 48;

        // Value of the distance from the origin of a hexmap to centre point
        // of each edge, given a hexagon where the distance of the origin
        // to each corner is one. This is equivalent to sqrt(3)/2.
        private const float hexH = 0.86602540378f;

        // This is the height of the block of grass from the midpoint, which
        // currently depends on the height of the prefab used for grass. Past
        // the MVP this will likely be made taller but can probably remain
        // hardcoded, depending on the direction we go with the terrain.
        private const float grassTopHeight = 10.0f;

        // Encapsulates the general randomness created in the procedurally
        // generated map.
        private const float randomness = 0.05f;

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
            // Debug.Log(DistFromCentre(0, 0));
            // Debug.Log(DistFromCentre(mapSize/2, mapSize/2));
        }

        // Generates a map based off of the Diamond-Square algorithm.
        private void GenerateMap() {
            // TODO: Make this seed off of usernasme
            UnityEngine.Random.seed = 42;
            float bump = randomness;

            float mounBorder = mapSize/2.0f;  // Radius of
            float hillBorder = mapSize/2.6f;  // Radius of hill slope area
            float grassBorder = mapSize/3.4f; // Radius of grass area

            for (int i = 0; i < mapSize; i++) {
                for (int j = 0; j < mapSize; j++) {
                    float dist = DistFromCentre(i, j);
                    float height = 0;
                    MapResource type = MapResource.Grass;
                    bool spawnWildlife = false;
                    MapResource wildlife = MapResource.Tree1;

                    if (dist < grassBorder) {
                        height = UnityEngine.Random.Range(-bump, bump) - grassTopHeight;

                        if (UnityEngine.Random.Range(0.0f, 1.0f) > 0.95f) {
                            type = MapResource.Mud;
                        }
                    }
                    else if (dist < hillBorder) {
                        height = (dist - grassBorder) + UnityEngine.Random.Range(-bump*5, bump*20) - grassTopHeight;

                        if (UnityEngine.Random.Range(0.0f, 1.0f) > 0.5f) {
                            type = MapResource.Rock;
                        }
                        if (UnityEngine.Random.Range(0.0f, 1.0f) > 0.7f) {
                            spawnWildlife = true;
                            wildlife = MapResource.Tree1;
                        }
                    }
                    else if (dist < mounBorder) {
                        height = (dist - grassBorder) + UnityEngine.Random.Range(0.0f, bump*50) - grassTopHeight;

                        if (UnityEngine.Random.Range(0.0f, 1.0f) > 0.05f) {
                            type = MapResource.Rock;
                        }

                        if (UnityEngine.Random.Range(0.0f, 1.0f) > 0.1f) {
                            spawnWildlife = true;
                            wildlife = MapResource.Tree1;
                        }
                    }

                    // Only want to spawn if inside outer circle
                    if (dist < mounBorder) {
                        SetAndInstantiate(i, j, height, type);
                    }

                    if (spawnWildlife == true) {
                        PlaceOnGrid(i, j, Quaternion.Euler(0, 0, 0), wildlife);
                    }
                }
            }

            // Experimental ideas, not working currently

            // // Raise the height of some peaks
            // for (int y = 0; y < 10; y ++) {
            //     double randAngle = UnityEngine.Random.Range(0, 2.0f * (float)Math.PI);
            //     int i = (int)(mounBorder + mounBorder * 0.9f * Math.Sin(randAngle));
            //     int j = (int)(mounBorder + mounBorder * 0.9f * Math.Cos(randAngle));
            //     // Debug.Log("Angle of " + randAngle + "giving sin of " + Math.Sin(randAngle) + " and cos of " + Math.Cos(randAngle));
            //     // Debug.Log("Updating height at i " + i + " j " + j);
            //     UpdateHeight(i, j, UnityEngine.Random.Range(5, 10));
            // }

            // Smooth the ground by averaging the height of each hexagon
            // for (int q = 0; q < 2; q++) {
            //     for (int i = 1; i < mapSize - 1; i++) {
            //         for (int j = 1; j < mapSize - 1; j++) {
            //             Debug.Log("Attemoting to average height at " + i + " " + j);
            //             AverageHeight(i, j);
            //         }
            //     }
            // }
        }

        private void AverageHeight(int xCo, int yCo) {
            if (hexGrid[xCo, yCo] != null) {
                float sum = 0.0f;
                float count = 0.0f;
                for (int i = -1; i < 2; i ++) {
                    for (int j = -1; j < 2; j ++) {
                        if (hexGrid[xCo + i, yCo + j] != null) {
                            sum += mapGrid[xCo + i, yCo + j].y;
                            count += 1.0f;
                        }
                    }
                }
                Debug.Log("Sum: " + sum + " Count: " + count);
                if (count > 0) {
                    Debug.Log("Updating the height of " + xCo + " " + yCo + " to " + sum/count);
                    UpdateHeight(xCo, yCo, sum/count);
                }
            }
        }

        // Get the distance from the centre (coordinate based rather than actual)
        private float DistFromCentre(int xCo, int yCo) {
            return (float)Math.Sqrt(Math.Pow(xCo - mapSize/2, 2) + Math.Pow(yCo - mapSize/2, 2));
        }

        // Sets the height in the grid and instantiates
        private void SetAndInstantiate(int xCo, int yCo, float height, MapResource type) {
            mapGrid[xCo, yCo].y = height;
            hexGrid[xCo, yCo] = MonoBehaviour.Instantiate(objects[type], mapGrid[xCo, yCo], rotation);
        }

        private void UpdateHeight(int xCo, int yCo, float height) {
            mapGrid[xCo, yCo].y = height;
            Vector3 temp = new Vector3(0, height, 0);
            hexGrid[xCo, yCo].transform.position += temp;
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
            float radius = 1.0f;

            GameObject gameObject = HexMapController.resourceMap[objectCode];

            if (gameObject.GetComponent<MapObject>() != null) {
                 radius = gameObject.GetComponent<MapObject>().modelHeight;
            }
            radius *= gameObject.transform.lossyScale.y;

            Vector3 objPos = new Vector3(mapGrid[xCo, yCo][0],
                                         mapGrid[xCo, yCo][1] + grassTopHeight + radius/2,
                                         mapGrid[xCo, yCo][2]);

            objectGrid[xCo, yCo] = MonoBehaviour.Instantiate(gameObject, objPos, rot);
        }

        // Places an object on the grid using floats
        public void PlaceOnGrid(float fxCo, float fyCo, Quaternion rot, MapResource objectCode) {
            int xCo = XToCo(fxCo, fyCo);
            int yCo = YToCo(fxCo, fyCo);
            PlaceOnGrid(xCo, yCo, rot, objectCode);
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
