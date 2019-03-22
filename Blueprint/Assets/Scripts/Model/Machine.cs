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
    }
}