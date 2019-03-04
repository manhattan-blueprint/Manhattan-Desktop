using System.Numerics;
using Vector2 = UnityEngine.Vector2;

namespace Model.Action {
    public interface MapVisitor {
        void visit(CellSelected cellSelected);
    }

    public abstract class MapAction : Action {
        public abstract void Accept(MapVisitor visitor);
    }
    
    /* User did select a cell on the grid */
    public class CellSelected: MapAction {
        public readonly Vector2 position;
        
        public CellSelected(Vector2 position) {
            this.position = position;
        }

        public override void Accept(MapVisitor visitor) {
            visitor.visit(this);
        }
    }

}