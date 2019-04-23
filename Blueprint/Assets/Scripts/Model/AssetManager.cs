using System;
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

        public readonly Sprite blueprintUIBackground = Resources.Load("UI/blueprint-ui-background", typeof(Sprite)) as Sprite;
        public readonly Sprite blueprintUICellPrimary = Resources.Load("UI/blueprint-ui-cell-primary", typeof(Sprite)) as Sprite;
        public readonly Sprite blueprintUICellPrimaryHighlight = Resources.Load("UI/blueprint-ui-cell-primary-hl", typeof(Sprite)) as Sprite;
        public readonly Sprite blueprintUICellDark = Resources.Load("UI/blueprint-ui-cell-dark", typeof(Sprite)) as Sprite;
        public readonly Sprite blueprintUICellDarkHighlight = Resources.Load("UI/blueprint-ui-cell-dark-hl", typeof(Sprite)) as Sprite;

        public readonly Sprite outlineFurnace = Resources.Load("UI/outlines/furnace", typeof(Sprite)) as Sprite;
        
        private AssetManager() { }

        public static AssetManager Instance() {
            if (instance == null) {
                instance = new AssetManager();
            }

            return instance;
        }

        // Get the associated model for a given ID
        public GameObject GetModel(int id) {
            String baseLocation = "Models/3D/";
            GameObject gameObject = Resources.Load(baseLocation + "model_" + id) as GameObject;
            
            // Load default game object if doesn't exist
            if (gameObject == null) {
                gameObject = Resources.Load(baseLocation + "model_default") as GameObject; 
            }
            return gameObject;
        }

        // Get the associated UI sprite for a given item ID
        public Sprite GetItemSprite(int id) {
            String baseLocation = "Models/2D/";
            Sprite sprite = Resources.Load(baseLocation + "sprite_" + id, typeof(Sprite)) as Sprite;

            // Load default if object sprite doesn't exist
            if (sprite == null) {
                sprite = Resources.Load(baseLocation + "sprite_default", typeof(Sprite)) as Sprite;
            }

            return sprite;
        }

        public Sprite GetBlueprintOutline(int id) {
            String baseLocation = "UI/outlines/";
            Sprite sprite = Resources.Load(baseLocation + "outline_" + id, typeof(Sprite)) as Sprite;
            
            // Load default if object sprite doesn't exist
            if (sprite == null) {
                sprite = Resources.Load("Models/2D/sprite_default", typeof(Sprite)) as Sprite;
            }

            return sprite;
        }

        // Get the associated UI sprite for a given item ID
        public Texture GetItemTexture(int id, string path="") {
            Texture texture = Resources.Load(path + "sprite_" + id, typeof(Texture)) as Texture;

            // Load default if object sprite doesn't exist
            if (texture == null) {
                texture = Resources.Load("sprite_default", typeof(Texture)) as Texture;
            }

            return texture;
        }
    }
}
