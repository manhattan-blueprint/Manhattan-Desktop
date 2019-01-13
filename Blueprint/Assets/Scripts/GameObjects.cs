using System.Collections.Generic;
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

    public RecipeElement(int itemId, int quantity) {
        item_id = itemId;
        this.quantity = quantity;
    }
}