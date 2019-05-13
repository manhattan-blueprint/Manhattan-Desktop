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
}