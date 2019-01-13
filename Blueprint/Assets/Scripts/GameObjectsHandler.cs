using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameObjectsHandler {
    public GameObjects GameObjs;

    public GameObjectsHandler(string filepath) {
        this.GameObjs = parseJsonSchema(filepath);
    }

    private GameObjects parseJsonSchema(string filepath) { 
        using (StreamReader r = new StreamReader(filepath)) {
            string json = r.ReadToEnd();
            
            GameObjects returnObjects = JsonUtility.FromJson<GameObjects>(json);
            
            return returnObjects;
        }
    }

    // Takes ItemIds of available objects, and itemId of target object
    // If targetItem can be made, return corresponding GameObjectEntry
    public GameObjectEntry GetBlueprint(List<RecipeElement> availableItems, int targetItemId) {
        // Obtain blueprint from targetItemId
        GameObjectEntry goe = GameObjs.items.Find(item => item.item_id == targetItemId);
        
        // ForEach item in the blueprint, check the required items are available
        foreach (RecipeElement item in goe.blueprint) {
            // Find recipe element in provided available elements
            RecipeElement available = availableItems.Find(itemToFind => itemToFind.item_id == item.item_id);
            
            // If item is present
            if (available != null) {
                // Is the required quantity available?
                if (available.quantity >= item.quantity) {
                    
                }
                else {
                    throw new InvalidDataException("Not enough of item " + item.item_id + " available.");
                }
            }
            // If item is not present
            else {
                throw new InvalidDataException("No item " + item.item_id + " present.");
            }
        }      

        return goe;
    }

    // Given available items and a machineId
    // Returns possible object
    public GameObjectEntry GetRecipe(List<RecipeElement> inputItems, int machineId) {
        // Find objects that can be produced by machineId 
        List<GameObjectEntry> objects = GameObjs.items.FindAll(item => item.machine_id == machineId);
        
        // Find the correct output item for given input items
        foreach (GameObjectEntry obj in objects) {
            // For each item in the object's recipe
            foreach (RecipeElement recipeItem in obj.recipe) {
                Boolean correctItem = true;
                
                // Is the required item present?
                RecipeElement available = inputItems.Find(itemToFind => itemToFind.item_id == recipeItem.item_id);
                
                // If present
                if (available != null) {
                    // Is the required quantity available?
                    if (available.quantity >= recipeItem.quantity) {
                        // Enough item present for output item
                    }
                    else {
                        // Not enough item available
                        correctItem = false;
                    }
                }
                // If item is not present
                else {
                    correctItem = false;
                }

                // Success case
                if (correctItem) {
                    return obj;
                }
            }
        }

        // Failure case, no valid outputs
        return null;
    }
}
