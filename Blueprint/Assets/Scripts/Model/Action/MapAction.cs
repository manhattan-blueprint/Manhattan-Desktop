using System.Numerics;
using Vector2 = UnityEngine.Vector2;

namespace Model.Action {
    public interface MapVisitor {
        void visit(UpdateGoal updateGoal);
        void visit(PlaceItem placeItem);
        void visit(CollectItem collectItem);
        void visit(RotateItem rotateItem);
    }

    public abstract class MapAction : Action {
        public abstract void Accept(MapVisitor visitor);
    }
    
    /* Update the goal progress */
    public class UpdateGoal: MapAction {
        public readonly GoalPosition goalPosition;
        
        public UpdateGoal(GoalPosition goalPosition) {
            this.goalPosition = goalPosition;
        }

        public override void Accept(MapVisitor visitor) {
            visitor.visit(this);
        }
    }
    
    /* Place an item at grid position */
    public class PlaceItem: MapAction {
        public readonly Vector2 position;
        public readonly int itemID;
        
        public PlaceItem(Vector2 position, int itemID) {
            this.position = position;
            this.itemID = itemID;
        }

        public override void Accept(MapVisitor visitor) {
            visitor.visit(this);
        }
    }
   
    /* Collect an item from a grid position */
    public class CollectItem: MapAction {
        public readonly Vector2 position;
        
        public CollectItem(Vector2 position) {
            this.position = position;
        }

        public override void Accept(MapVisitor visitor) {
            visitor.visit(this);
        }
    }
    
    /* Rotate an item at grid position */
    public class RotateItem : MapAction {
        public readonly Vector2 position;

        public RotateItem(Vector2 position) {
            this.position = position;
        }
        public override void Accept(MapVisitor visitor) {
            visitor.visit(this);
        }
    }

}