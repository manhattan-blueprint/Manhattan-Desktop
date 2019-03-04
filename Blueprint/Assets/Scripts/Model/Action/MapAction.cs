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
        public readonly int id;
        
        public CellSelected(Vector2 position, int id) {
            this.position = position;
            this.id = id;
        }

        public override void Accept(MapVisitor visitor) {
            visitor.visit(this);
        }
    }

}