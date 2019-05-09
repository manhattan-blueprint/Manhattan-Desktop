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
           
            // This doesn't seem to work, leaving as an issue
            foreach (Highlight h in GameObject.Find("MapGenerator").GetComponentsInChildren<Highlight>()) {
                h.enabled = false;
            }
            
            Material grass = Resources.Load<Material>("Grass");
            foreach (MeshRenderer renderer in GameObject.Find("MapGenerator").GetComponentsInChildren<MeshRenderer>()) {
                renderer.material = grass;
            }
        }

        public void CloseAlertView() {
            gameObject.GetComponent<Canvas>().enabled = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            GameObject.Find("PlayerCamera").GetComponent<PlayerLookController>().enabled = true;
            GameObject.Find("Player").GetComponent<PlayerMoveController>().enabled = true;
           
            Material grass = Resources.Load<Material>("Grass");
            foreach (Highlight h in GameObject.Find("MapGenerator").GetComponentsInChildren<Highlight>()) {
                h.enabled = true;
            }
            
        }
    }
}