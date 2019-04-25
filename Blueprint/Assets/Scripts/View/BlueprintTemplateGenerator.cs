using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;

public class BlueprintTemplateGenerator : MonoBehaviour {

    void Start() {
        
        GameObject background = new GameObject("TemplateBackground");
        background.transform.SetParent(this.transform);
        RectTransform backgroundRT = (RectTransform) background.AddComponent(typeof(RectTransform));
        backgroundRT.anchorMin = Vector2.zero;
        backgroundRT.anchorMax = Vector2.one;
        backgroundRT.localPosition = new Vector2(0, 0);

        SVGImage backgroundImage = (SVGImage) background.AddComponent(typeof(SVGImage));
        backgroundImage.sprite = AssetManager.Instance().blueprintTemplateBackground;
    }
}
