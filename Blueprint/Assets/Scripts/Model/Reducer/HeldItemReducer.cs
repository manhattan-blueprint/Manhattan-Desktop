using Model.Action;
using Model.State;
using UnityEngine;

namespace Model.Reducer {
    public class HeldItemReducer: Reducer<HeldItemState, HeldItemAction>, HeldItemVisitor{
        private HeldItemState state;
        
        public HeldItemState Reduce(HeldItemState current, HeldItemAction action) {
            this.state = current; 
            action.Accept(this);
            return this.state;
        }

        public void visit(RotateHeldItemForward rotateHeldItemForward) {
            state.indexOfHeldItem = (state.indexOfHeldItem + 1) % 6;
        }

        public void visit(RotateHeldItemBackward rotateHeldItemBackward) {
            // +5 is the same as -1 mod 6, but avoids dealing with negative indices
            state.indexOfHeldItem = (state.indexOfHeldItem + 5) % 6;
        }
    }
}