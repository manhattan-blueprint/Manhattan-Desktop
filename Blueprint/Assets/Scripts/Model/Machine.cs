using System.Collections.Generic;

namespace Model {
    public class Machine {
        // Instead of ID, maybe we make this a type like electrical or coal powered so we don't have to 
        // look up? 
        public int id;
        public Optional<InventoryItem> leftInput;
        public Optional<InventoryItem> rightInput;
        public Optional<InventoryItem> fuel;
        public Optional<InventoryItem> output;

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
    }
}