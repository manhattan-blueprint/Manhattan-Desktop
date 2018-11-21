using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuShow : MonoBehaviour {

	private Canvas CanvasObject; // Assign in inspector
	
	void Start()
		 {
			 CanvasObject = GetComponent<Canvas> ();
			 Debug.Log(CanvasObject.name);
			 CanvasObject.enabled = false;
		 }
 
	void Update() 
	{
		if (Input.GetKeyUp(KeyCode.E)) 
		{
			CanvasObject.enabled = !CanvasObject.enabled;
		}
	}
}
