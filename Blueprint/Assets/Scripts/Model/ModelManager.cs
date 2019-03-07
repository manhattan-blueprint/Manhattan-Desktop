using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Model {
    public class ModelManager {
        private static ModelManager instance;
    
        private ModelManager() { }
    
        public static ModelManager Instance() {
            if (instance == null) {
                instance = new ModelManager();
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
