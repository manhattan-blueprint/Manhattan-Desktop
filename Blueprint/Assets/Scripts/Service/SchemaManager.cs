using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Model;
using Service;
using UnityEngine;

public class SchemaManager {
    public SchemaItems GameObjs;
    
    public SchemaManager(SchemaItems gameObjs) {
        this.GameObjs = gameObjs;
    }

    public static SchemaManager FromFilepath(string filepath) {
       return new SchemaManager(parseJsonSchemaFromFile(filepath));
    }

    private static SchemaItems parseJsonSchemaFromFile(string filepath) {
        using (StreamReader r = new StreamReader(filepath)) {
            string json = r.ReadToEnd();
            SchemaItems returnObjects = JsonUtility.FromJson<SchemaItems>(json);
            return returnObjects;
        }
    }

    // Takes ItemIds of available objects, and itemId of target object
    // If targetItem can be made, return corresponding GameObjectEntry
    public Optional<SchemaItem> GetBlueprint(List<RecipeElement> availableItems, int targetItemID) {
        SchemaItem entry = GameObjs.items.Find(item => item.item_id == targetItemID);
        
        foreach (RecipeElement item in entry.blueprint) {
            RecipeElement available = availableItems.Find(itemToFind => itemToFind.item_id == item.item_id);
            
            // If item isn't present or don't have enough, fail
            if (available == null || available.quantity < item.quantity) {
                return Optional<SchemaItem>.Empty();
            }
        }

        return Optional<SchemaItem>.Of(entry);
    }

    public Optional<MachineProduct> GetRecipe(List<RecipeElement> inputItems, int machineId) {
        // Find objects that can be produced by machine
        List<SchemaItem> objects = GameObjs.items.FindAll(item => item.machine_id == machineId);
        
        foreach (SchemaItem obj in objects) {
            int maxQuantity = int.MaxValue;
            Boolean correctItem = true;
            
            foreach (RecipeElement recipeItem in obj.recipe) {
                List<RecipeElement> available = inputItems.FindAll(itemToFind => itemToFind.item_id == recipeItem.item_id);
                
                // If item isn't present or don't have enough, fail
                if (available.Count > 0) {
                    foreach (RecipeElement element in available) {
                        if (element == null || element.quantity < recipeItem.quantity) {
                            correctItem = false;
                            break;
                        } else {
                            int maxMultiple = element.quantity / recipeItem.quantity;
                            maxQuantity = Math.Min(maxQuantity, maxMultiple);
                        }
                    }
                } else {
                    correctItem = false;
                }
            }

            // Success case
            if (correctItem) {
                return Optional<MachineProduct>.Of(new MachineProduct(obj, maxQuantity));
            }
        }
        return Optional<MachineProduct>.Empty();
    }
}
