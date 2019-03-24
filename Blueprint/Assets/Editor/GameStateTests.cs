using System.Collections.Generic;
using Model.State;
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
            gameManager.uiStore.Dispatch(new OpenMachineUI());
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.Machine));
        }

        [Test]
        public void TestCloseMachineUI() {
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            gameManager.uiStore.Dispatch(new OpenMachineUI());
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
            gameManager.uiStore.Dispatch(new OpenMachineUI());
            gameManager.uiStore.Dispatch(new OpenSettingsUI());
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.MachPause));
        }

        [Test]
        public void TestCloseMachSettingsUI() {
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            gameManager.uiStore.Dispatch(new OpenMachineUI());
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
            gameManager.uiStore.Dispatch(new OpenMachineUI());
            gameManager.uiStore.Dispatch(new OpenSettingsUI());
            gameManager.uiStore.Dispatch(new Logout());
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.MachLogout));
        }

        [Test]
        public void TestCloseMachLogoutUI() {
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            gameManager.uiStore.Dispatch(new OpenMachineUI());
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
            gameManager.uiStore.Dispatch(new OpenMachineUI());
            gameManager.uiStore.Dispatch(new OpenSettingsUI());
            gameManager.uiStore.Dispatch(new Exit());
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.MachExit));
        }

        [Test]
        public void TestCloseMachExitUI() {
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            gameManager.uiStore.Dispatch(new OpenMachineUI());
            gameManager.uiStore.Dispatch(new OpenSettingsUI());
            gameManager.uiStore.Dispatch(new Exit());
            gameManager.uiStore.Dispatch(new CloseUI());
            Assert.That(gameManager.uiStore.GetState().Selected, Is.EqualTo(UIState.OpenUI.MachPause));
        }

        [Test]
        public void TestMachExitUIToMachineUI() {
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            gameManager.uiStore.Dispatch(new OpenMachineUI());
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
        public void TestGameManagerExists() {
            Assert.NotNull(gameManager);
        }

        // Inventory Actions
        [Test]
        public void TestAddToInventory() {
            // Add an item and validate it updates the state
            gameManager.uiStore.Dispatch(new OpenPlayingUI());
            this.gameManager.inventoryStore.Dispatch(new AddItemToInventory(1, 1, "wood"));

            foreach (KeyValuePair<int, List<HexLocation> > item in this.gameManager.inventoryStore.GetState().inventoryContents) {
                //Success case
                if (item.Key == 1) {
                    Assert.AreEqual(1, item.Value[0].quantity);
                    return;
                }
            }
            Assert.Fail();
        }

        [Test]
        public void TestAddToInventoryTwice() {
            // Assert empty to begin with
            if (this.gameManager.inventoryStore.GetState().inventoryContents.Count > 0) {
                Assert.Fail();
            }

            // Add an item and validate it updates the state
            this.gameManager.inventoryStore.Dispatch(new AddItemToInventory(1, 1, "wood"));
            this.gameManager.inventoryStore.Dispatch(new AddItemToInventory(1, 10, "wood"));

            foreach (KeyValuePair<int, List<HexLocation> > item in this.gameManager.inventoryStore.GetState().inventoryContents) {
                //Success case
                if (item.Key == 1) {
                    Assert.AreEqual(11, item.Value[0].quantity);
                    return;
                }
            }
            Assert.Fail();
        }

        [Test]
        public void TestRemoveFromInventory() {
            // Assert empty to begin with
            if (this.gameManager.inventoryStore.GetState().inventoryContents.Count > 0) {
                Assert.Fail();
            }

            // Add an item and validate it updates the state
            this.gameManager.inventoryStore.Dispatch(new AddItemToInventory(1, 10, "wood"));
            this.gameManager.inventoryStore.Dispatch(new RemoveItemFromInventory(1, 4));

            foreach (KeyValuePair<int, List<HexLocation> > item in this.gameManager.inventoryStore.GetState().inventoryContents) {

                //Success case
                if (item.Key == 1) {
                    Assert.AreEqual(6, item.Value[0].quantity);
                    return;
                }
            }
            Assert.Fail();
        }

        [Test]
        public void TestRemoveMoreThanAvailable() {
            // Assert empty to begin with
            if (this.gameManager.inventoryStore.GetState().inventoryContents.Count > 0) {
                Assert.Fail();
            }

            // Add an item and validate it updates the state
            this.gameManager.inventoryStore.Dispatch(new AddItemToInventory(1, 10, "wood"));
            this.gameManager.inventoryStore.Dispatch(new RemoveItemFromInventory(1, 11));

            foreach (KeyValuePair<int, List<HexLocation> > item in this.gameManager.inventoryStore.GetState().inventoryContents) {
                //Success case
                if (item.Key == 1) {
                    Assert.AreEqual(0, item.Value.Count);
                    return;
                }
            }
            Assert.Fail();
        }

        [Test]
        public void TestSetHeldItem() {
            // Assert empty to begin with
            if (this.gameManager.inventoryStore.GetState().inventoryContents.Count > 0) {
                Assert.Fail();
            }

            // Add an item
            this.gameManager.inventoryStore.Dispatch(new AddItemToInventory(1, 10, "wood"));

//            // Set item as heldItem
//            this.gameManager.store.Dispatch(new SetHeldItem(new InventoryState.HeldItem(1, new HexLocation(0, 10))));

            // Assert heldItem has correct ItemID, hexID, quantity
//            Assert.That(this.gameManager.store.GetState().inventoryState.heldItem.Get().itemID, Is.EqualTo(1));
//            Assert.That(this.gameManager.store.GetState().inventoryState.heldItem.Get().location.hexID, Is.EqualTo(0));
//            Assert.That(this.gameManager.store.GetState().inventoryState.heldItem.Get().location.quantity, Is.EqualTo(10));
        }

        [Test]
        public void TestRemoveSomeHeldItem() {
            // Assert empty to begin with
            if (this.gameManager.inventoryStore.GetState().inventoryContents.Count > 0) {
                Assert.Fail();
            }

            // Add an item
            this.gameManager.inventoryStore.Dispatch(new AddItemToInventory(1, 10, "wood"));

//            // Set item as heldItem
//            this.gameManager.store.Dispatch(new SetHeldItem(new InventoryState.HeldItem(1, new HexLocation(0, 10))));
//
//            // Remove 1 from heldItem quantity
//            this.gameManager.store.Dispatch(new RemoveHeldItem(new InventoryState.HeldItem(1, new HexLocation(0, 1))));

            // Assert reduced quantity
//            Assert.That(this.gameManager.store.GetState().inventoryState.heldItem.Get().itemID, Is.EqualTo(1));
//            Assert.That(this.gameManager.store.GetState().inventoryState.heldItem.Get().location.hexID, Is.EqualTo(0));
//            Assert.That(this.gameManager.store.GetState().inventoryState.heldItem.Get().location.quantity, Is.EqualTo(9));
        }
    }
}
