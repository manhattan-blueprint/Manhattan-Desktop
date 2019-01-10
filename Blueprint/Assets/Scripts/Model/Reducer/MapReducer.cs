using Model.Action;

namespace Model.Reducer {
    public class MapReducer: Reducer<MapState, MapAction>, MapVisitor {
        public MapState Reduce(MapState current, MapAction action) {
            throw new System.NotImplementedException();
        }
    }
}