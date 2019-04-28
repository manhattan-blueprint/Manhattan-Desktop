using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Model;
using Model.Action;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using View;

public class BlueprintUIGenerator : MonoBehaviour {

    private float scale = 8;
    private float scaleUnit;
    private float primaryCellDimension;
    private float blueprintCellDimension;
    private float lineDimension;
    private float scrollSensitivity = 24f;
    private GameObject scrollContainer;
    private GameObject contentContainer;
    
    void Start() {
        scaleUnit = Screen.height / scale;
        primaryCellDimension = scaleUnit;
        blueprintCellDimension = primaryCellDimension * 2;
        lineDimension = scaleUnit / (scale * 1.5f);
        
        // Setup scroll container
        scrollContainer = new GameObject("ScrollContainer");
        scrollContainer.transform.SetParent(this.transform);
        RectTransform scrollRT = (RectTransform) scrollContainer.AddComponent(typeof(RectTransform));    
        scrollRT.anchorMin = Vector2.zero;
        scrollRT.anchorMax = Vector2.one;
        scrollRT.localPosition = new Vector2(0, 0);
        scrollContainer.AddComponent(typeof(Image));
        ScrollRect scrollSR = (ScrollRect) scrollContainer.AddComponent(typeof(ScrollRect));
        scrollSR.movementType = ScrollRect.MovementType.Clamped;
        scrollSR.vertical = false;
        scrollSR.scrollSensitivity = scrollSensitivity;
        scrollContainer.AddComponent(typeof(Mask));
        scrollContainer.GetComponent<Mask>().showMaskGraphic = true;
        
        // Setup content container
        contentContainer = new GameObject("ContentContainer");
        contentContainer.transform.SetParent(scrollContainer.transform);
        RectTransform contentRT = (RectTransform) contentContainer.AddComponent(typeof(RectTransform));
        contentRT.anchorMin = Vector2.zero;
        contentRT.anchorMax = new Vector2(3, 1);
        contentRT.pivot = new Vector2(0, 0.5f);
        contentRT.localPosition = new Vector2(0, 0);
        contentContainer.AddComponent(typeof(SVGImage));
        contentContainer.GetComponent<SVGImage>().sprite = AssetManager.Instance().blueprintUIBackground;
        
        // Add primary resource cells
        addPrimaryCell("wood",      1,  (float) (scaleUnit * 1.5), scaleUnit * 3);
        addPrimaryCell("stone",     2,  (float) (scaleUnit * 1.5), (float) (scaleUnit * 1.5));
        addPrimaryCell("clay",      3,  (float) (scaleUnit * 1.5), 0);
        addPrimaryCell("ironOre",   4,  (float) (scaleUnit * 1.5), (float) (scaleUnit * -1.5));
        addPrimaryCell("copperOre", 5,  (float) (scaleUnit * 1.5), scaleUnit * -3);
        addPrimaryCell("rubber",    6,  scaleUnit * 3, scaleUnit * 3);
        addPrimaryCell("diamond",   7,  scaleUnit * 3, (float) (scaleUnit * 1.5));
        addPrimaryCell("sand",      8,  scaleUnit * 3, 0);
        addPrimaryCell("silicaOre", 9,  scaleUnit * 3, (float) (scaleUnit * -1.5));
        addPrimaryCell("quartz",    10, scaleUnit * 3, scaleUnit * -3);
        
        // Add blueprint cells and lines
        addBlueprintCell("furnace", 11, scaleUnit * 7,  0);
        
        addBlueprintLine("furnace-1", (float) (scaleUnit * 1.5), lineDimension, (float) (scaleUnit * 8.75), 0);
        addBlueprintLine("furnace-2", lineDimension, scaleUnit * 5 + (lineDimension), (float) (scaleUnit * 9.5), 0);
        addBlueprintLine("furnace-3", (float) (scaleUnit * 1.5), lineDimension, (float) (scaleUnit * 10.25), (float) (scaleUnit * 2.5));
        addBlueprintLine("furnace-4", (float) (scaleUnit * 1.5), lineDimension, (float) (scaleUnit * 10.25), (float) (scaleUnit * -2.5));
        
        addBlueprintCell("machineBase", 19, scaleUnit * 12, (float) (scaleUnit * 2.5));
        addBlueprintCell("fibreglass",  18, scaleUnit * 12, (float) (scaleUnit * -2.5));
        
        addBlueprintLine("machineBase-1", (float) (scaleUnit * 3), lineDimension, (float) (scaleUnit * 14.5), (float) (scaleUnit * 2.5));
        addBlueprintLine("machineBase-2", lineDimension, (float) (scaleUnit * 5) + (lineDimension), (float) (scaleUnit * 14.5), 0);
        addBlueprintLine("machineBase-3", (float) (scaleUnit * 1.5), lineDimension, (float) (scaleUnit * 15.25), (float) (scaleUnit * -2.5));
        
        addBlueprintCell("wireDrawer", 20, scaleUnit * 17, (float) (scaleUnit * 2.5));
        addBlueprintCell("solarPanel", 25, scaleUnit * 17, (float) (scaleUnit * -2.5));
        
        addBlueprintLine("wireDrawer-1", (float) (scaleUnit * 3), lineDimension, (float) (scaleUnit * 19.5), (float) (scaleUnit * 2.5));
        addBlueprintLine("wireDrawer-2", lineDimension, (float) (scaleUnit * 5) + (lineDimension), (float) (scaleUnit * 19.5), 0);
        addBlueprintLine("wireDrawer-3", (float) (scaleUnit * 1.5), lineDimension, (float) (scaleUnit * 20.25), (float) (scaleUnit * -2.5));
        
        addBlueprintCell("copperCoil",    23, scaleUnit * 22, (float) (scaleUnit * 2.5));
        addBlueprintCell("insulatedWire", 22, scaleUnit * 22, (float) (scaleUnit * -2.5));
        
        addBlueprintLine("copperCoil-1", (float) (scaleUnit * 3), lineDimension, (float) (scaleUnit * 24.5), (float) (scaleUnit * 2.5));
        addBlueprintLine("copperCoil-2", lineDimension, (float) (scaleUnit * 5) + (lineDimension), (float) (scaleUnit * 24.5), 0);
        addBlueprintLine("copperCoil-3", (float) (scaleUnit * 1.5), lineDimension, (float) (scaleUnit * 25.25), 0);
        addBlueprintLine("insulatedWire-1", (float) (scaleUnit * 3), lineDimension, (float) (scaleUnit * 24.5), (float) (scaleUnit * -2.5));
        
        addBlueprintCell("motor",          24, scaleUnit * 27, (float) (scaleUnit * 2.5));
        addBlueprintCell("welder",         26, scaleUnit * 27, 0);
        addBlueprintCell("circuitPrinter", 29, scaleUnit * 27, (float) (scaleUnit * -2.5));
        
        addBlueprintLine("motor-1", (float) (scaleUnit * 3), lineDimension, (float) (scaleUnit * 29.5), (float) (scaleUnit * 2.5));
        addBlueprintLine("welder-1", (float) (scaleUnit * 1.5), lineDimension, (float) (scaleUnit * 28.75), 0);
        addBlueprintLine("welder-2", lineDimension, (float) (scaleUnit * 2.5 + lineDimension), (float) (scaleUnit * 29.5), (float) (scaleUnit * 1.25));
        addBlueprintLine("circuitPrinter-1", (float) (scaleUnit * 3), lineDimension, (float) (scaleUnit * 29.5), (float) (scaleUnit * -2.5));
        
        addBlueprintCell("satelliteDish",       28, scaleUnit * 32, (float) (scaleUnit * 2.5));
        addBlueprintCell("transmitterReceiver", 31, scaleUnit * 32, (float) (scaleUnit * -2.5));

        // Attach content to scroll rect
        scrollSR.content = contentContainer.GetComponent<RectTransform>();
    }

