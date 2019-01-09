using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;

[Serializable]
public class GameObjects {
    public List<GameObjectEntry> items;
}

[Serializable]
public class GameObjectEntry {
    public int item_id, type, machine_id;
    public string name;
    public List<RecipeElement> blueprint, recipe;
}

[Serializable]
public class RecipeElement {
    public int item_id, quantity;
}

public class GameObjectsHandler {
    public GameObjects GameObjs;

    public GameObjectsHandler(string filepath) {
        this.GameObjs = parseJsonFile(filepath);
    }

    private GameObjects parseJsonFile(string filepath) { 
        using (StreamReader r = new StreamReader(filepath)) {
            string json = r.ReadToEnd();
            
            GameObjects returnObjects = JsonUtility.FromJson<GameObjects>(json);
            
            return returnObjects;
        }
    }
}