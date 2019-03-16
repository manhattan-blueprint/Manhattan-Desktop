using System.Collections.Generic;
using Model;
using Model.Action;
using NUnit.Framework;
using NUnit.Framework.Internal;
using UnityEngine;

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
        public void TestLogout() {
            gameManager.store.Dispatch(new OpenPlayingUI());
            gameManager.store.Dispatch(new OpenSettingsUI());
            gameManager.store.Dispatch(new Logout());
            Assert.That(gameManager.store.GetState().uiState.Selected, Is.EqualTo(UIState.OpenUI.Welcome));
        }
        
        [Test]
        public void TestGameManagerExists() {
            Assert.NotNull(gameManager); 
        }
        
        // Inventory Actions
        [Test]
        public void TestAddToInventory() {                        
            // Add an item and validate it updates the state
            this.gameManager.store.Dispatch(new AddItemToInventory(1, 1, "wood"));

            foreach (KeyValuePair<int, List<HexLocation> > item in this.gameManager.store.GetState().inventoryState.inventoryContents) {
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
            if (this.gameManager.store.GetState().inventoryState.inventoryContents.Count > 0) {
                Assert.Fail();
            }
            
            // Add an item and validate it updates the state
            this.gameManager.store.Dispatch(new AddItemToInventory(1, 1, "wood"));
            this.gameManager.store.Dispatch(new AddItemToInventory(1, 10, "wood"));

            foreach (KeyValuePair<int, List<HexLocation> > item in this.gameManager.store.GetState().inventoryState.inventoryContents) {
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
            if (this.gameManager.store.GetState().inventoryState.inventoryContents.Count > 0) {
                Assert.Fail();
            }
            
            // Add an item and validate it updates the state
            this.gameManager.store.Dispatch(new AddItemToInventory(1, 10, "wood"));
            this.gameManager.store.Dispatch(new RemoveItemFromInventory(1, 4));

            foreach (KeyValuePair<int, List<HexLocation> > item in this.gameManager.store.GetState().inventoryState.inventoryContents) {
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
            if (this.gameManager.store.GetState().inventoryState.inventoryContents.Count > 0) {
                Debug.Log(this.gameManager.store.GetState().inventoryState.inventoryContents.Count);
                Assert.Fail();
            }
            
            // Add an item and validate it updates the state
            this.gameManager.store.Dispatch(new AddItemToInventory(1, 10, "wood"));
            this.gameManager.store.Dispatch(new RemoveItemFromInventory(1, 11));

            foreach (KeyValuePair<int, List<HexLocation> > item in this.gameManager.store.GetState().inventoryState.inventoryContents) {
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
            if (this.gameManager.store.GetState().inventoryState.inventoryContents.Count > 0) {
                Debug.Log(this.gameManager.store.GetState().inventoryState.inventoryContents.Count);
                Assert.Fail();
            }
            
            // Add an item 
            this.gameManager.store.Dispatch(new AddItemToInventory(1, 10, "wood"));
            
            // Set item as heldItem  
            this.gameManager.store.Dispatch(new SetHeldItem(new InventoryState.HeldItem(1, new HexLocation(0, 10)))); 
            
            // Assert heldItem has correct ItemID, hexID, quantity
            Assert.That(this.gameManager.store.GetState().inventoryState.heldItem.Get().itemID, Is.EqualTo(1));
            Assert.That(this.gameManager.store.GetState().inventoryState.heldItem.Get().location.hexID, Is.EqualTo(0));
            Assert.That(this.gameManager.store.GetState().inventoryState.heldItem.Get().location.quantity, Is.EqualTo(10));
        }

        [Test]
        public void TestRemoveSomeHeldItem() {
            // Assert empty to begin with
            if (this.gameManager.store.GetState().inventoryState.inventoryContents.Count > 0) {
                Debug.Log(this.gameManager.store.GetState().inventoryState.inventoryContents.Count);
                Assert.Fail();
            }
            
            // Add an item 
            this.gameManager.store.Dispatch(new AddItemToInventory(1, 10, "wood"));
            
            // Set item as heldItem  
            this.gameManager.store.Dispatch(new SetHeldItem(new InventoryState.HeldItem(1, new HexLocation(0, 10)))); 
            
            // Remove 1 from heldItem quantity
            this.gameManager.store.Dispatch(new RemoveHeldItem(new InventoryState.HeldItem(1, new HexLocation(0, 1))));
            
            // Assert reduced quantity
            Assert.That(this.gameManager.store.GetState().inventoryState.heldItem.Get().itemID, Is.EqualTo(1));
            Assert.That(this.gameManager.store.GetState().inventoryState.heldItem.Get().location.hexID, Is.EqualTo(0));
            Assert.That(this.gameManager.store.GetState().inventoryState.heldItem.Get().location.quantity, Is.EqualTo(9));
        }
    }
}
