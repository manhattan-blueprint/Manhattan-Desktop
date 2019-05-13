using Model.Action;
using Model.State;
using UnityEngine;

namespace Model.Reducer {
    public class TutorialReducer: Reducer<TutorialState, TutorialAction>, TutorialVisitor {
        private TutorialState state;
        
        public TutorialState Reduce(TutorialState current, TutorialAction action) {
            this.state = current; 
            action.Accept(this);
            return this.state;
        }


        public void visit(StartTutorial startTutorial) {
            this.state.stage = TutorialState.TutorialStage.Welcome;
        }

        public void visit(StartMoving startMoving) {
            this.state.stage = TutorialState.TutorialStage.Moving;
        }

        public void visit(ShouldOpenInventory shouldOpenInventory) {
            this.state.stage = TutorialState.TutorialStage.ShouldOpenInventory;
        }

        public void visit(InsideInventory insideInventory) {
            this.state.stage = TutorialState.TutorialStage.InsideInventory;
        }

        public void visit(DidMoveInventoryItem didMoveInventoryItem) {
            this.state.stage = TutorialState.TutorialStage.DidMoveInventoryItem;
        }
    }
}