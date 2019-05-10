using System;
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
        public void TestGameManagerExists() {
            Assert.NotNull(gameManager);
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
        public void TestOpenGoalUI() {
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            gameManager.uiStore.Dispatch(new OpenGoalUI());
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.Goal));
        }

        [Test]
        public void TestCloseGoalUI() {
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            gameManager.uiStore.Dispatch(new OpenGoalUI());
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
            try {
                gameManager.uiStore.Dispatch(new OpenSettingsUI());
                Assert.Fail("Exception wasn't thrown");
            } catch {
                Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.Inventory));
            }
        }

        [Test]
        public void TestOpenBlueSettingsUI() {
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            gameManager.uiStore.Dispatch(new OpenBlueprintUI());
            try {
                gameManager.uiStore.Dispatch(new OpenSettingsUI());
                Assert.Fail("Exception wasn't thrown");
            } catch {
                Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.Blueprint));
            }
        }

        [Test]
        public void TestOpenMachSettingsUI() {
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            gameManager.uiStore.Dispatch(new OpenMachineUI(new Vector2(0, 0)));
            try {
                gameManager.uiStore.Dispatch(new OpenSettingsUI());
                Assert.Fail("Exception wasn't thrown");
            } catch {
                Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.Machine));
            }
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
        public void TestOpenExitUI() {
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            gameManager.uiStore.Dispatch(new OpenSettingsUI());
            gameManager.uiStore.Dispatch(new Exit());
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.Exit));
        }

        [Test]
        public void TestOpenGateUI() {
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            gameManager.uiStore.Dispatch(new OpenMouseUI());
            gameManager.uiStore.Dispatch(new OpenGateUI());
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.Gate));
        }

        [Test]
        public void TestCloseGateUI() {
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            gameManager.uiStore.Dispatch(new OpenMouseUI());
            gameManager.uiStore.Dispatch(new OpenGateUI());
            gameManager.uiStore.Dispatch(new CloseUI());
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.Mouse));
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
        public void TestMachineToSettings() {
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            gameManager.uiStore.Dispatch(new OpenMachineUI(new Vector2(0, 0)));
            gameManager.uiStore.Dispatch(new CloseUI());
            gameManager.uiStore.Dispatch(new OpenSettingsUI());
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.Pause));
            gameManager.uiStore.Dispatch(new Exit());
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.Exit));
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
    }
}
