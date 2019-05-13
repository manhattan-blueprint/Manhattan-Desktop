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
        
        public void visit(DidSplitInventoryItem didSplitInventoryItem) {
            this.state.stage = TutorialState.TutorialStage.DidSplitInventoryItem;
        }
        
        public void visit(DidCollectFromBackpack didCollectFromBackpack) {
            this.state.stage = TutorialState.TutorialStage.DidCollectFromBackpack;
        }
        
        public void visit(ShouldCloseInventory shouldCloseInventory) {
            this.state.stage = TutorialState.TutorialStage.ShouldCloseInventory;
        }
        
        public void visit(ClosedInventory closedInventory) {
            this.state.stage = TutorialState.TutorialStage.ClosedInventory;
        }
        
        public void visit(ShouldOpenBlueprint shouldOpenBlueprint) {
            this.state.stage = TutorialState.TutorialStage.ShouldOpenBlueprint;
        }
        
        public void visit(InsideBlueprint insideBlueprint) {
            this.state.stage = TutorialState.TutorialStage.InsideBlueprint;
        }

        public void visit(HighlightFurnace highlightFurnace) {
            this.state.stage = TutorialState.TutorialStage.HighlightFurnace;
        }

        public void visit(OpenFurnaceBlueprint openFurnaceBlueprint) {
            this.state.stage = TutorialState.TutorialStage.OpenFurnace;
        }
        
        public void visit(CraftedFurnace craftedFurnace) {
            this.state.stage = TutorialState.TutorialStage.CraftedFurnace;
        }
        
        public void visit(HighlightBlueprintNotes highlightBlueprintNotes) {
            this.state.stage = TutorialState.TutorialStage.HighlightBlueprintNotes;
        }
        
        public void visit(ReturnToProgression returnToProgression) {
            this.state.stage = TutorialState.TutorialStage.ReturnToProgression;
        }

        public void visit(ShouldCloseBlueprintTemplate shouldCloseBlueprintTemplate) {
            this.state.stage = TutorialState.TutorialStage.ShouldCloseBlueprintTemplate;
        }
        
        public void visit(ShouldCloseBlueprint shouldCloseBlueprint) {
            this.state.stage = TutorialState.TutorialStage.ShouldCloseBlueprint;
        }

        public void visit(ClosedBlueprint closedBlueprint) {
            this.state.stage = TutorialState.TutorialStage.ClosedBlueprint;
        }

        public void visit(ShowHeldItemScroll showHeldItemScroll) {
            this.state.stage = TutorialState.TutorialStage.ShowHeldItemScroll;
        }
        
        public void visit(DidScrollHeldItem didScrollHeldItem) {
            this.state.stage = TutorialState.TutorialStage.ScrolledHeldItem;
        }

        public void visit(DidPlaceFurnace didPlaceFurnace) {
            this.state.stage = TutorialState.TutorialStage.PlacedFurnace;
        }
    }
}