namespace Model.Action {
    public interface TutorialVisitor {
        void visit(StartTutorial startTutorial);
        void visit(StartMoving startMoving);
        void visit(ShouldOpenInventory shouldOpenInventory);
        void visit(InsideInventory insideInventory);
        void visit(DidMoveInventoryItem didMoveInventoryItem);
        void visit(DidSplitInventoryItem didSplitInventoryItem);
        void visit(DidCollectFromBackpack didCollectFromBackpack);
        void visit(ShouldCloseInventory shouldCloseInventory);
        void visit(ClosedInventory closedInventory);
        void visit(ShouldOpenBlueprint shouldOpenBlueprint);
        void visit(InsideBlueprint insideBlueprint);
        void visit(HighlightFurnace highlightFurnace);
        void visit(OpenFurnaceBlueprint openFurnaceBlueprint);
        void visit(CraftedFurnace craftedFurnace);
        void visit(HighlightBlueprintNotes highlightNotes);
        void visit(ReturnToProgression returnToProgression);
        void visit(ShouldCloseBlueprintTemplate shouldCloseBlueprintTemplate);
        void visit(ShouldCloseBlueprint shouldCloseBlueprint);
        void visit(ClosedBlueprint closedBlueprint);
        void visit(ShowHeldItemScroll showHeldItemScroll);
        void visit(DidScrollHeldItem didScrollHeldItem);
        void visit(DidPlaceFurnace didPlaceFurnace);
    }

    public abstract class TutorialAction: Action {
        public abstract void Accept(TutorialVisitor visitor);
    }
    
    public class StartTutorial: TutorialAction {
        public override void Accept(TutorialVisitor visitor) {
            visitor.visit(this);
        }
    }

    public class StartMoving : TutorialAction {
        public override void Accept(TutorialVisitor visitor) {
            visitor.visit(this);
        }
    }

    public class ShouldOpenInventory : TutorialAction {
        public override void Accept(TutorialVisitor visitor) {
            visitor.visit(this);
        }
    }

    public class InsideInventory: TutorialAction {
        public override void Accept(TutorialVisitor visitor) {
            visitor.visit(this);
        }
    }

    public class DidMoveInventoryItem : TutorialAction {
        public override void Accept(TutorialVisitor visitor) {
            visitor.visit(this);
        }
    }

    public class DidSplitInventoryItem : TutorialAction {
        public override void Accept(TutorialVisitor visitor) {
            visitor.visit(this);
        }
    }

    public class DidCollectFromBackpack : TutorialAction {
        public override void Accept(TutorialVisitor visitor) {
            visitor.visit(this);
        }
    }

    public class ShouldCloseInventory : TutorialAction {
        public override void Accept(TutorialVisitor visitor) {
            visitor.visit(this);
        }
    }

    public class ClosedInventory : TutorialAction {
        public override void Accept(TutorialVisitor visitor) {
            visitor.visit(this);
        }
    }

    public class ShouldOpenBlueprint : TutorialAction {
        public override void Accept(TutorialVisitor visitor) {
            visitor.visit(this);
        }
    }

    public class InsideBlueprint: TutorialAction {
        public override void Accept(TutorialVisitor visitor) {
            visitor.visit(this);
        }
    }

    public class HighlightFurnace: TutorialAction {
        public override void Accept(TutorialVisitor visitor) {
            visitor.visit(this);
        }
    }

    public class OpenFurnaceBlueprint: TutorialAction {
        public override void Accept(TutorialVisitor visitor) {
            visitor.visit(this);
        }
    }

    public class CraftedFurnace: TutorialAction {
        public override void Accept(TutorialVisitor visitor) {
            visitor.visit(this);
        }
    }

    public class HighlightBlueprintNotes: TutorialAction {
        public override void Accept(TutorialVisitor visitor) {
            visitor.visit(this);
        }
    }

    public class ReturnToProgression: TutorialAction {
        public override void Accept(TutorialVisitor visitor) {
            visitor.visit(this);
        }
    }

    public class ShouldCloseBlueprintTemplate : TutorialAction {
        public override void Accept(TutorialVisitor visitor) {
            visitor.visit(this);
        }
    }

    public class ShouldCloseBlueprint : TutorialAction {
        public override void Accept(TutorialVisitor visitor) {
            visitor.visit(this);
        }
    }

    public class ClosedBlueprint: TutorialAction {
        public override void Accept(TutorialVisitor visitor) {
            visitor.visit(this);
        }
    }
    
    public class ShowHeldItemScroll: TutorialAction {
        public override void Accept(TutorialVisitor visitor) {
            visitor.visit(this);
        }
    }

    public class DidScrollHeldItem: TutorialAction {
        public override void Accept(TutorialVisitor visitor) {
            visitor.visit(this);
        }
    }

    public class DidPlaceFurnace : TutorialAction {
        public override void Accept(TutorialVisitor visitor) {
            visitor.visit(this);
        }
    }
}