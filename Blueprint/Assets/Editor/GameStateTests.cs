using Model;
using Model.Action;
using NUnit.Framework;

namespace Tests {
    public class GameStateTests {
        private GameManager gameManager;

        [SetUp]
        public void Setup() {
            this.gameManager = GameManager.Instance();
            gameManager.ResetGame();
        }

        [Test]
        public void TestGameStartsInCorrectUIState() {
            Assert.That(gameManager.store.GetState().uiState.Selected, Is.EqualTo(UIState.OpenUI.Login));
        }

        [Test]
        public void TestOpenPlayingUI() {
            gameManager.store.Dispatch(new OpenPlayingUI());
            Assert.That(gameManager.store.GetState().uiState.Selected, Is.EqualTo(UIState.OpenUI.Playing));
        }

        [Test]
        public void TestOpenInventoryUI() {
            gameManager.store.Dispatch(new OpenPlayingUI());
            gameManager.store.Dispatch(new OpenInventoryUI());
            Assert.That(gameManager.store.GetState().uiState.Selected, Is.EqualTo(UIState.OpenUI.Inventory));
        }

        [Test]
        public void TestCloseInventoryUI() {
            gameManager.store.Dispatch(new OpenPlayingUI());
            gameManager.store.Dispatch(new OpenInventoryUI());
            gameManager.store.Dispatch(new CloseUI());
            gameManager.store.Dispatch(new OpenInventoryUI());
            gameManager.store.Dispatch(new CloseUI());
            Assert.That(gameManager.store.GetState().uiState.Selected, Is.EqualTo(UIState.OpenUI.Playing));
        }

        [Test]
        public void TestOpenBlueprintUI() {
            gameManager.store.Dispatch(new OpenPlayingUI());
            gameManager.store.Dispatch(new OpenBlueprintUI());
            Assert.That(gameManager.store.GetState().uiState.Selected, Is.EqualTo(UIState.OpenUI.Blueprint));
        }

        [Test]
        public void TestCloseBlueprintUI() {
            gameManager.store.Dispatch(new OpenPlayingUI());
            gameManager.store.Dispatch(new OpenBlueprintUI());
            gameManager.store.Dispatch(new CloseUI());
            Assert.That(gameManager.store.GetState().uiState.Selected, Is.EqualTo(UIState.OpenUI.Playing));
        }

        [Test]
        public void TestOpenMachineUI() {
            gameManager.store.Dispatch(new OpenPlayingUI());
            gameManager.store.Dispatch(new OpenMachineUI());
            Assert.That(gameManager.store.GetState().uiState.Selected, Is.EqualTo(UIState.OpenUI.Machine));
        }

        [Test]
        public void TestCloseMachineUI() {
            gameManager.store.Dispatch(new OpenPlayingUI());
            gameManager.store.Dispatch(new OpenMachineUI());
            gameManager.store.Dispatch(new CloseUI());
            Assert.That(gameManager.store.GetState().uiState.Selected, Is.EqualTo(UIState.OpenUI.Playing));
        }

        [Test]
        public void TestOpenPlaySettingsUI() {
            gameManager.store.Dispatch(new OpenPlayingUI());
            gameManager.store.Dispatch(new OpenSettingsUI());
            Assert.That(gameManager.store.GetState().uiState.Selected, Is.EqualTo(UIState.OpenUI.Pause));
        }

        [Test]
        public void TestClosePlaySettingsUI() {
            gameManager.store.Dispatch(new OpenPlayingUI());
            gameManager.store.Dispatch(new OpenSettingsUI());
            gameManager.store.Dispatch(new CloseUI());
            Assert.That(gameManager.store.GetState().uiState.Selected, Is.EqualTo(UIState.OpenUI.Playing));
        }

        [Test]
        public void TestOpenInvSettingsUI() {
            gameManager.store.Dispatch(new OpenPlayingUI());
            gameManager.store.Dispatch(new OpenInventoryUI());
            gameManager.store.Dispatch(new OpenSettingsUI());
            Assert.That(gameManager.store.GetState().uiState.Selected, Is.EqualTo(UIState.OpenUI.InvPause));
        }

        [Test]
        public void TestCloseInvSettingsUI() {
            gameManager.store.Dispatch(new OpenPlayingUI());
            gameManager.store.Dispatch(new OpenInventoryUI());
            gameManager.store.Dispatch(new OpenSettingsUI());
            gameManager.store.Dispatch(new CloseUI());
            Assert.That(gameManager.store.GetState().uiState.Selected, Is.EqualTo(UIState.OpenUI.Inventory));
        }

