	using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace View {
	public class Interactable : MonoBehaviour {
		[SerializeField] private int id;
		[SerializeField] private string type;
		private bool lockToggle;

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
}
