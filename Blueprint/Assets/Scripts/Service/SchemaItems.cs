using System.Collections.Generic;
using System;

[Serializable]
public class SchemaItems {
    public List<SchemaItem> items;
}

[Serializable]
public class RecipeElement {
    public int item_id;
    public int quantity;

    public RecipeElement(int itemID, int quantity) {
        this.item_id = itemID;
        this.quantity = quantity;
    }
}

[Serializable]
public class FuelElement {
    public int item_id;

    public FuelElement(int itemID) {
        this.item_id = itemID;
    }

    public override bool Equals(object obj) {
        FuelElement fuelElement = obj as FuelElement;
        if (fuelElement != null) {
            return item_id == fuelElement.item_id;
        }

        return false;
    }
}

[Serializable]
public class SchemaItem {
    public enum ItemType {
        PrimaryResource = 1,
        BlueprintCraftedMachine= 2,
        MachineCraftedComponent = 3,
        BlueprintCraftedComponent = 4,
        Intangible = 5,
    }
    
    public int item_id;
    public int machine_id;
    public string name;
    public ItemType type; 
    public List<RecipeElement> blueprint;
    public List<RecipeElement> recipe;
    public List<FuelElement> fuel;

    public bool isMachine() {
        return type == ItemType.BlueprintCraftedMachine;
    }

    public bool isPoweredByElectricity() {
        return fuel.Contains(new FuelElement(32));
    }
}

