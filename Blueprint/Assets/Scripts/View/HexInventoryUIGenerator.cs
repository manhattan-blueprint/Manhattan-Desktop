using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Controller;
using UnityEngine.UI;

public class HexInventoryUIGenerator : MonoBehaviour {
    public Sprite HexTile;
    
    private int numLayers = 2;
    private float tileYOffset = 1.35f;
    private float previousX = 0;
    private float previousY = 0;
    
    void Start() {
        int hexCount = 0;
        GameObject go = newSlot(ref hexCount);
        setPreviousCoords(go);
        GameObject temp = null;
                   
        // For each layer
        for (int l=1; l<numLayers+1; l++) {
            // Initialise top hexagon
            temp = newSlot(ref hexCount);
            for (int i = 0; i < l; i++) {
                translateTopLeft(temp);
            }
            setPreviousCoords(temp); 

            for (int i = 0; i < l; i++) {
                temp = newSlot(ref hexCount, previousX, previousY);
                translateRight(temp);
                setPreviousCoords(temp);
            } 
            
            for (int i = 0; i < l; i++) {
                temp = newSlot(ref hexCount, previousX, previousY);
                translateBottomRight(temp);
                setPreviousCoords(temp);
            } 
            
            for (int i = 0; i < l; i++) {
                temp = newSlot(ref hexCount, previousX, previousY);
                translateBottomLeft(temp);
                setPreviousCoords(temp);
            } 
            
            for (int i = 0; i < l; i++) {
                temp = newSlot(ref hexCount, previousX, previousY);
                translateLeft(temp);
                setPreviousCoords(temp);
            } 
            
            for (int i = 0; i < l; i++) {
                temp = newSlot(ref hexCount, previousX, previousY);
                translateTopLeft(temp);
                setPreviousCoords(temp);
            } 
           
            // Iterates to l-1 to avoid duplicate hexagons at the top
            for (int i = 0; i < l-1; i++) {
                temp = newSlot(ref hexCount, previousX, previousY);
                translateTopRight(temp);
                setPreviousCoords(temp);
            } 
        }
    }

    // Create new slot, place in centre 
    private GameObject newSlot(ref int id) {
        GameObject go = new GameObject("Slot" + id);
        go.transform.SetParent(this.transform);
        go.transform.position = new Vector2(Screen.width/2, Screen.height/2);
        
        Image image = go.AddComponent<Image>();
        image.sprite = HexTile;

        InventorySlotController isc = go.AddComponent<InventorySlotController>();
        isc.setId(id);

        id++;
        return go;
    }
    
    // Create new slot, place at specified coordinates
    private GameObject newSlot(ref int id, float x, float y) {
        GameObject go = new GameObject("Slot" + id);
        go.transform.SetParent(this.transform);
        go.transform.position = new Vector2(x, y);
        
        Image image = go.AddComponent<Image>();
        image.sprite = HexTile;
        
        InventorySlotController isc = go.AddComponent<InventorySlotController>();
        isc.setId(id);

        id++;
        return go;
    }

    private void setPreviousCoords(GameObject go) {
        previousX = go.transform.position.x;
        previousY = go.transform.position.y;
    } 

    private void translateTop(GameObject go) {
        go.transform.position = new Vector2(getCurrentX(go), getCurrentY(go)+getHeight(go));
    }
    
    private void translateBottomRight(GameObject go) {
        go.transform.position = new Vector2(getCurrentX(go)+getWidth(go)/2, getCurrentY(go)-getHeight(go)/tileYOffset);
    }
    
    private void translateBottom(GameObject go) {
        go.transform.position = new Vector2(getCurrentX(go), getCurrentY(go)-getHeight(go));
    }
    
    private void translateBottomLeft(GameObject go) {
        go.transform.position = new Vector2(getCurrentX(go)-getWidth(go)/2, getCurrentY(go)-getHeight(go)/tileYOffset);
    }
    
    private void translateTopLeft(GameObject go) {
        go.transform.position = new Vector2(getCurrentX(go)-getWidth(go)/2, getCurrentY(go)+getHeight(go)/tileYOffset);
    }
    
    private void translateTopRight(GameObject go) {
        go.transform.position = new Vector2(getCurrentX(go)+getWidth(go)/2, getCurrentY(go)+getHeight(go)/tileYOffset);
    }
    
    private void translateRight(GameObject go) {
        go.transform.position = new Vector2(getCurrentX(go)+getWidth(go), getCurrentY(go));
    }

    private void translateLeft(GameObject go) {
        go.transform.position = new Vector2(getCurrentX(go)-getWidth(go), getCurrentY(go));
    }

    private float getWidth(GameObject go) {
        return go.GetComponent<RectTransform>().rect.width;
    }
    
    private float getHeight(GameObject go) {
        return go.GetComponent<RectTransform>().rect.height;
    }

    private float getCurrentX(GameObject go) {
        return go.transform.position.x;
    }

    private float getCurrentY(GameObject go) {
        return go.transform.position.y;
    }
}