        [Test]
        public void TestOpenBlueSettingsUI() {
            gameManager.store.Dispatch(new OpenPlayingUI());
            gameManager.store.Dispatch(new OpenBlueprintUI());
            gameManager.store.Dispatch(new OpenSettingsUI());
            Assert.That(gameManager.store.GetState().uiState.Selected, Is.EqualTo(UIState.OpenUI.BluePause));
        }

        [Test]
        public void TestCloseBlueSettingsUI() {
            gameManager.store.Dispatch(new OpenPlayingUI());
            gameManager.store.Dispatch(new OpenBlueprintUI());
            gameManager.store.Dispatch(new OpenSettingsUI());
            gameManager.store.Dispatch(new CloseUI());
            Assert.That(gameManager.store.GetState().uiState.Selected, Is.EqualTo(UIState.OpenUI.Blueprint));
        }

        [Test]
        public void TestOpenMachSettingsUI() {
            gameManager.store.Dispatch(new OpenPlayingUI());
            gameManager.store.Dispatch(new OpenMachineUI());
            gameManager.store.Dispatch(new OpenSettingsUI());
            Assert.That(gameManager.store.GetState().uiState.Selected, Is.EqualTo(UIState.OpenUI.MachPause));
        }

        [Test]
        public void TestCloseMachSettingsUI() {
            gameManager.store.Dispatch(new OpenPlayingUI());
            gameManager.store.Dispatch(new OpenMachineUI());
            gameManager.store.Dispatch(new OpenSettingsUI());
            gameManager.store.Dispatch(new CloseUI());
            Assert.That(gameManager.store.GetState().uiState.Selected, Is.EqualTo(UIState.OpenUI.Machine));
        }

        [Test]
        public void TestOpenLogoutUI() {
            gameManager.store.Dispatch(new OpenPlayingUI());
            gameManager.store.Dispatch(new OpenSettingsUI());
            gameManager.store.Dispatch(new Logout());
            Assert.That(gameManager.store.GetState().uiState.Selected, Is.EqualTo(UIState.OpenUI.Logout));
        }

        [Test]
        public void TestCloseLogoutUI() {
            gameManager.store.Dispatch(new OpenPlayingUI());
            gameManager.store.Dispatch(new OpenSettingsUI());
            gameManager.store.Dispatch(new Logout());
            gameManager.store.Dispatch(new CloseUI());
            Assert.That(gameManager.store.GetState().uiState.Selected, Is.EqualTo(UIState.OpenUI.Pause));
        }

        [Test]
        public void TestOpenInvLogoutUI() {
            gameManager.store.Dispatch(new OpenPlayingUI());
            gameManager.store.Dispatch(new OpenInventoryUI());
            gameManager.store.Dispatch(new OpenSettingsUI());
            gameManager.store.Dispatch(new Logout());
            Assert.That(gameManager.store.GetState().uiState.Selected, Is.EqualTo(UIState.OpenUI.InvLogout));
        }

        [Test]
        public void TestCloseInvLogoutUI() {
            gameManager.store.Dispatch(new OpenPlayingUI());
            gameManager.store.Dispatch(new OpenInventoryUI());
            gameManager.store.Dispatch(new OpenSettingsUI());
            gameManager.store.Dispatch(new Logout());
            gameManager.store.Dispatch(new CloseUI());
            Assert.That(gameManager.store.GetState().uiState.Selected, Is.EqualTo(UIState.OpenUI.InvPause));
        }

        [Test]
        public void TestOpenBlueLogoutUI() {
            gameManager.store.Dispatch(new OpenPlayingUI());
            gameManager.store.Dispatch(new OpenBlueprintUI());
            gameManager.store.Dispatch(new OpenSettingsUI());
            gameManager.store.Dispatch(new Logout());
            Assert.That(gameManager.store.GetState().uiState.Selected, Is.EqualTo(UIState.OpenUI.BlueLogout));
        }

        [Test]
        public void TestCloseBlueLogoutUI() {
            gameManager.store.Dispatch(new OpenPlayingUI());
            gameManager.store.Dispatch(new OpenBlueprintUI());
            gameManager.store.Dispatch(new OpenSettingsUI());
            gameManager.store.Dispatch(new Logout());
            gameManager.store.Dispatch(new CloseUI());
            Assert.That(gameManager.store.GetState().uiState.Selected, Is.EqualTo(UIState.OpenUI.BluePause));
        }

        [Test]
        public void TestOpenMachLogoutUI() {
            gameManager.store.Dispatch(new OpenPlayingUI());
            gameManager.store.Dispatch(new OpenMachineUI());
            gameManager.store.Dispatch(new OpenSettingsUI());
            gameManager.store.Dispatch(new Logout());
            Assert.That(gameManager.store.GetState().uiState.Selected, Is.EqualTo(UIState.OpenUI.MachLogout));
        }

