using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Controller;
using Model;
using UnityEngine.UI;

public class HexInventoryUIGenerator : MonoBehaviour {
    private Sprite HexTile;
    private Sprite borderSprite;
    private Sprite highlightSprite;
    private Sprite outerBorderSprite;
    private float tileYOffset = 1.35f;
    private float tileXOffset = 2.0f;
    private float previousX = 0;
    private float previousY = 0;
    private float slotDimension = 0;

    // EDITABLE
    private int numLayers = 2;
    private float slotScale = 10;

    void Start() {
        HexTile = Resources.Load("inventory_slot", typeof(Sprite)) as Sprite;
        borderSprite = Resources.Load("slot_border", typeof(Sprite)) as Sprite;
        highlightSprite = Resources.Load("slot_border_highlight", typeof(Sprite)) as Sprite;
        outerBorderSprite = Resources.Load("slot_border_outer", typeof(Sprite)) as Sprite;

        slotDimension = Screen.width / slotScale;

        int hexCount = 0;
        GameObject go = newSlot(ref hexCount, false);
        setPreviousCoords(go);
        GameObject temp = null;

        // For each layer
        for (int l=1; l<numLayers+1; l++) {
            bool outerBoarder;
            if (l == numLayers) outerBoarder = true;
            else outerBoarder = false;

            // Initialise top hexagon
            temp = newSlot(ref hexCount, outerBoarder);
            for (int i = 0; i < l; i++) {
                translateTopLeft(temp);
            }
            setPreviousCoords(temp);

            for (int i = 0; i < l; i++) {
                temp = newSlot(ref hexCount, previousX, previousY, outerBoarder);
                translateRight(temp);
                setPreviousCoords(temp);
            }

            for (int i = 0; i < l; i++) {
                temp = newSlot(ref hexCount, previousX, previousY, outerBoarder);
                translateBottomRight(temp);
                setPreviousCoords(temp);
            }

            for (int i = 0; i < l; i++) {
                temp = newSlot(ref hexCount, previousX, previousY, outerBoarder);
                translateBottomLeft(temp);
                setPreviousCoords(temp);
            }

            for (int i = 0; i < l; i++) {
                temp = newSlot(ref hexCount, previousX, previousY, outerBoarder);
                translateLeft(temp);
                setPreviousCoords(temp);
            }

            for (int i = 0; i < l; i++) {
                temp = newSlot(ref hexCount, previousX, previousY, outerBoarder);
                translateTopLeft(temp);
                setPreviousCoords(temp);
            }

            // Iterates to l-1 to avoid duplicate hexagons at the top
            for (int i = 0; i < l-1; i++) {
                temp = newSlot(ref hexCount, previousX, previousY, outerBoarder);
                translateTopRight(temp);
                setPreviousCoords(temp);
            }
        }

        // Highlight object
        GameObject highlight = new GameObject("Highlight");
        highlight.transform.parent = transform;
        SVGImage svg = highlight.AddComponent<SVGImage>();
        svg.sprite = highlightSprite;
        svg.raycastTarget = false;
        (svg.transform as RectTransform).sizeDelta = new Vector2(slotDimension, slotDimension);
        highlight.transform.position = new Vector3(-Screen.width, -Screen.height, 0);

        // Temp drag object
        GameObject drag = new GameObject("drag");
        drag.transform.parent = transform;
        Image image = drag.AddComponent<Image>();
        image.raycastTarget = false;
        // To initialise drag object off screen
        drag.transform.position = new Vector3(-Screen.width, -Screen.height, 0);

        // Rollover text object
        GameObject rollover = new GameObject("Rollover");
        rollover.transform.parent = transform;
        SVGImage backgroundImage = rollover.AddComponent<SVGImage>();
        backgroundImage.sprite = Resources.Load("rolloverBox", typeof(Sprite)) as Sprite;
        (rollover.transform as RectTransform).sizeDelta = new Vector2(slotDimension/2, slotDimension/6);

        GameObject text = new GameObject("Text");
        text.transform.parent = rollover.transform;
        Text rolloverText = text.AddComponent<Text>();
        rolloverText.raycastTarget = false;
        rolloverText.fontSize = AssetManager.Instance().QuantityFieldFontSize;
        rolloverText.horizontalOverflow = HorizontalWrapMode.Overflow;

        rollover.transform.position = new Vector3(-Screen.width, -Screen.height, 0);
    }

    // Create new slot, place in centre
    private GameObject newSlot(ref int id, bool outerBorder) {
        return newSlot(ref id, Screen.width / 2, Screen.height / 2, outerBorder);
    }

    // Create new slot, place at specified coordinates
    private GameObject newSlot(ref int id, float x, float y, bool outerBorder) {
        GameObject go = new GameObject("Slot" + id);
        go.transform.SetParent(this.transform);
        go.transform.position = new Vector2(x, y);

        // Background Image
        Image background = go.AddComponent<Image>();
        background.sprite = HexTile;
        background.color = new Color32((byte)(background.color.r*255), (byte)(background.color.g*255),
            (byte)(background.color.b*255), (byte)192);
        background.alphaHitTestMinimumThreshold = 0.5f;

        // Border
        GameObject svgChild = new GameObject("Border" + id);
        svgChild.transform.SetParent(go.transform);
        svgChild.transform.position = new Vector2(x, y);
        SVGImage border = svgChild.AddComponent<SVGImage>();
        border.sprite = getBorder(outerBorder);
        (svgChild.transform as RectTransform).sizeDelta = new Vector2(slotDimension, slotDimension);
        (svgChild.transform as RectTransform).localScale = new Vector3(1.05f, 1.05f, 0.0f);

        InventorySlotController isc = go.AddComponent<InventorySlotController>();
        isc.setId(id);

        RectTransform rt = go.transform as RectTransform;
        rt.sizeDelta = new Vector2(slotDimension, slotDimension);

        id++;
        GameManager.Instance().store.GetState().inventoryState.inventorySize += 1;
        return go;
    }

    private Sprite getBorder(bool outer) {
        if (outer) {
            return outerBorderSprite;
        } else {
            return borderSprite;
        }
    }

    private void setPreviousCoords(GameObject go) {
        previousX = go.transform.position.x;
        previousY = go.transform.position.y;
    }

    private void translateTop(GameObject go) {
        go.transform.position = new Vector2(getCurrentX(go), getCurrentY(go)+getHeight(go));
    }

    private void translateBottomRight(GameObject go) {
        go.transform.position = new Vector2(getCurrentX(go)+getWidth(go)/tileXOffset, getCurrentY(go)-getHeight(go)/tileYOffset);
    }

    private void translateBottom(GameObject go) {
        go.transform.position = new Vector2(getCurrentX(go), getCurrentY(go)-getHeight(go));
    }

    private void translateBottomLeft(GameObject go) {
        go.transform.position = new Vector2(getCurrentX(go)-getWidth(go)/tileXOffset, getCurrentY(go)-getHeight(go)/tileYOffset);
    }

    private void translateTopLeft(GameObject go) {
        go.transform.position = new Vector2(getCurrentX(go)-getWidth(go)/tileXOffset, getCurrentY(go)+getHeight(go)/tileYOffset);
    }

    private void translateTopRight(GameObject go) {
        go.transform.position = new Vector2(getCurrentX(go)+getWidth(go)/tileXOffset, getCurrentY(go)+getHeight(go)/tileYOffset);
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
