using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Action;
using Model.Redux;
using Model.State;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

/* Attached to MapGenerator and spawns map onto scene */
namespace Controller {
    public class HexMapController : MonoBehaviour, Subscriber<MapState> {
        private int gridSize = 18;
        private float previousX = 0;
        private float previousZ = 0;
        
        private Dictionary<Vector2, GameObject> grid;
        private Dictionary<Vector2, GameObject> objectsPlaced;
        
        private void Start() {
            this.grid = new Dictionary<Vector2, GameObject>();
            this.objectsPlaced = new Dictionary<Vector2, GameObject>();
            drawMap();
            
            GameManager.Instance().mapStore.Subscribe(this);
        }
        private void drawMap() {
            GameObject hexTile = Resources.Load("hex_cell") as GameObject;
            Quaternion rotation = Quaternion.Euler(0, 90, 0);

            // Draw origin hexagon
            GameObject cell = Instantiate(hexTile, new Vector3(0, -0.5f, 0), rotation);
            cell.transform.parent = this.gameObject.transform;
            Vector2 position = new Vector2(0, 0);
            cell.AddComponent<HexCell>().setPosition(position);
            
            grid.Add(position, cell);
          
            
            for (int l = 1; l < gridSize; l++) {
                // Move to correct layer, top left of origin
                
                cell = Instantiate(hexTile, new Vector3(l * - (float) Math.Sqrt(3) / 2, -0.5f, l * 1.5f), rotation);
                position = new Vector2(-l, l);
                cell.transform.parent = this.gameObject.transform;
                cell.AddComponent<HexCell>().setPosition(position);
                grid.Add(position, cell);
                
                setPreviousCoords(cell);

                // Move right
                for (int i = 1; i < l + 1; i++) {
                    cell = Instantiate(hexTile, new Vector3(previousX + i * (float) Math.Sqrt(3), -0.5f, previousZ), rotation);
                    position = new Vector2(-l + i, l);
                    cell.AddComponent<HexCell>().setPosition(position);
                    cell.transform.parent = this.gameObject.transform;
                    grid.Add(position, cell);
                }
                setPreviousCoords(cell);

                // Move bottom right
                for (int i = 1; i < l + 1; i++) {
                    cell = Instantiate(hexTile, new Vector3(previousX + i * (float) Math.Sqrt(3) / 2, -0.5f, previousZ - i * 1.5f), rotation);
                    position = new Vector2(i, l-i);
                    cell.AddComponent<HexCell>().setPosition(position);
                    cell.transform.parent = this.gameObject.transform;
                    grid.Add(position, cell);
                }
                setPreviousCoords(cell);

                // Move bottom left
                for (int i = 1; i < l + 1; i++) {
                    cell = Instantiate(hexTile, new Vector3(previousX - i * (float) Math.Sqrt(3) / 2, -0.5f, previousZ - i * 1.5f), rotation);
                    position = new Vector2(l, -i);
                    cell.AddComponent<HexCell>().setPosition(position);
                    cell.transform.parent = this.gameObject.transform;
                    grid.Add(position, cell);
                }
                setPreviousCoords(cell);

                // Move left
                for (int i = 1; i < l + 1; i++) {
                    cell = Instantiate(hexTile, new Vector3(previousX - i * (float) Math.Sqrt(3), -0.5f, previousZ), rotation);
                    position = new Vector2(l-i, -l);
                    cell.AddComponent<HexCell>().setPosition(position);
                    cell.transform.parent = this.gameObject.transform;
                    grid.Add(position, cell);
                }
                setPreviousCoords(cell);
                
                // Move top left
                for (int i = 1; i < l + 1; i++) {
                    cell = Instantiate(hexTile, new Vector3(previousX - i * (float) Math.Sqrt(3) / 2, -0.5f, previousZ + i * 1.5f), rotation);
                    position = new Vector2(-i, -l + i);
                    cell.AddComponent<HexCell>().setPosition(position);
                    cell.transform.parent = this.gameObject.transform;
                    grid.Add(position, cell);
                }
                setPreviousCoords(cell);
                
                // Move top right, avoiding overlapping with l - 1
                for (int i = 1; i < l; i++) {
                    cell = Instantiate(hexTile, new Vector3(previousX + i * (float) Math.Sqrt(3) / 2, -0.5f, previousZ + i * 1.5f), rotation);
                    position = new Vector2(-l, i);
                    cell.AddComponent<HexCell>().setPosition(position);
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
            Dictionary<Vector2, MapObject> newObjects = state.getObjects();
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
                
                GameObject obj = Instantiate(original, pos, Quaternion.Euler(0, 90, 0)); 
                objectsPlaced.Add(newObjectPosition, obj);
                obj.transform.parent = parent.transform;

            }
	
            // Remove things in old but not in new
            foreach (Vector3 oldObject in inOldNotInNew) {
                Destroy(objectsPlaced[oldObject]);
                objectsPlaced.Remove(oldObject);
            }
        }
    }
}
