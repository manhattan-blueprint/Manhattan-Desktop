using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    public GameObject tooltip;
    public string text = "";
    public TextMeshProUGUI tooltipText;
    
    public void OnPointerEnter(PointerEventData eventData) {
        tooltipText.text = text;
        tooltip.transform.position = transform.position;
        tooltip.SetActive(true);
    }
    
    public void OnPointerExit(PointerEventData eventData) {
        
        tooltip.SetActive(false);
    }
}