        [Test]
        public void TestCloseMachLogoutUI() {
            gameManager.store.Dispatch(new OpenPlayingUI());
            gameManager.store.Dispatch(new OpenMachineUI());
            gameManager.store.Dispatch(new OpenSettingsUI());
            gameManager.store.Dispatch(new Logout());
            gameManager.store.Dispatch(new CloseUI());
            Assert.That(gameManager.store.GetState().uiState.Selected, Is.EqualTo(UIState.OpenUI.MachPause));
        }

        [Test]
        public void TestOpenExitUI() {
            gameManager.store.Dispatch(new OpenPlayingUI());
            gameManager.store.Dispatch(new OpenSettingsUI());
            gameManager.store.Dispatch(new Exit());
            Assert.That(gameManager.store.GetState().uiState.Selected, Is.EqualTo(UIState.OpenUI.Exit));
        }

        [Test]
        public void TestCloseExitUI() {
            gameManager.store.Dispatch(new OpenPlayingUI());
            gameManager.store.Dispatch(new OpenSettingsUI());
            gameManager.store.Dispatch(new Exit());
            gameManager.store.Dispatch(new CloseUI());
            Assert.That(gameManager.store.GetState().uiState.Selected, Is.EqualTo(UIState.OpenUI.Pause));
        }

        [Test]
        public void TestOpenInvExitUI() {
            gameManager.store.Dispatch(new OpenPlayingUI());
            gameManager.store.Dispatch(new OpenInventoryUI());
            gameManager.store.Dispatch(new OpenSettingsUI());
            gameManager.store.Dispatch(new Exit());
            Assert.That(gameManager.store.GetState().uiState.Selected, Is.EqualTo(UIState.OpenUI.InvExit));
        }

        [Test]
        public void TestCloseInvExitUI() {
            gameManager.store.Dispatch(new OpenPlayingUI());
            gameManager.store.Dispatch(new OpenInventoryUI());
            gameManager.store.Dispatch(new OpenSettingsUI());
            gameManager.store.Dispatch(new Exit());
            gameManager.store.Dispatch(new CloseUI());
            Assert.That(gameManager.store.GetState().uiState.Selected, Is.EqualTo(UIState.OpenUI.InvPause));
        }

        [Test]
        public void TestOpenBlueExitUI() {
            gameManager.store.Dispatch(new OpenPlayingUI());
            gameManager.store.Dispatch(new OpenBlueprintUI());
            gameManager.store.Dispatch(new OpenSettingsUI());
            gameManager.store.Dispatch(new Exit());
            Assert.That(gameManager.store.GetState().uiState.Selected, Is.EqualTo(UIState.OpenUI.BlueExit));
        }

        [Test]
        public void TestCloseBlueExitUI() {
            gameManager.store.Dispatch(new OpenPlayingUI());
            gameManager.store.Dispatch(new OpenBlueprintUI());
            gameManager.store.Dispatch(new OpenSettingsUI());
            gameManager.store.Dispatch(new Exit());
            gameManager.store.Dispatch(new CloseUI());
            Assert.That(gameManager.store.GetState().uiState.Selected, Is.EqualTo(UIState.OpenUI.BluePause));
        }

        [Test]
        public void TestOpenMachExitUI() {
            gameManager.store.Dispatch(new OpenPlayingUI());
            gameManager.store.Dispatch(new OpenMachineUI());
            gameManager.store.Dispatch(new OpenSettingsUI());
            gameManager.store.Dispatch(new Exit());
            Assert.That(gameManager.store.GetState().uiState.Selected, Is.EqualTo(UIState.OpenUI.MachExit));
        }

        [Test]
        public void TestCloseMachExitUI() {
            gameManager.store.Dispatch(new OpenPlayingUI());
            gameManager.store.Dispatch(new OpenMachineUI());
            gameManager.store.Dispatch(new OpenSettingsUI());
            gameManager.store.Dispatch(new Exit());
            gameManager.store.Dispatch(new CloseUI());
            Assert.That(gameManager.store.GetState().uiState.Selected, Is.EqualTo(UIState.OpenUI.MachPause));
        }

        [Test]
        public void TestMachExitUIToMachineUI() {
            gameManager.store.Dispatch(new OpenPlayingUI());
            gameManager.store.Dispatch(new OpenMachineUI());
            gameManager.store.Dispatch(new OpenSettingsUI());
            gameManager.store.Dispatch(new Exit());
            gameManager.store.Dispatch(new CloseUI());
            gameManager.store.Dispatch(new CloseUI());
            Assert.That(gameManager.store.GetState().uiState.Selected, Is.EqualTo(UIState.OpenUI.Machine));
        }

