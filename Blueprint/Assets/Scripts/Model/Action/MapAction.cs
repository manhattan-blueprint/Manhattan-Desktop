namespace Model.Action {
    public interface MapVisitor { }

    public abstract class MapAction : Action {
        public abstract void Accept(InventoryVisitor visitor);
    }
    
    // TODO: Define actions for the map


}