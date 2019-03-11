﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Controller;

public class UIGenerator : MonoBehaviour {
    [SerializeField] private GameObject panel;
    [SerializeField] private  int numSlots;
    [SerializeField] private int numRows;
    [SerializeField] private Sprite sprite;
    [SerializeField] private string buttonText;
    [SerializeField] private Font buttonFont;
    private Vector2 cellSize;
    private GridLayoutGroup slotGrid;


    // Use this for initialization
    void Start () {
        cellSize = new Vector2(panel.GetComponent<RectTransform>().rect.width / (numRows), panel.GetComponent<RectTransform>().rect.height / (numRows));
        slotGrid = SetUpSlotGrid(panel);

        for (int i = 0; i < numSlots; i++) {
            GameObject gridChild = SetUpGridChild(i);
            GameObject slot = SetUpSlot(i, gridChild);
            GameObject button = SetUpButton(slot, i);
            GameObject text = SetUpText(button);

            gridChild.SetActive(true);
            slot.SetActive(true);
            button.SetActive(true);
            text.SetActive(true);
        }
    }

    private string GetGridChildName(int i) {
        return "GridChild" + ++i;
    }

    private string GetSlotName(int i) {
        return "Slot" + ++i;
    }

    private string GetButtonName(int i) {
        return "Button" + ++i;
    }

    private string GetTextName() {
        return "Text";
    }

    private GridLayoutGroup SetUpSlotGrid(GameObject panel) {
        GridLayoutGroup grid = panel.AddComponent<GridLayoutGroup>();
        grid.cellSize = cellSize;
        grid.startCorner = GridLayoutGroup.Corner.UpperLeft;
        grid.startAxis = GridLayoutGroup.Axis.Horizontal;
        grid.childAlignment = TextAnchor.UpperLeft;
        grid.constraint = GridLayoutGroup.Constraint.Flexible;
        return grid;
    }

    private GameObject SetUpGridChild(int i) {
        GameObject gridChild = new GameObject(GetGridChildName(i), typeof(RectTransform));
        gridChild.GetComponent<RectTransform>().SetParent(slotGrid.transform);
        return gridChild;
    }

    private GameObject SetUpSlot(int i, GameObject gridChild) {
        GameObject slot = new GameObject(GetSlotName(i), typeof(RectTransform));
        Image slotImg = slot.AddComponent<Image>();
        InventorySlotController script = slot.AddComponent<InventorySlotController>();
        script.SetId(i);
        // This will be used in future to scale UI to resolution
        //var fitter = slot.AddComponent<AspectRatioFitter>();
        //fitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
        //fitter.aspectRatio = 1;
        slot.GetComponent<RectTransform>().sizeDelta = cellSize;
        slot.GetComponent<RectTransform>().SetParent(gridChild.transform);
        return slot;
    }

    private GameObject SetUpButton(GameObject slot, int i) {
        GameObject button = new GameObject(GetButtonName(i), typeof(RectTransform));
        button.GetComponent<RectTransform>().sizeDelta = new Vector2(slot.GetComponent<RectTransform>().sizeDelta.x, slot.GetComponent<RectTransform>().sizeDelta.y/4);
        button.GetComponent<RectTransform>().position = new Vector2(0, slot.GetComponent<RectTransform>().sizeDelta.y/3);
        button.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        button.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
        button.GetComponent<RectTransform>().SetParent(slot.transform);
        Image buttonImg = button.AddComponent<Image>();
        buttonImg.sprite = sprite;
        buttonImg.type = Image.Type.Sliced;
        buttonImg.fillCenter = true;
        Button press = button.AddComponent<Button>();
        press.onClick.AddListener(() => slot.GetComponent<InventorySlotController>().DropItem());
        return button;
    }

    private GameObject SetUpText(GameObject button) {
        GameObject text = new GameObject(GetTextName(), typeof(RectTransform));
        Text btnTxt = text.AddComponent<Text>();
        btnTxt.font = buttonFont;
        btnTxt.text = buttonText;
        btnTxt.fontSize = 18;
        btnTxt.color = Color.black;
        btnTxt.alignment = TextAnchor.MiddleCenter;
        text.GetComponent<RectTransform>().SetParent(button.transform);
        text.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        text.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        text.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        text.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
        text.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        return text;
    }

}