        [Test]
        public void TestExitUIToPlayingToInvLogoutUI() {
            gameManager.store.Dispatch(new OpenPlayingUI());
            gameManager.store.Dispatch(new OpenSettingsUI());
            gameManager.store.Dispatch(new Exit());
            gameManager.store.Dispatch(new CloseUI());
            gameManager.store.Dispatch(new CloseUI());
            Assert.That(gameManager.store.GetState().uiState.Selected, Is.EqualTo(UIState.OpenUI.Playing));
            gameManager.store.Dispatch(new OpenInventoryUI());
            gameManager.store.Dispatch(new OpenSettingsUI());
            Assert.That(gameManager.store.GetState().uiState.Selected, Is.EqualTo(UIState.OpenUI.InvPause));
            gameManager.store.Dispatch(new Logout());
            Assert.That(gameManager.store.GetState().uiState.Selected, Is.EqualTo(UIState.OpenUI.InvLogout));
        }

        [Test]
        public void TestLogout() {
            gameManager.store.Dispatch(new OpenPlayingUI());
            gameManager.store.Dispatch(new OpenSettingsUI());
            gameManager.store.Dispatch(new Logout());
            gameManager.store.Dispatch(new OpenLoginUI());
            Assert.That(gameManager.store.GetState().uiState.Selected, Is.EqualTo(UIState.OpenUI.Login));
        }

        [Test]
        public void TestGameManagerExists() {
            Assert.NotNull(gameManager);
        }

        // Inventory Actions
        [Test]
        public void TestAddToInventory() {
            // Add an item and validate it updates the state
            gameManager.store.Dispatch(new OpenPlayingUI());
            this.gameManager.store.Dispatch(new AddItemToInventory(1, 1, "wood"));

            foreach (InventoryItem item in this.gameManager.store.GetState().inventoryState.inventoryContents) {
                //Success case
                if (item.GetId() == 1) {
                    Assert.AreEqual(1, item.GetQuantity());
                    Assert.AreEqual("wood", item.GetName());
                    return;
                }
            }
            Assert.Fail();
        }

        [Test]
        public void TestAddToInventoryTwice() {
            // Assert empty to begin with
            if (this.gameManager.store.GetState().inventoryState.inventoryContents == null || this.gameManager.store.GetState().inventoryState.inventoryContents.Length == 0) {
                Assert.Fail();
            }

            // Add an item and validate it updates the state
            this.gameManager.store.Dispatch(new AddItemToInventory(1, 1, "wood"));
            this.gameManager.store.Dispatch(new AddItemToInventory(1, 10, "wood"));

            foreach (InventoryItem item in this.gameManager.store.GetState().inventoryState.inventoryContents) {
                //Success case
                if (item.GetId() == 1) {
                    Assert.AreEqual(11, item.GetQuantity());
                    Assert.AreEqual("wood", item.GetName());
                    return;
                }
            }
            Assert.Fail();
        }

        [Test]
        public void TestRemoveFromInventory() {
            // Assert empty to begin with
            if (this.gameManager.store.GetState().inventoryState.inventoryContents == null || this.gameManager.store.GetState().inventoryState.inventoryContents.Length == 0) {
                Assert.Fail();
            }

            // Add an item and validate it updates the state
            this.gameManager.store.Dispatch(new AddItemToInventory(1, 10, "wood"));
            this.gameManager.store.Dispatch(new RemoveItemFromInventory(1, 4));

            foreach (InventoryItem item in this.gameManager.store.GetState().inventoryState.inventoryContents) {
                //Success case
                if (item.GetId() == 1) {
                    Assert.AreEqual(6, item.GetQuantity());
                    Assert.AreEqual("wood", item.GetName());
                    return;
                }
            }
            Assert.Fail();
        }

        [Test]
        public void TestRemoveMoreThanAvailable() {
            // Assert empty to begin with
            if (this.gameManager.store.GetState().inventoryState.inventoryContents == null || this.gameManager.store.GetState().inventoryState.inventoryContents.Length == 0) {
                Assert.Fail();
            }

            // Add an item and validate it updates the state
            this.gameManager.store.Dispatch(new AddItemToInventory(1, 10, "wood"));
            this.gameManager.store.Dispatch(new RemoveItemFromInventory(1, 11));

            foreach (InventoryItem item in this.gameManager.store.GetState().inventoryState.inventoryContents) {
                //Success case
                if (item.GetId() == 1) {
                    Assert.AreEqual(0, item.GetQuantity());
                    Assert.AreEqual("wood", item.GetName());
                    return;
                }
            }
            Assert.Fail();
        }
    }
}
