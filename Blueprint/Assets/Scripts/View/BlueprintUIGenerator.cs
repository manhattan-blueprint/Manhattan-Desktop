using System;
using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;
using View;

public class BlueprintUIGenerator : MonoBehaviour {

    private GameObject scrollContainer;
    private GameObject contentContainer;
    private float cellScale = 8;
    private float primaryCellHeight;
    private float primaryCellWidth;
    private float scrollSensitivity = 24f;
    
    void Start() {
        
        primaryCellHeight = Screen.height / cellScale;
        primaryCellWidth = primaryCellHeight * (float) Math.Sqrt(3) / 2;
        
        // Setup scroll container
        scrollContainer = new GameObject("ScrollContainer");
        scrollContainer.transform.SetParent(this.transform);
        scrollContainer.AddComponent(typeof(RectTransform));
        RectTransform scrollRT = scrollContainer.GetComponent<RectTransform>();
        scrollRT.anchorMin = Vector2.zero;
        scrollRT.anchorMax = Vector2.one;
        scrollRT.localPosition = new Vector2(0, 0);
        scrollContainer.AddComponent(typeof(Image));
        scrollContainer.AddComponent(typeof(ScrollRect));
        ScrollRect scrollSR = scrollContainer.GetComponent<ScrollRect>();
        scrollSR.movementType = ScrollRect.MovementType.Clamped;
        scrollSR.vertical = false;
        scrollSR.scrollSensitivity = scrollSensitivity;
        scrollContainer.AddComponent(typeof(Mask));
        scrollContainer.GetComponent<Mask>().showMaskGraphic = true;
        
        // Setup content container
        contentContainer = new GameObject("ContentContainer");
        contentContainer.transform.SetParent(scrollContainer.transform);
        contentContainer.AddComponent(typeof(RectTransform));
        RectTransform contentRT = contentContainer.GetComponent<RectTransform>();
        contentRT.anchorMin = Vector2.zero;
        contentRT.anchorMax = new Vector2(2, 1);
        contentRT.pivot = new Vector2(0, 0.5f);
        contentRT.localPosition = new Vector2(0,0);
        contentContainer.AddComponent(typeof(SVGImage));
        contentContainer.GetComponent<SVGImage>().sprite = AssetManager.Instance().blueprintUIBackground;

        // Add primary resources
        addPrimaryCell("wood", 1, (float) primaryCellWidth * 2, primaryCellHeight * 3);
        addPrimaryCell("stone", 2, (float) (primaryCellWidth * 3.5), primaryCellHeight * 3);
        addPrimaryCell("clay", 3, (float) primaryCellWidth * 2, (float) (primaryCellHeight * 1.5));
        addPrimaryCell("ironOre", 4, (float) (primaryCellWidth * 3.5), (float) (primaryCellHeight * 1.5));
        addPrimaryCell("copperOre", 5, (float) primaryCellWidth * 2, 0);
        addPrimaryCell("rubber", 6, (float) (primaryCellWidth * 3.5), 0);
        addPrimaryCell("diamond", 7, (float) primaryCellWidth * 2, (float) (primaryCellHeight * -1.5));
        addPrimaryCell("sand", 8, (float) (primaryCellWidth * 3.5), (float) (primaryCellHeight * -1.5));
        addPrimaryCell("silicaOre", 9, (float) primaryCellWidth * 2, primaryCellHeight * -3);
        addPrimaryCell("quartz", 10, (float) (primaryCellWidth * 3.5), primaryCellHeight * -3);
        
        addBlueprintCell("furnace", primaryCellWidth * 7, 0);
        addBlueprintCell("machineBase", primaryCellWidth * 11, (float) (primaryCellHeight * 2.5));
        addBlueprintCell("fibreglass", primaryCellWidth * 11, (float) (primaryCellHeight * -2.5));
        addBlueprintCell("wireDrawer", primaryCellWidth * 15, (float) (primaryCellHeight * 2.5));
        addBlueprintCell("solarPanel", primaryCellWidth * 15, (float) (primaryCellHeight * -2.5));
        addBlueprintCell("copperCoil", primaryCellWidth * 19, (float) (primaryCellHeight * 2.5));
        addBlueprintCell("insulatedWire", primaryCellWidth * 19, (float) (primaryCellHeight * -2.5));
        addBlueprintCell("welder", primaryCellWidth * 23, (float) (primaryCellHeight * 2.5));
        addBlueprintCell("motor", primaryCellWidth * 23, 0);
        addBlueprintCell("circuitPrinter", primaryCellWidth * 23, (float) (primaryCellHeight * -2.5));
        addBlueprintCell("satelliteDish", primaryCellWidth * 27, (float) (primaryCellHeight * 2.5));
        addBlueprintCell("transmitterReceiver", primaryCellWidth * 27, (float) (primaryCellHeight * -2.5));

        // Attach content to scroll rect
        scrollSR.content = contentContainer.GetComponent<RectTransform>();
    }

    private void addPrimaryCell(string name, int id, float xPos, float yPos) {
        
        GameObject cell = new GameObject(name + "Cell");
        cell.transform.SetParent(contentContainer.transform);
        cell.AddComponent(typeof(RectTransform));
        RectTransform cellRT = cell.GetComponent<RectTransform>();
        cellRT.pivot = new Vector2(0.5f, 0.5f);
        cellRT.sizeDelta = new Vector2(primaryCellWidth, primaryCellHeight);
        cellRT.localPosition = new Vector2(xPos, yPos);
        cell.AddComponent(typeof(SVGImage));
        cell.GetComponent<SVGImage>().sprite = AssetManager.Instance().blueprintUICellPrimary;
        
        // Add item sprite
        GameObject sprite = new GameObject(name + "Sprite");
        sprite.transform.SetParent(cell.transform);
        sprite.AddComponent(typeof(RectTransform));
        RectTransform spriteRT = sprite.GetComponent<RectTransform>();
        spriteRT.pivot = new Vector2(0.5f, 0.5f);
        spriteRT.sizeDelta = new Vector2((float) (primaryCellWidth * 0.64), (float) (primaryCellHeight * 0.64));
        spriteRT.localPosition = new Vector2(0, 0);
        sprite.AddComponent(typeof(Image));
        sprite.GetComponent<Image>().sprite = AssetManager.Instance().GetItemSprite(id);
    }

    private void addBlueprintCell(string name, float xPos, float yPos) {
        GameObject cell = new GameObject(name + "Cell");
        cell.transform.SetParent(contentContainer.transform);
        cell.AddComponent(typeof(RectTransform));
        RectTransform cellRT = cell.GetComponent<RectTransform>();
        cellRT.pivot = new Vector2(0.5f, 0.5f);
        cellRT.sizeDelta = new Vector2(primaryCellWidth * 2, primaryCellHeight * 2);
        cellRT.localPosition = new Vector2(xPos, yPos);
        cell.AddComponent(typeof(SVGImage));
        cell.GetComponent<SVGImage>().sprite = AssetManager.Instance().blueprintUICellDark;
    }
    
}
