using UnityEngine;
using System;
using System.Collections.Generic;
using System.Numerics;
using Model;
using Model.Redux;
using Model.State;
using UnityEngine.Analytics;
using UnityEngine.Experimental.UIElements.StyleEnums;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

/* Attached to MapGenerator and spawns map onto scene */
namespace Controller {
    public class HexMapController : MonoBehaviour, Subscriber<GameState> {
        private float tileYOffset = 1.35f;
        private float tileXOffset = 2.0f;
        private float previousX = 0;
        private float previousZ = 0;
        
        Dictionary<Vector2, GameObject> gridMap;
        
        private void Start() {
            this.gridMap = new Dictionary<Vector2, GameObject>();
            drawMap(16);
            
            GameManager.Instance().store.Subscribe(this);


            // Add items from state to grid
        }
        private void drawMap(int layers) {
            GameObject hexTile = Resources.Load("hex_cell") as GameObject;
            Quaternion rotation = Quaternion.Euler(0, 90, 0);

            // Draw origin hexagon
            GameObject cell = Instantiate(hexTile, Vector3.zero, rotation);
            Vector2 position = new Vector2(0, 0);
            cell.AddComponent<HexCell>().setPosition(position);
            gridMap.Add(position, cell);
           
            for (int l = 1; l < layers; l++) {
                // Move to correct layer, top left of origin
                cell = Instantiate(hexTile, new Vector3(l * - (float) Math.Sqrt(3) / 2, 0f, l * 1.5f), rotation);
                position = new Vector2(-l, l);
                cell.AddComponent<HexCell>().setPosition(position);
                gridMap.Add(position, cell);
                
                setPreviousCoords(cell);

                // Move right
                for (int i = 1; i < l + 1; i++) {
                    cell = Instantiate(hexTile, new Vector3(previousX + i * (float) Math.Sqrt(3), 0f, previousZ), rotation);
                    position = new Vector2(-l + i, l);
                    cell.AddComponent<HexCell>().setPosition(position);
                    gridMap.Add(position, cell);
                }
                setPreviousCoords(cell);

                // Move bottom right
                for (int i = 1; i < l + 1; i++) {
                    cell = Instantiate(hexTile, new Vector3(previousX + i * (float) Math.Sqrt(3) / 2, 0f, previousZ - i * 1.5f), rotation);
                    position = new Vector2(i, l-i);
                    cell.AddComponent<HexCell>().setPosition(position);
                    gridMap.Add(position, cell);
                }
                setPreviousCoords(cell);

                // Move bottom left
                for (int i = 1; i < l + 1; i++) {
                    cell = Instantiate(hexTile, new Vector3(previousX - i * (float) Math.Sqrt(3) / 2, 0f, previousZ - i * 1.5f), rotation);
                    position = new Vector2(l, -i);
                    cell.AddComponent<HexCell>().setPosition(position);
                    gridMap.Add(position, cell);
                }
                setPreviousCoords(cell);

                // Move left
                for (int i = 1; i < l + 1; i++) {
                    cell = Instantiate(hexTile, new Vector3(previousX - i * (float) Math.Sqrt(3), 0f, previousZ), rotation);
                    position = new Vector2(l-i, -l);
                    cell.AddComponent<HexCell>().setPosition(position);
                    gridMap.Add(position, cell);
                }
                setPreviousCoords(cell);
                
                // Move top left
                for (int i = 1; i < l + 1; i++) {
                    cell = Instantiate(hexTile, new Vector3(previousX - i * (float) Math.Sqrt(3) / 2, 0f, previousZ + i * 1.5f), rotation);
                    position = new Vector2(-i, -l + i);
                    cell.AddComponent<HexCell>().setPosition(position);
                    gridMap.Add(position, cell);
                }
                setPreviousCoords(cell);
                
                // Move top right, avoiding overlapping with l - 1
                for (int i = 1; i < l; i++) {
                    cell = Instantiate(hexTile, new Vector3(previousX + i * (float) Math.Sqrt(3) / 2, 0f, previousZ + i * 1.5f), rotation);
                    position = new Vector2(-l, i);
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
            Dictionary<Vector2, bool> newObjects = state.mapState.getObjects(); 
            
            foreach (KeyValuePair<Vector2, GameObject> element in gridMap) {
                if (newObjects.ContainsKey(element.Key)) {
                    element.Value.GetComponentInChildren<Renderer>().material.color = Color.red;  
                } else {
                    element.Value.GetComponentInChildren<Renderer>().material.color = Color.cyan;  
                }
            }
        }
    }
}
