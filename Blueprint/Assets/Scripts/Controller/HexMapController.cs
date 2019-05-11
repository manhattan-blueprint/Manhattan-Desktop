using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Redux;
using Model.State;
using View;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

/* Attached to MapGenerator and spawns map onto scene */
namespace Controller {
    public class HexMapController : MonoBehaviour, Subscriber<MapState>, Subscriber<MachineState> {
        [SerializeField] private Material wireMaterial;
        private int gridSize = 18;
        private float previousX = 0;
        private float previousZ = 0;
        
        private Dictionary<Vector2, GameObject> grid;
        private Dictionary<Vector2, GameObject> objectsPlaced;
        private List<GameObject> wires;
        
        private void Start() {
            this.grid = new Dictionary<Vector2, GameObject>();
            this.objectsPlaced = new Dictionary<Vector2, GameObject>();
            this.wires = new List<GameObject>();
            drawMap();
            
            GameManager.Instance().mapStore.Subscribe(this);
            GameManager.Instance().machineStore.Subscribe(this);
        }
        
        private void drawMap() {
            GameObject hexTile = Resources.Load("hex_cell") as GameObject;
            Quaternion rotation = Quaternion.Euler(0, 90, 0);

            // Draw origin hexagon without placeable script
            GameObject cell = Instantiate(hexTile, new Vector3(0, -0.5f, 0), rotation);
            cell.transform.parent = this.gameObject.transform;
            Vector2 position = new Vector2(0, 0);
            cell.GetComponentInChildren<Highlight>().enabled = false;
            grid.Add(position, cell);
          
            
            for (int l = 1; l < gridSize; l++) {
                // Move to correct layer, top left of origin
                
                // Not allowed to place on layers 1 or 2
                bool shouldBePlaceable = !(l == 1 || l == 2);
                
                cell = Instantiate(hexTile, new Vector3(l * - (float) Math.Sqrt(3) / 2, -0.5f, l * 1.5f), rotation);
                position = new Vector2(-l, l);
                cell.transform.parent = this.gameObject.transform;
                if (shouldBePlaceable) {
                    cell.AddComponent<HexCell>().SetPosition(position);
                } else {
                    cell.GetComponentInChildren<Highlight>().enabled = false;
                }

                grid.Add(position, cell);
                
                setPreviousCoords(cell);

                // Move right
                for (int i = 1; i < l + 1; i++) {
                    cell = Instantiate(hexTile, new Vector3(previousX + i * (float) Math.Sqrt(3), -0.5f, previousZ), rotation);
                    position = new Vector2(-l + i, l);
                    if (shouldBePlaceable) {
                        cell.AddComponent<HexCell>().SetPosition(position);
                    } else {
                        cell.GetComponentInChildren<Highlight>().enabled = false;
                    }
                    cell.transform.parent = this.gameObject.transform;
                    grid.Add(position, cell);
                }
                setPreviousCoords(cell);

                // Move bottom right
                for (int i = 1; i < l + 1; i++) {
                    cell = Instantiate(hexTile, new Vector3(previousX + i * (float) Math.Sqrt(3) / 2, -0.5f, previousZ - i * 1.5f), rotation);
                    position = new Vector2(i, l-i);
                    if (shouldBePlaceable) {
                        cell.AddComponent<HexCell>().SetPosition(position);
                    } else {
                        cell.GetComponentInChildren<Highlight>().enabled = false;
                    }
                    cell.transform.parent = this.gameObject.transform;
                    grid.Add(position, cell);
                }
                setPreviousCoords(cell);

                // Move bottom left
                for (int i = 1; i < l + 1; i++) {
                    cell = Instantiate(hexTile, new Vector3(previousX - i * (float) Math.Sqrt(3) / 2, -0.5f, previousZ - i * 1.5f), rotation);
                    position = new Vector2(l, -i);
                    if (shouldBePlaceable) {
                        cell.AddComponent<HexCell>().SetPosition(position);
                    } else {
                        cell.GetComponentInChildren<Highlight>().enabled = false;
                    }
                    cell.transform.parent = this.gameObject.transform;
                    grid.Add(position, cell);
                }
                setPreviousCoords(cell);

                // Move left
                for (int i = 1; i < l + 1; i++) {
                    cell = Instantiate(hexTile, new Vector3(previousX - i * (float) Math.Sqrt(3), -0.5f, previousZ), rotation);
                    position = new Vector2(l-i, -l);
                    if (shouldBePlaceable) {
                        cell.AddComponent<HexCell>().SetPosition(position);
                    } else {
                        cell.GetComponentInChildren<Highlight>().enabled = false;
                    }
                    cell.transform.parent = this.gameObject.transform;
                    grid.Add(position, cell);
                }
                setPreviousCoords(cell);
                
                // Move top left
                for (int i = 1; i < l + 1; i++) {
                    cell = Instantiate(hexTile, new Vector3(previousX - i * (float) Math.Sqrt(3) / 2, -0.5f, previousZ + i * 1.5f), rotation);
                    position = new Vector2(-i, -l + i);
                    if (shouldBePlaceable) {
                        cell.AddComponent<HexCell>().SetPosition(position);
                    } else {
                        cell.GetComponentInChildren<Highlight>().enabled = false;
                    }
                    cell.transform.parent = this.gameObject.transform;
                    grid.Add(position, cell);
                }
                setPreviousCoords(cell);
                
                // Move top right, avoiding overlapping with l - 1
                for (int i = 1; i < l; i++) {
                    cell = Instantiate(hexTile, new Vector3(previousX + i * (float) Math.Sqrt(3) / 2, -0.5f, previousZ + i * 1.5f), rotation);
                    position = new Vector2(-l, i);
                    if (shouldBePlaceable) {
                        cell.AddComponent<HexCell>().SetPosition(position);
                    } else {
                        cell.GetComponentInChildren<Highlight>().enabled = false;
                    }
                    cell.transform.parent = this.gameObject.transform;
                    grid.Add(position, cell);
                }
            } 
            
        }
        
