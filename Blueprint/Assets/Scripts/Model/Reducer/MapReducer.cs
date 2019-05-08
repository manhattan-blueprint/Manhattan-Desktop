using Model.Action;
using Model.State;
using UnityEngine;
using Utils;

namespace Model.Reducer {
    public class MapReducer: Reducer<MapState, MapAction>, MapVisitor {
        private MapState state;
        public MapState Reduce(MapState current, MapAction action) {
            this.state = current; 
            action.Accept(this);
            return this.state;
        }

        public void visit(UpdateGoal updateGoal) {
            state.addGoalItem(updateGoal.goalPosition);
        }

        public void visit(PlaceItem placeItem) {
            if (!state.GetObjects().ContainsKey(placeItem.position)) {
                // Rotation is set to 30 to negate incorrect model rotation
                state.AddObject(placeItem.position, placeItem.itemID, 30);

                SchemaItem item = GameManager.Instance().sm.GameObjs.items
                    .Find(x => x.item_id == placeItem.itemID);
                
                // If solar panel, wire or machine, do wires
                if (placeItem.itemID == 25  || placeItem.itemID == 22 || (item.isMachine() && item.isPoweredByElectricity())) {
                    foreach (Vector2 hexNeighbour in placeItem.position.HexNeighbours()) {
                        // If neighbour is empty, skip
                        if (!state.GetObjects().ContainsKey(hexNeighbour)) continue;
                        
                        int neighbourID = state.GetObjects()[hexNeighbour].GetID();
                        
                        SchemaItem neighbour = GameManager.Instance().sm.GameObjs.items
                            .Find(x => x.item_id == neighbourID);
                       
                        // If solar and solar, don't connect
                        if (neighbourID == 25 && placeItem.itemID == 25) continue;
                        // If machine and machine, don't connect
                        if (item.isMachine() && neighbour.isMachine()) continue;
                        
                        // If solar, wire or electricity powered machine, continue
                        if (neighbourID == 25 || neighbourID == 22 || (neighbour.isMachine() && neighbour.isPoweredByElectricity())) {
                            state.AddWirePath(new WirePath(placeItem.position, hexNeighbour));
                        } 
                    }
                }
                GameManager.Instance().machineStore.Dispatch(new UpdateConnected());
            }
        }

        public void visit(CollectItem collectItem) {
            if (state.GetObjects().ContainsKey(collectItem.position)) {
                MapObject obj = state.GetObjects()[collectItem.position];
               
                string name = GameManager.Instance().sm.GameObjs.items[obj.GetID() - 1].name;
                
                state.RemoveObject(collectItem.position);
                state.RemoveWirePaths(collectItem.position);
                GameManager.Instance().inventoryStore.Dispatch(new AddItemToInventory(obj.GetID(), 1, name));
                GameManager.Instance().machineStore.Dispatch(new UpdateConnected());
            }
        }

        public void visit(RotateItem rotateItem) {
            if (state.getObjects().ContainsKey(rotateItem.position)) {
                state.RotateObject(rotateItem.position);
            }
        }
    }
}