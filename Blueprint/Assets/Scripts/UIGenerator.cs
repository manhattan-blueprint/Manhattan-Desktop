using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.UI;

public class UIGenerator : MonoBehaviour {
	[SerializeField] GameObject canvas;
	[SerializeField] GameObject panel;
	[SerializeField] int numSlots;
	[SerializeField] int numRows;
	[SerializeField] Sprite sprite;
	[SerializeField] string buttonText;
	[SerializeField] Font buttonFont;
	private Vector2 cellSize;
	private GridLayoutGroup slotGrid;
	private Inventory inventory;


	// Use this for initialization
	void Start () {
		cellSize = new Vector2(panel.GetComponent<RectTransform>().rect.width / (numRows), panel.GetComponent<RectTransform>().rect.height / (numRows));
		slotGrid = SetUpSlotGrid(panel);
		inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();

		for (var i = 0; i < numSlots; i++) {
			var gridChild = SetUpGridChild(i);
			var slot = SetUpSlot(i, gridChild);
			var button = SetUpButton(slot, i);
			var text = SetUpText(button);

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
		var gridChild = new GameObject(GetGridChildName(i), typeof(RectTransform));
		gridChild.GetComponent<RectTransform>().SetParent(slotGrid.transform);
		return gridChild;
	}
	
	private GameObject SetUpSlot(int i, GameObject gridChild) {	
		var slot = new GameObject(GetSlotName(i), typeof(RectTransform));
		var slotImg = slot.AddComponent<Image>();
		var script = slot.AddComponent<ItemSlot>();
		script.SetId(i);
		//var fitter = slot.AddComponent<AspectRatioFitter>();
		//fitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
		//fitter.aspectRatio = 1;
		slot.GetComponent<RectTransform>().sizeDelta = cellSize;
		slot.GetComponent<RectTransform>().SetParent(gridChild.transform);
		return slot;
	}

	private GameObject SetUpButton(GameObject slot, int i) {
		var button = new GameObject(GetButtonName(i), typeof(RectTransform));
		button.GetComponent<RectTransform>().sizeDelta = new Vector2(slot.GetComponent<RectTransform>().sizeDelta.x, slot.GetComponent<RectTransform>().sizeDelta.y/4);
		button.GetComponent<RectTransform>().position = new Vector2(0, slot.GetComponent<RectTransform>().sizeDelta.y/3);
		button.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
		button.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
		button.GetComponent<RectTransform>().SetParent(slot.transform);
		var buttonImg = button.AddComponent<Image>();
		buttonImg.sprite = sprite;
		buttonImg.type = Image.Type.Sliced;
		buttonImg.fillCenter = true;
		var press = button.AddComponent<Button>();
		press.onClick.AddListener(() => slot.GetComponent<ItemSlot>().DropItem());
		return button;
	}

	private GameObject SetUpText(GameObject button) {
		var text = new GameObject(GetTextName(), typeof(RectTransform));
		var btnTxt = text.AddComponent<Text>();
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
