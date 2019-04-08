using Model.Action;
using Model.State;

namespace Model.Reducer {
    public class MapReducer: Reducer<MapState, MapAction>, MapVisitor {
        private MapState state;
        public MapState Reduce(MapState current, MapAction action) {
            this.state = current; 
            action.Accept(this);
            return this.state;
        }

        public void visit(PlaceItem placeItem) {
            if (!state.getObjects().ContainsKey(placeItem.position)) {
                state.addObject(placeItem.position, placeItem.itemID);
            }
        }

        public void visit(CollectItem collectItem) {
            if (state.getObjects().ContainsKey(collectItem.position)) {
                MapObject obj = state.getObjects()[collectItem.position];
               
                string name = GameManager.Instance().sm.GameObjs.items[obj.GetID() - 1].name;
                
                state.removeObject(collectItem.position);
                GameManager.Instance().inventoryStore.Dispatch(new AddItemToInventory(obj.GetID(), 1, name));
            }
        }
    }
}