    private void addPrimaryCell(string name, int id, float xPos, float yPos) {
        GameObject cell = new GameObject(name + "Cell");
        cell.transform.SetParent(contentContainer.transform);
        RectTransform cellRT = (RectTransform) cell.AddComponent(typeof(RectTransform));
        cellRT.pivot = new Vector2(0.5f, 0.5f);
        cellRT.sizeDelta = new Vector2(primaryCellDimension, primaryCellDimension);
        cellRT.localPosition = new Vector2(xPos, yPos);
        cell.AddComponent(typeof(Image));
        cell.GetComponent<Image>().sprite = AssetManager.Instance().blueprintUICellPrimary;
        
        GameObject sprite = new GameObject(name + "Sprite");
        sprite.transform.SetParent(cell.transform);
        RectTransform spriteRT = (RectTransform) sprite.AddComponent(typeof(RectTransform));
        spriteRT.pivot = new Vector2(0.5f, 0.5f);
        spriteRT.sizeDelta = new Vector2((float) (primaryCellDimension * 0.5), (float) (primaryCellDimension * 0.5));
        spriteRT.localPosition = new Vector2(0, 0);
        sprite.AddComponent(typeof(Image));
        sprite.GetComponent<Image>().sprite = AssetManager.Instance().GetItemSprite(id);
    }

