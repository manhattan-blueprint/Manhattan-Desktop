using System;
using System.Collections.Generic;

public enum GoalPosition {
    Top,
    Mid,
    Bot
}

namespace Model {
    [Serializable]
    public class Goal {
        public bool topInput;
        public bool midInput;
        public bool botInput;

        public Goal() {
            this.topInput = false;
            this.midInput = false;
            this.botInput = false;
        }

        public bool IsComplete() {
            return (topInput && midInput && botInput);
        }
    }
}