using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Model;
using Model.Action;
using Service.Request;
using TMPro;
using UnityEditor;
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
    private GameObject tooltip;
    private TextMeshProUGUI tooltipText;
    
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
        
        // Move tooltip into scroll container, cheers Unity
        tooltip = GameObject.Find("BTooltip");
        tooltip.transform.SetParent(contentContainer.transform);
        tooltipText = GameObject.Find("BTooltipText").GetComponent<TextMeshProUGUI>();
        tooltip.SetActive(false);
        
        // Add primary resource cells
        addPrimaryCell(1,  (float) (scaleUnit * 1.5), scaleUnit * 3);
        addPrimaryCell(2,  (float) (scaleUnit * 1.5), (float) (scaleUnit * 1.5));
        addPrimaryCell(3,  (float) (scaleUnit * 1.5), 0);
        addPrimaryCell(4,  (float) (scaleUnit * 1.5), (float) (scaleUnit * -1.5));
        addPrimaryCell(5,  (float) (scaleUnit * 1.5), scaleUnit * -3);
        addPrimaryCell(6,  scaleUnit * 3, scaleUnit * 3);
        addPrimaryCell(7,  scaleUnit * 3, (float) (scaleUnit * 1.5));
        addPrimaryCell(8,  scaleUnit * 3, 0);
        addPrimaryCell(9,  scaleUnit * 3, (float) (scaleUnit * -1.5));
        addPrimaryCell(10, scaleUnit * 3, scaleUnit * -3);
        
        // Add blueprint cells and lines
        addBlueprintCell(11, scaleUnit * 7,  0);
        
        addBlueprintLine("Furnace - 1", (float) (scaleUnit * 1.5), lineDimension, (float) (scaleUnit * 8.75), 0);
        addBlueprintLine("Furnace - 2", lineDimension, scaleUnit * 5 + (lineDimension), (float) (scaleUnit * 9.5), 0);
        addBlueprintLine("Furnace - 3", (float) (scaleUnit * 1.5), lineDimension, (float) (scaleUnit * 10.25), (float) (scaleUnit * 2.5));
        addBlueprintLine("Furnace - 4", (float) (scaleUnit * 1.5), lineDimension, (float) (scaleUnit * 10.25), (float) (scaleUnit * -2.5));
        
        addBlueprintCell(19, scaleUnit * 12, (float) (scaleUnit * 2.5));
        addBlueprintCell(18, scaleUnit * 12, (float) (scaleUnit * -2.5));
        
        addBlueprintLine("Machine base - 1", (float) (scaleUnit * 3), lineDimension, (float) (scaleUnit * 14.5), (float) (scaleUnit * 2.5));
        addBlueprintLine("Machine base - 2", lineDimension, (float) (scaleUnit * 5) + (lineDimension), (float) (scaleUnit * 14.5), 0);
        addBlueprintLine("Machine base - 3", (float) (scaleUnit * 1.5), lineDimension, (float) (scaleUnit * 15.25), (float) (scaleUnit * -2.5));
        
        addBlueprintCell(20, scaleUnit * 17, (float) (scaleUnit * 2.5));
        addBlueprintCell(25, scaleUnit * 17, (float) (scaleUnit * -2.5));
        
        addBlueprintLine("Wire drawer - 1", (float) (scaleUnit * 3), lineDimension, (float) (scaleUnit * 19.5), (float) (scaleUnit * 2.5));
        addBlueprintLine("Wire drawer - 2", lineDimension, (float) (scaleUnit * 5) + (lineDimension), (float) (scaleUnit * 19.5), 0);
        addBlueprintLine("Wire drawer - 3", (float) (scaleUnit * 1.5), lineDimension, (float) (scaleUnit * 20.25), (float) (scaleUnit * -2.5));
        
        addBlueprintCell(23, scaleUnit * 22, (float) (scaleUnit * 2.5));
        addBlueprintCell(22, scaleUnit * 22, (float) (scaleUnit * -2.5));
        
        addBlueprintLine("Copper coil - 1", (float) (scaleUnit * 3), lineDimension, (float) (scaleUnit * 24.5), (float) (scaleUnit * 2.5));
        addBlueprintLine("Copper coil - 2", lineDimension, (float) (scaleUnit * 5) + (lineDimension), (float) (scaleUnit * 24.5), 0);
        addBlueprintLine("Copper coil - 3", (float) (scaleUnit * 1.5), lineDimension, (float) (scaleUnit * 25.25), 0);
        addBlueprintLine("Insulated wire - 1", (float) (scaleUnit * 3), lineDimension, (float) (scaleUnit * 24.5), (float) (scaleUnit * -2.5));
        
        addBlueprintCell(24, scaleUnit * 27, (float) (scaleUnit * 2.5));
        addBlueprintCell(26, scaleUnit * 27, 0);
        addBlueprintCell(29, scaleUnit * 27, (float) (scaleUnit * -2.5));
        
        addBlueprintLine("Motor - 1", (float) (scaleUnit * 3), lineDimension, (float) (scaleUnit * 29.5), (float) (scaleUnit * 2.5));
        addBlueprintLine("Welder - 1", (float) (scaleUnit * 1.5), lineDimension, (float) (scaleUnit * 28.75), 0);
        addBlueprintLine("Welder - 2", lineDimension, (float) (scaleUnit * 2.5 + lineDimension), (float) (scaleUnit * 29.5), (float) (scaleUnit * 1.25));
        addBlueprintLine("Circuit printer - 1", (float) (scaleUnit * 3), lineDimension, (float) (scaleUnit * 29.5), (float) (scaleUnit * -2.5));
        
        addBlueprintCell(28, scaleUnit * 32, (float) (scaleUnit * 2.5));
        addBlueprintCell(31, scaleUnit * 32, (float) (scaleUnit * -2.5));
        
        // Add beacon outline
        GameObject beaconOutline = new GameObject("BeaconSprite");
        beaconOutline.transform.SetParent(contentContainer.transform);
        RectTransform beaconRT = (RectTransform) beaconOutline.AddComponent(typeof(RectTransform));
        beaconRT.pivot = new Vector2(0.5f, 0.5f);
        beaconRT.sizeDelta = new Vector2(primaryCellDimension * 5, primaryCellDimension * 5);
        beaconRT.localPosition = new Vector2(scaleUnit * 36, 0);
        Image beaconImage = (Image) beaconOutline.AddComponent(typeof(Image));
        beaconImage.sprite = AssetManager.Instance().blueprintBeacon;
         
        // Make tooltip appear on top
        tooltip.transform.SetAsLastSibling();

        // Attach content to scroll rect
        scrollSR.content = contentContainer.GetComponent<RectTransform>();
    }

    private void addPrimaryCell(int id, float xPos, float yPos) {
        string name = GameManager.Instance().sm.GameObjs.items.Find(x => x.item_id == id).name;
        GameObject cell = new GameObject(name + "Cell");
        cell.transform.SetParent(contentContainer.transform);
        RectTransform cellRT = (RectTransform) cell.AddComponent(typeof(RectTransform));
        cellRT.pivot = new Vector2(0.5f, 0.5f);
        cellRT.sizeDelta = new Vector2(primaryCellDimension, primaryCellDimension);
        cellRT.localPosition = new Vector2(xPos, yPos);
        cell.AddComponent(typeof(Image));
        cell.GetComponent<Image>().sprite = AssetManager.Instance().blueprintUICellPrimary;
        Tooltip cellTooltip = (Tooltip) cell.AddComponent(typeof(Tooltip));
        cellTooltip.tooltip = tooltip;
        cellTooltip.tooltipText = tooltipText;
        cellTooltip.tooltipText.fontSize = scaleUnit / 4;
        cellTooltip.text = name;
        
        GameObject sprite = new GameObject(name + "Sprite");
        sprite.transform.SetParent(cell.transform);
        RectTransform spriteRT = (RectTransform) sprite.AddComponent(typeof(RectTransform));
        spriteRT.pivot = new Vector2(0.5f, 0.5f);
        spriteRT.sizeDelta = new Vector2((float) (primaryCellDimension * 0.5), (float) (primaryCellDimension * 0.5));
        spriteRT.localPosition = new Vector2(0, 0);
        sprite.AddComponent(typeof(Image));
        sprite.GetComponent<Image>().sprite = AssetManager.Instance().GetItemSprite(id);
    }

    private void addBlueprintCell(int id, float xPos, float yPos) {
        string name = GameManager.Instance().sm.GameObjs.items.Find(x => x.item_id == id).name;
        bool completed = GameManager.Instance().completedBlueprints.Any(x => x.item_id == id);
        
        GameObject cell = new GameObject(name + "BlueprintCell");
        cell.transform.SetParent(contentContainer.transform);
        RectTransform cellRT = (RectTransform) cell.AddComponent(typeof(RectTransform));
        cellRT.pivot = new Vector2(0.5f, 0.5f);
        cellRT.sizeDelta = new Vector2(primaryCellDimension * 2, primaryCellDimension * 2);
        cellRT.localPosition = new Vector2(xPos, yPos);
        cell.AddComponent(typeof(Image));

        if (completed) {
            cell.GetComponent<Image>().sprite = AssetManager.Instance().blueprintUICellPrimary;
        } else {
            cell.GetComponent<Image>().sprite = AssetManager.Instance().blueprintUICellDark;
        }
        
        Button cellBtn = (Button) cell.AddComponent(typeof(Button));
        cellBtn.transition = Selectable.Transition.SpriteSwap;
        SpriteState ss = new SpriteState();
        
        if (completed) {
            ss.highlightedSprite = AssetManager.Instance().blueprintUICellPrimaryHighlight;
        } else {
            ss.highlightedSprite = AssetManager.Instance().blueprintUICellDarkHighlight;
        }
        
        cellBtn.spriteState = ss;
        cellBtn.onClick.AddListener(delegate { loadBlueprintTemplate(id); });
        
        GameObject sprite = new GameObject(name + "BlueprintSprite");
        sprite.transform.SetParent(cell.transform);
        RectTransform spriteRT = (RectTransform) sprite.AddComponent(typeof(RectTransform));
        spriteRT.pivot = new Vector2(0.5f, 0.5f);
        spriteRT.sizeDelta = new Vector2((float) (blueprintCellDimension * 0.6), (float) (blueprintCellDimension * 0.6));
        spriteRT.localPosition = new Vector2(0, 0);
        sprite.AddComponent(typeof(Image));
        
        if (completed) {
            sprite.GetComponent<Image>().sprite = AssetManager.Instance().GetItemSprite(id);
        } else {
            sprite.GetComponent<Image>().sprite = AssetManager.Instance().GetBlueprintOutline(id);
        }
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
