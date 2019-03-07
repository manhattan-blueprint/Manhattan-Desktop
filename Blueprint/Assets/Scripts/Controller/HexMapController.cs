using UnityEngine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Model;
using Model.Redux;
using Model.State;
using UnityEngine.Analytics;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

/* Attached to MapGenerator and spawns map onto scene */
namespace Controller {
    public class HexMapController : MonoBehaviour, Subscriber<GameState> {
        private float tileYOffset = 1.35f;
        private float tileXOffset = 2.0f;
        private float previousX = 0;
        private float previousZ = 0;
        
        Dictionary<Vector3, GameObject> gridMap;
        
        private void Start() {
            this.gridMap = new Dictionary<Vector3, GameObject>();
            drawMap(16);
            
            GameManager.Instance().store.Subscribe(this);
        }
        private void drawMap(int layers) {
            GameObject hexTile = Resources.Load("hex_cell") as GameObject;
            Quaternion rotation = Quaternion.Euler(0, 90, 0);

            // Draw origin hexagon
            GameObject cell = Instantiate(hexTile, Vector3.zero, rotation);
            Vector3 position = new Vector3(0, 0, 0);
            cell.AddComponent<HexCell>().setPosition(position);
            gridMap.Add(position, cell);
           
            for (int l = 1; l < layers; l++) {
                // Move to correct layer, top left of origin
                cell = Instantiate(hexTile, new Vector3(l * - (float) Math.Sqrt(3) / 2, 0f, l * 1.5f), rotation);
                position = new Vector3(-l, 0, l);
                cell.AddComponent<HexCell>().setPosition(position);
                gridMap.Add(position, cell);
                
                setPreviousCoords(cell);

                // Move right
                for (int i = 1; i < l + 1; i++) {
                    cell = Instantiate(hexTile, new Vector3(previousX + i * (float) Math.Sqrt(3), 0f, previousZ), rotation);
                    position = new Vector3(-l + i, 0, l);
                    cell.AddComponent<HexCell>().setPosition(position);
                    gridMap.Add(position, cell);
                }
                setPreviousCoords(cell);

                // Move bottom right
                for (int i = 1; i < l + 1; i++) {
                    cell = Instantiate(hexTile, new Vector3(previousX + i * (float) Math.Sqrt(3) / 2, 0f, previousZ - i * 1.5f), rotation);
                    position = new Vector3(i, 0, l-i);
                    cell.AddComponent<HexCell>().setPosition(position);
                    gridMap.Add(position, cell);
                }
                setPreviousCoords(cell);

                // Move bottom left
                for (int i = 1; i < l + 1; i++) {
                    cell = Instantiate(hexTile, new Vector3(previousX - i * (float) Math.Sqrt(3) / 2, 0f, previousZ - i * 1.5f), rotation);
                    position = new Vector3(l, 0, -i);
                    cell.AddComponent<HexCell>().setPosition(position);
                    gridMap.Add(position, cell);
                }
                setPreviousCoords(cell);

                // Move left
                for (int i = 1; i < l + 1; i++) {
                    cell = Instantiate(hexTile, new Vector3(previousX - i * (float) Math.Sqrt(3), 0f, previousZ), rotation);
                    position = new Vector3(l-i, 0, -l);
                    cell.AddComponent<HexCell>().setPosition(position);
                    gridMap.Add(position, cell);
                }
                setPreviousCoords(cell);
                
                // Move top left
                for (int i = 1; i < l + 1; i++) {
                    cell = Instantiate(hexTile, new Vector3(previousX - i * (float) Math.Sqrt(3) / 2, 0f, previousZ + i * 1.5f), rotation);
                    position = new Vector3(-i, 0, -l + i);
                    cell.AddComponent<HexCell>().setPosition(position);
                    gridMap.Add(position, cell);
                }
                setPreviousCoords(cell);
                
                // Move top right, avoiding overlapping with l - 1
                for (int i = 1; i < l; i++) {
                    cell = Instantiate(hexTile, new Vector3(previousX + i * (float) Math.Sqrt(3) / 2, 0f, previousZ + i * 1.5f), rotation);
                    position = new Vector3(-l, 0, i);
                    cell.AddComponent<HexCell>().setPosition(position);
                    gridMap.Add(position, cell);
                }
            } 
        }
        
        private void setPreviousCoords(GameObject go) {
            previousX = go.transform.position.x;
            previousZ = go.transform.position.z;
        }

        public void StateDidUpdate(GameState state) {
            Dictionary<Vector3, MapObject> newObjects = state.mapState.getObjects();

            Dictionary<Vector3, MapObject>.KeyCollection newKeys = newObjects.Keys;
            Dictionary<Vector3, GameObject>.KeyCollection oldKeys = gridMap.Keys;

            List<Vector3> inNewNotInOld = new List<Vector3>();
            List<Vector3> inOldNotInNew = new List<Vector3>();
            
            foreach (Vector3 oldKey in oldKeys) {
                if (!newKeys.Contains(oldKey)) {
                    inOldNotInNew.Add(oldKey);
                }
            }

            foreach (Vector3 newKey in newKeys) {
                if (!oldKeys.Contains(newKey)) {
                    inNewNotInOld.Add(newKey);
                }
            }
            
            // Draw things in new but not in old
            foreach (Vector3 newObject in inNewNotInOld) {
                MapObject mapObject = newObjects[newObject];
                GameObject original = ModelManager.Instance().GetModel(mapObject.GetID());

                GameObject parent = gridMap[new Vector3(newObject.x, 0, newObject.z)];
                
                Instantiate(original, parent, Quaternion.identity); 
                
                gridMap.Add(newObject, ); 
            }
            
            // Remove things in old but not in new
           foreach (Vector3 oldObject in inOldNotInNew) {
                Destroy(gridMap[oldObject]);
                gridMap.Remove(oldObject);
           }


//            foreach (KeyValuePair<Vector3, GameObject> existing in gridMap) {
//                
//                
//                
//                if (newObjects.ContainsKey(existing.Key)) {
//                    MapObject mapObject = newObjects[existing.Key];
//                    Vector3 position = existing.Value.transform.position;
//                    // Add half height of hex tile
//                    position.y += 0.5f;
//                    GameObject original = ModelManager.Instance().GetModel(mapObject.GetID());
//                    GameObject tree = Instantiate(original, position, Quaternion.identity);
//                    tree.transform.parent = existing.Value.transform; 
//                } else {
//                   
//                }
//            }
        }
    }
}