        private void setPreviousCoords(GameObject go) {
            previousX = go.transform.position.x;
            previousZ = go.transform.position.z;
        }

        public void StateDidUpdate(MapState state) {
            Dictionary<Vector2, MapObject> newObjects = state.GetObjects();
            Dictionary<Vector2, MapObject>.KeyCollection newKeys = newObjects.Keys;
            Dictionary<Vector2, GameObject>.KeyCollection oldKeys = objectsPlaced.Keys;
	
            List<Vector2> inNewNotInOld = new List<Vector2>();
            List<Vector2> inOldNotInNew = new List<Vector2>();
           
            foreach (Vector2 oldKey in oldKeys) {
                if (!newKeys.Contains(oldKey)) {
                    inOldNotInNew.Add(oldKey);
                }
            }
	
            foreach (Vector2 newKey in newKeys) {
                if (!oldKeys.Contains(newKey)) {
                    inNewNotInOld.Add(newKey);
                }
            }
	
            // Draw things in new but not in old
            foreach (Vector2 newObjectPosition in inNewNotInOld) {
                MapObject mapObject = newObjects[newObjectPosition];
                GameObject original = AssetManager.Instance().GetModel(mapObject.GetID());

                GameObject parent = grid[newObjectPosition];
                Vector3 pos = parent.transform.position;
                pos.y += 0.5f;
                
                GameObject obj = Instantiate(original, pos, Quaternion.Euler(0, mapObject.GetRotation(), 0));
                objectsPlaced.Add(newObjectPosition, obj);
                obj.transform.parent = parent.transform;
            }
	
            // Remove things in old but not in new
            foreach (Vector3 oldObject in inOldNotInNew) {
                Destroy(objectsPlaced[oldObject]);
                objectsPlaced.Remove(oldObject);
            }
            
            // Draw wires
            wires.ForEach(Destroy);
            wires.Clear();
            foreach (WirePath path in state.GetWirePaths()) {
                // Convert grid coordinate to real world coordinates
                Vector3 startWorld = grid[path.start].transform.position;
                Vector3 endWorld = grid[path.end].transform.position;
                
                GameObject lineObject = new GameObject("Line");
                LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();
                lineRenderer.widthMultiplier = 0.1f;
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, new Vector3(startWorld.x, 0.1f, startWorld.z));
                lineRenderer.SetPosition(1, new Vector3(endWorld.x, 0.1f, endWorld.z));
                lineRenderer.material = wireMaterial;
                wires.Add(lineObject);
            }
            
            updateMachineLights();
        }

        public void StateDidUpdate(MachineState state) {
            updateMachineLights();
        }

        private void updateMachineLights() {
            foreach (KeyValuePair<Vector2, Machine> kvp in GameManager.Instance().machineStore.GetState().grid) {
                if (!objectsPlaced.ContainsKey(kvp.Key)) continue;
                
                Light[] lights = objectsPlaced[kvp.Key].GetComponentsInChildren<Light>();
                foreach (Light light in lights) {
                    AudioSource audioSource = objectsPlaced[kvp.Key].GetComponent<AudioSource>();
                    if (kvp.Value.HasFuel()) {
                        light.intensity = 20;
                        if (!audioSource.isPlaying)
                            audioSource.Play();
                    }
                    else {
                        light.intensity = 0;
                        if (audioSource.isPlaying)
                            audioSource.Stop();
                    }
                }

                ParticleSystem[] particleSystems = objectsPlaced[kvp.Key].GetComponentsInChildren<ParticleSystem>();
                foreach (ParticleSystem system in particleSystems) {
                    if (kvp.Value.HasFuel()) {
                        system.Play();
                    } else {
                        system.Pause();
                        system.Clear();
                    }
                }
            }
        }
    }
}
