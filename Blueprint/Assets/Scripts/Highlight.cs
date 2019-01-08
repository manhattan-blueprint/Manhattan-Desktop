﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour {

	// Use this for initialization
	void Start () {
		rend = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	public Color tempColor;
	public Color highlightColor;
	private Renderer rend;
	public bool Holdable;
	
	void OnMouseEnter() {
		tempColor = rend.material.color;
		rend.material.color = highlightColor;
	}

	private void OnMouseDown() {
		if (!Holdable) return;
		rend.material.color = Color.yellow;
	}

	void OnMouseExit() {
		rend.material.color = tempColor;
	}

}