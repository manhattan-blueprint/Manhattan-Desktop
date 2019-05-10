using TMPro;
using UnityEngine;
using View;

namespace Controller {
    public class AlertController : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI messageText;

        public void SetAlert(string title, string message) {
            this.titleText.text = title;
            this.messageText.text = message;
        }

        public void ShowAlertView() {
            gameObject.GetComponent<Canvas>().enabled = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            GameObject.Find("PlayerCamera").GetComponent<PlayerLookController>().enabled = false;
            GameObject.Find("Player").GetComponent<PlayerMoveController>().enabled = false;
        }

        public void CloseAlertView() {
            gameObject.GetComponent<Canvas>().enabled = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            GameObject.Find("PlayerCamera").GetComponent<PlayerLookController>().enabled = true;
            GameObject.Find("Player").GetComponent<PlayerMoveController>().enabled = true;
        }
    }
}