namespace Model.Action {
    public interface TutorialVisitor {
        void visit(StartTutorial startTutorial);
        void visit(StartMoving startMoving);
        void visit(ShouldOpenInventory shouldOpenInventory);
        void visit(InsideInventory insideInventory);
        void visit(DidMoveInventoryItem didMoveInventoryItem);
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

}