using System;
using System.Collections.Generic;

namespace Model {
    [Serializable]
    public class Machine {
        public readonly int id;
        public Optional<InventoryItem> leftInput;
        public Optional<InventoryItem> rightInput;
        public Optional<InventoryItem> fuel;
        public Optional<InventoryItem> output;

        public Machine(int id) {
            this.id = id;
            this.leftInput = Optional<InventoryItem>.Empty();
            this.rightInput = Optional<InventoryItem>.Empty();
            this.fuel = Optional<InventoryItem>.Empty();
            this.output = Optional<InventoryItem>.Empty();
        } 

        // Flattens left and right input into a list of recipe elements
        public List<RecipeElement> GetInputs() {
            List<RecipeElement> recipeElements = new List<RecipeElement>();
            if (leftInput.IsPresent()) {
                InventoryItem left = leftInput.Get();
                recipeElements.Add(new RecipeElement(left.GetId(), left.GetQuantity()));
            }

            if (rightInput.IsPresent()) {
                InventoryItem right = rightInput.Get();
                recipeElements.Add(new RecipeElement(right.GetId(), right.GetQuantity()));
            }

            return recipeElements;
        }
        
        // Validate the machine has fuel
        // For some machines this is the presence of a material (coal / wood)
        // For others it is connection of electricity
        public bool HasFuel() {
            GameObjectEntry entry = GameManager.Instance().goh.GameObjs.items.Find(x => x.item_id == this.id);
            if (entry == null) return false;
          
            // If powered by electricity
            if (entry.fuel.Contains(new FuelElement(32))) {
                // TODO: compute if connected
                return true;
            }
            
            // If powered by some other fuel
            if (!fuel.IsPresent()) {
                return false;
            } 
            
            // Find any fuel in the schema that matches the fuel in the machine
            return entry.fuel.Find(x => x.item_id == fuel.Get().GetId()) != null;
        }
    }
}