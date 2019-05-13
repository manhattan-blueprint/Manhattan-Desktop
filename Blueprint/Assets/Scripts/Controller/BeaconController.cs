using UnityEngine;
using Model.Action;
using Model.State;

namespace Controller {
    public class BeaconController : MonoBehaviour {
        private GameObject player;
        private const float minDistance = 7;

        public void Start() {
            player = GameObject.Find("Player");
        }

        public void OnMouseOver() {
            float dist = Vector3.Distance(player.transform.position, transform.position);
            if (dist < minDistance && GameManager.Instance().uiStore.GetState().Selected == UIState.OpenUI.Playing) {
                GameManager.Instance().uiStore.Dispatch(new OpenBeaconMouseUI());
            }  else if (dist > minDistance && GameManager.Instance().uiStore.GetState().Selected == UIState.OpenUI.BeaconMouse) {
                GameManager.Instance().uiStore.Dispatch(new CloseUI());
            }
        }

        public void OnMouseExit() {
            if (GameManager.Instance().uiStore.GetState().Selected == UIState.OpenUI.BeaconMouse) {
                GameManager.Instance().uiStore.Dispatch(new CloseUI());
            }
        }
    }
}
