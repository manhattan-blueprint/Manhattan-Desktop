namespace Model {
    public class MachineProduct {
        public readonly SchemaItem item;
        public readonly int maxQuantity;

        public MachineProduct(SchemaItem item, int maxQuantity) {
            this.item = item;
            this.maxQuantity = maxQuantity;
        }
    }
}