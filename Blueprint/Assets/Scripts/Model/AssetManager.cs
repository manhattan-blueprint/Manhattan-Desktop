using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model {
    public class AssetManager {
        private static AssetManager instance;
        public readonly Font FontHelveticaNeueBold = Resources.Load("Fonts/HelveticaNeueBold", typeof(Font)) as Font;
        public readonly Color ColourOffWhite = new Color32(245, 245, 245, 255);
        public readonly int QuantityFieldFontSize = (int) Mathf.Round(Screen.height/50);
        
        public readonly Sprite backgroundSprite = Resources.Load("inventory_slot", typeof(Sprite)) as Sprite;
        public readonly Sprite highlightSprite = Resources.Load("slot_border_highlight", typeof(Sprite)) as Sprite;
        public readonly Sprite borderSprite = Resources.Load("slot_border", typeof(Sprite)) as Sprite;
        public readonly Sprite outerBorderSprite = Resources.Load("slot_border_outer", typeof(Sprite)) as Sprite;
    
        private AssetManager() { }
    
        public static AssetManager Instance() {
            if (instance == null) {
                instance = new AssetManager();
            }
            
            return instance;
        }
    
        // Get the associated model for a given ID
        public GameObject GetModel(int id) {
            GameObject gameObject = Resources.Load("model_" + id) as GameObject;
            // Load default game object if doesn't exist
            if (gameObject == null) {
                gameObject = Resources.Load("model_default") as GameObject; 
            }
    
            return gameObject;
        }
        
        // Get the associated UI sprite for a given item ID
        public Sprite GetItemSprite(int id) {
            Sprite sprite = Resources.Load("sprite_" + id, typeof(Sprite)) as Sprite;

            // Load default if object sprite doesn't exist
            if (sprite == null) {
                sprite = Resources.Load("sprite_default", typeof(Sprite)) as Sprite;
            }

            return sprite;
        }
    }
}
