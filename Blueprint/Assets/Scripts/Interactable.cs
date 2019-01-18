	using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {
	[SerializeField] int id;
	[SerializeField] string type;
	[SerializeField] bool lockToggle;

	public int GetId() {
		return this.id;
	}

	public string GetItemType() {
		return this.type;
	}

	public void LockObject() {
		this.lockToggle = true;
	}

	public void UnlockObject() {
		this.lockToggle = false;
	}
}
