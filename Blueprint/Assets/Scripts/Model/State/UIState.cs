using System.Threading;
using UnityEngine;

namespace Model.State {
    public class UIState {
        public enum OpenUI {
            Welcome,
            Login,
            Playing,
            Inventory,
            Blueprint,
            Machine,
            Pause,
            Exit,
            Logout,
            Bindings,
            Gate,
            Mouse,
            Intro,
        };

        public OpenUI Selected;
        public Vector2 SelectedMachineLocation;
        public bool IntroComplete;

        public UIState() {
            this.Selected = OpenUI.Playing;
            this.IntroComplete = false;
        }
    }
}
