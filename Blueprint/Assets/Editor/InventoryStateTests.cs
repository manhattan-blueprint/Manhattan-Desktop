using System.Collections.Generic;
using Model.State;
using Model.Action;
using NUnit.Framework;
using UnityEngine;

namespace Tests {
    public class InventoryStateTests {
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
