using Model.Action;

namespace Model.Reducer {
    public class MapReducer: Reducer<MapState, MapAction>, MapVisitor {
        private MapState state;
        public MapState Reduce(MapState current, MapAction action) {
            this.state = current; 
            
            action.Accept(this);
            return this.state;
        }

        public void visit(CellSelected cellSelected) {
            // Height of model for that ID
            
            // 
            
            
            if (state.getObjects().ContainsKey(cellSelected.position)) {
                state.removeObject(cellSelected.position);
            } else {
                state.addObject(cellSelected.position, cellSelected.id);
            }
        }
    }
}