    private void addBlueprintCell(string name, int id, float xPos, float yPos) {
        GameObject cell = new GameObject(name + "Cell");
        cell.transform.SetParent(contentContainer.transform);
        RectTransform cellRT = (RectTransform) cell.AddComponent(typeof(RectTransform));
        cellRT.pivot = new Vector2(0.5f, 0.5f);
        cellRT.sizeDelta = new Vector2(primaryCellDimension * 2, primaryCellDimension * 2);
        cellRT.localPosition = new Vector2(xPos, yPos);
        cell.AddComponent(typeof(Image));
        cell.GetComponent<Image>().sprite = AssetManager.Instance().blueprintUICellDark;
        Button cellBtn = (Button) cell.AddComponent(typeof(Button));
        cellBtn.transition = Selectable.Transition.SpriteSwap;
        SpriteState ss = new SpriteState();
        ss.highlightedSprite = AssetManager.Instance().blueprintUICellDarkHighlight;
        cellBtn.spriteState = ss;
        cellBtn.onClick.AddListener(delegate { loadBlueprintTemplate(id); });
        
        GameObject sprite = new GameObject(name + "Sprite");
        sprite.transform.SetParent(cell.transform);
        RectTransform spriteRT = (RectTransform) sprite.AddComponent(typeof(RectTransform));
        spriteRT.pivot = new Vector2(0.5f, 0.5f);
        spriteRT.sizeDelta = new Vector2((float) (blueprintCellDimension * 0.64), (float) (blueprintCellDimension * 0.64));
        spriteRT.localPosition = new Vector2(0, 0);
        sprite.AddComponent(typeof(Image));
        sprite.GetComponent<Image>().sprite = AssetManager.Instance().GetBlueprintOutline(id);
    }

    private void addBlueprintLine(string name, float width, float height, float xPos, float yPos) {
        GameObject line = new GameObject(name + "Line");
        line.transform.SetParent(contentContainer.transform);
        RectTransform lineRT = (RectTransform) line.AddComponent(typeof(RectTransform));
        lineRT.pivot = new Vector2(0.5f, 0.5f);
        lineRT.sizeDelta = new Vector2(width, height);
        lineRT.localPosition = new Vector2(xPos, yPos);
        line.AddComponent(typeof(Image));
    }

    private void loadBlueprintTemplate(int id) {
        // This probably has side effects ngl
        EventSystem.current.SetSelectedGameObject(null);
        GameManager.Instance().uiStore.Dispatch(new OpenBlueprintTemplateUI(id));
    }
}
