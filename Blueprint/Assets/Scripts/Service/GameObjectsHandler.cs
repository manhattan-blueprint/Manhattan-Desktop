using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Model;
using Service;
using UnityEngine;

public class GameObjectsHandler {
    public GameObjects GameObjs;

    private GameObjectsHandler(GameObjects gameObjs) {
        this.GameObjs = gameObjs;
    }

    public static GameObjectsHandler FromFilepath(string filepath) {
       return new GameObjectsHandler(parseJsonSchemaFromFile(filepath));
    }

    public static GameObjectsHandler FromRemote() {
        GameObjectsHandler goh = null;
        Task.Run(async () => {
            APIResult<string, JsonError> response = await BlueprintAPI.DefaultCredentials().AsyncGetItemSchema();
            if (response.isSuccess()) {
                goh = new GameObjectsHandler(JsonUtility.FromJson<GameObjects>(response.GetSuccess()));
            } else {
                throw new InvalidDataException(response.GetError().error);
            }

        }).GetAwaiter().GetResult();
        return goh;
    }

    private static GameObjects parseJsonSchemaFromFile(string filepath) {
        using (StreamReader r = new StreamReader(filepath)) {
            string json = r.ReadToEnd();
            GameObjects returnObjects = JsonUtility.FromJson<GameObjects>(json);
            return returnObjects;
        }
    }

    // Takes ItemIds of available objects, and itemId of target object
    // If targetItem can be made, return corresponding GameObjectEntry
    public Optional<GameObjectEntry> GetBlueprint(List<RecipeElement> availableItems, int targetItemID) {
        GameObjectEntry entry = GameObjs.items.Find(item => item.item_id == targetItemID);
        
        foreach (RecipeElement item in entry.blueprint) {
            RecipeElement available = availableItems.Find(itemToFind => itemToFind.item_id == item.item_id);
            
            // If item isn't present or don't have enough, fail
            if (available == null || available.quantity < item.quantity) {
                return Optional<GameObjectEntry>.Empty();
            }
        }

        return Optional<GameObjectEntry>.Of(entry);
    }

    public Optional<GameObjectEntry> GetRecipe(List<RecipeElement> inputItems, int machineId) {
        // Find objects that can be produced by machine
        List<GameObjectEntry> objects = GameObjs.items.FindAll(item => item.machine_id == machineId);
        
        foreach (GameObjectEntry obj in objects) {
            Boolean correctItem = true;
            
            foreach (RecipeElement recipeItem in obj.recipe) {
                RecipeElement available = inputItems.Find(itemToFind => itemToFind.item_id == recipeItem.item_id);
                
                // If item isn't present or don't have enough, fail
                if (available == null || available.quantity < recipeItem.quantity) {
                    correctItem = false;
                }
            }

            // Success case
            if (correctItem) {
                return Optional<GameObjectEntry>.Of(obj);
            }
        }
        return Optional<GameObjectEntry>.Empty();
    }
}
