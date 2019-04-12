using System.Collections.Generic;
using Model.Action;
using Model.State;
using NUnit.Framework;
using UnityEngine;

namespace Tests {
    public class UIStateTests {
        private GameManager gameManager;

        [SetUp]
        public void Setup() {
            this.gameManager = GameManager.Instance();
            gameManager.ResetGame();
        }
        
          [Test]
        public void TestGameStartsInCorrectUIState() {
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.Login));
        }

        [Test]
        public void TestOpenPlayingUI() {
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.Playing));
        }

        [Test]
        public void TestOpenInventoryUI() {
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            gameManager.uiStore.Dispatch(new OpenInventoryUI());
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.Inventory));
        }

        [Test]
        public void TestCloseInventoryUI() {
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            gameManager.uiStore.Dispatch(new OpenInventoryUI());
            gameManager.uiStore.Dispatch(new CloseUI());
            gameManager.uiStore.Dispatch(new OpenInventoryUI());
            gameManager.uiStore.Dispatch(new CloseUI());
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.Playing));
        }

        [Test]
        public void TestOpenBlueprintUI() {
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            gameManager.uiStore.Dispatch(new OpenBlueprintUI());
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.Blueprint));
        }

        [Test]
        public void TestCloseBlueprintUI() {
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            gameManager.uiStore.Dispatch(new OpenBlueprintUI());
            gameManager.uiStore.Dispatch(new CloseUI());
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.Playing));
        }

        [Test]
        public void TestOpenMachineUI() {
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            gameManager.uiStore.Dispatch(new OpenMachineUI(new Vector2(0, 0)));
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.Machine));
            Assert.That(gameManager.uiStore.GetState().SelectedMachineLocation, Is.EqualTo(new Vector2(0, 0)));
        }

        [Test]
        public void TestCloseMachineUI() {
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            gameManager.uiStore.Dispatch(new OpenMachineUI(new Vector2(0, 0)));
            gameManager.uiStore.Dispatch(new CloseUI());
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.Playing));
        }

        [Test]
        public void TestOpenPlaySettingsUI() {
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            gameManager.uiStore.Dispatch(new OpenSettingsUI());
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.Pause));
        }

        [Test]
        public void TestClosePlaySettingsUI() {
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            gameManager.uiStore.Dispatch(new OpenSettingsUI());
            gameManager.uiStore.Dispatch(new CloseUI());
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.Playing));
        }

        [Test]
        public void TestOpenInvSettingsUI() {
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            gameManager.uiStore.Dispatch(new OpenInventoryUI());
            gameManager.uiStore.Dispatch(new OpenSettingsUI());
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.InvPause));
        }

        [Test]
        public void TestCloseInvSettingsUI() {
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            gameManager.uiStore.Dispatch(new OpenInventoryUI());
            gameManager.uiStore.Dispatch(new OpenSettingsUI());
            gameManager.uiStore.Dispatch(new CloseUI());
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.Inventory));
        }

        [Test]
        public void TestOpenBlueSettingsUI() {
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            gameManager.uiStore.Dispatch(new OpenBlueprintUI());
            gameManager.uiStore.Dispatch(new OpenSettingsUI());
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.BluePause));
        }

        [Test]
        public void TestCloseBlueSettingsUI() {
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            gameManager.uiStore.Dispatch(new OpenBlueprintUI());
            gameManager.uiStore.Dispatch(new OpenSettingsUI());
            gameManager.uiStore.Dispatch(new CloseUI());
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.Blueprint));
        }

        [Test]
        public void TestOpenMachSettingsUI() {
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            gameManager.uiStore.Dispatch(new OpenMachineUI(new Vector2(0, 0)));
            gameManager.uiStore.Dispatch(new OpenSettingsUI());
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.MachPause));
        }

        [Test]
        public void TestCloseMachSettingsUI() {
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            gameManager.uiStore.Dispatch(new OpenMachineUI(new Vector2(0, 0)));
            gameManager.uiStore.Dispatch(new OpenSettingsUI());
            gameManager.uiStore.Dispatch(new CloseUI());
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.Machine));
        }

        [Test]
        public void TestOpenLogoutUI() {
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            gameManager.uiStore.Dispatch(new OpenSettingsUI());
            gameManager.uiStore.Dispatch(new Logout());
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.Logout));
        }

        [Test]
        public void TestCloseLogoutUI() {
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            gameManager.uiStore.Dispatch(new OpenSettingsUI());
            gameManager.uiStore.Dispatch(new Logout());
            gameManager.uiStore.Dispatch(new CloseUI());
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.Pause));
        }

        [Test]
        public void TestOpenInvLogoutUI() {
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            gameManager.uiStore.Dispatch(new OpenInventoryUI());
            gameManager.uiStore.Dispatch(new OpenSettingsUI());
            gameManager.uiStore.Dispatch(new Logout());
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.InvLogout));
        }

        [Test]
        public void TestCloseInvLogoutUI() {
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            gameManager.uiStore.Dispatch(new OpenInventoryUI());
            gameManager.uiStore.Dispatch(new OpenSettingsUI());
            gameManager.uiStore.Dispatch(new Logout());
            gameManager.uiStore.Dispatch(new CloseUI());
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.InvPause));
        }

        [Test]
        public void TestOpenBlueLogoutUI() {
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            gameManager.uiStore.Dispatch(new OpenBlueprintUI());
            gameManager.uiStore.Dispatch(new OpenSettingsUI());
            gameManager.uiStore.Dispatch(new Logout());
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.BlueLogout));
        }
       
        [Test]
        public void TestCloseBlueLogoutUI() {
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            gameManager.uiStore.Dispatch(new OpenBlueprintUI());
            gameManager.uiStore.Dispatch(new OpenSettingsUI());
            gameManager.uiStore.Dispatch(new Logout());
            gameManager.uiStore.Dispatch(new CloseUI());
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.BluePause));
        }

        [Test]
        public void TestOpenMachLogoutUI() {
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            gameManager.uiStore.Dispatch(new OpenMachineUI(new Vector2(0, 0)));
            gameManager.uiStore.Dispatch(new OpenSettingsUI());
            gameManager.uiStore.Dispatch(new Logout());
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.MachLogout));
        }

        [Test]
        public void TestCloseMachLogoutUI() {
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            gameManager.uiStore.Dispatch(new OpenMachineUI(new Vector2(0,0)));
            gameManager.uiStore.Dispatch(new OpenSettingsUI());
            gameManager.uiStore.Dispatch(new Logout());
            gameManager.uiStore.Dispatch(new CloseUI());
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.MachPause));
        }

        [Test]
        public void TestOpenExitUI() {
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            gameManager.uiStore.Dispatch(new OpenSettingsUI());
            gameManager.uiStore.Dispatch(new Exit());
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.Exit));
        }

        [Test]
        public void TestCloseExitUI() {
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            gameManager.uiStore.Dispatch(new OpenSettingsUI());
            gameManager.uiStore.Dispatch(new Exit());
            gameManager.uiStore.Dispatch(new CloseUI());
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.Pause));
        }

        [Test]
        public void TestOpenInvExitUI() {
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            gameManager.uiStore.Dispatch(new OpenInventoryUI());
            gameManager.uiStore.Dispatch(new OpenSettingsUI());
            gameManager.uiStore.Dispatch(new Exit());
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.InvExit));
        }

        [Test]
        public void TestCloseInvExitUI() {
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            gameManager.uiStore.Dispatch(new OpenInventoryUI());
            gameManager.uiStore.Dispatch(new OpenSettingsUI());
            gameManager.uiStore.Dispatch(new Exit());
            gameManager.uiStore.Dispatch(new CloseUI());
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.InvPause));
        }

        [Test]
        public void TestOpenBlueExitUI() {
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            gameManager.uiStore.Dispatch(new OpenBlueprintUI());
            gameManager.uiStore.Dispatch(new OpenSettingsUI());
            gameManager.uiStore.Dispatch(new Exit());
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.BlueExit));
        }

        [Test]
        public void TestCloseBlueExitUI() {
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            gameManager.uiStore.Dispatch(new OpenBlueprintUI());
            gameManager.uiStore.Dispatch(new OpenSettingsUI());
            gameManager.uiStore.Dispatch(new Exit());
            gameManager.uiStore.Dispatch(new CloseUI());
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.BluePause));
        }

        [Test]
        public void TestOpenMachExitUI() {
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            gameManager.uiStore.Dispatch(new OpenMachineUI(new Vector2(0, 0)));
            gameManager.uiStore.Dispatch(new OpenSettingsUI());
            gameManager.uiStore.Dispatch(new Exit());
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.MachExit));
        }

        [Test]
        public void TestCloseMachExitUI() {
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            gameManager.uiStore.Dispatch(new OpenMachineUI(new Vector2(0, 0)));
            gameManager.uiStore.Dispatch(new OpenSettingsUI());
            gameManager.uiStore.Dispatch(new Exit());
            gameManager.uiStore.Dispatch(new CloseUI());
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.MachPause));
        }

        [Test]
        public void TestMachExitUIToMachineUI() {
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            gameManager.uiStore.Dispatch(new OpenMachineUI(new Vector2(0, 0)));
            gameManager.uiStore.Dispatch(new OpenSettingsUI());
            gameManager.uiStore.Dispatch(new Exit());
            gameManager.uiStore.Dispatch(new CloseUI());
            gameManager.uiStore.Dispatch(new CloseUI());
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.Machine));
        }

        [Test]
        public void TestExitUIToPlayingToInvLogoutUI() {
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            gameManager.uiStore.Dispatch(new OpenSettingsUI());
            gameManager.uiStore.Dispatch(new Exit());
            gameManager.uiStore.Dispatch(new CloseUI());
            gameManager.uiStore.Dispatch(new CloseUI());
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.Playing));
            gameManager.uiStore.Dispatch(new OpenInventoryUI());
            gameManager.uiStore.Dispatch(new OpenSettingsUI());
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.InvPause));
            gameManager.uiStore.Dispatch(new Logout());
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.InvLogout));
        }

        [Test]
        public void TestLogout() {
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            gameManager.uiStore.Dispatch(new OpenSettingsUI());
            gameManager.uiStore.Dispatch(new Logout());
            gameManager.uiStore.Dispatch(new OpenLoginUI());
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.Login));
        }

        [Test]
        public void TestLogoutLogin() {
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            gameManager.uiStore.Dispatch(new OpenSettingsUI());
            gameManager.uiStore.Dispatch(new Logout());
            gameManager.uiStore.Dispatch(new OpenLoginUI());
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            gameManager.uiStore.Dispatch(new OpenSettingsUI());
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.Pause));
        }

        [Test]
        public void TestLoginAndBindings() {
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            gameManager.uiStore.Dispatch(new OpenBindingsUI());
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.Bindings));
            gameManager.uiStore.Dispatch(new CloseUI());
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.Playing));
        }

        [Test]
        public void TestPauseAndNotBindings() {
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            gameManager.uiStore.Dispatch(new OpenSettingsUI());
            gameManager.uiStore.Dispatch(new OpenBindingsUI());
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.Pause));
            gameManager.uiStore.Dispatch(new CloseUI());
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.Playing));
        }

        [Test]
        public void TestGameManagerExists() {
            Assert.NotNull(gameManager);
        }


    }
}