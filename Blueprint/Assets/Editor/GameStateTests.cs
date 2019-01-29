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
        public void TestGameManagerExists() {
            Assert.NotNull(gameManager); 
        }
        
        // Inventory Actions
        [Test]
        public void TestAddToInventory() {
            // Assert empty to begin with
                        
            // Add an item and validate it updates the state
            this.gameManager.store.Dispatch(new AddItemToInventory(1, 1, "steel"));

            foreach (InventoryItem item in this.gameManager.store.GetState().inventoryState.inventoryContents) {
                //Success case
                if (item.GetId() == 1) {
                    Assert.AreEqual(1, item.GetQuantity());
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
            this.gameManager.store.Dispatch(new AddItemToInventory(1, 1, "coal"));
            this.gameManager.store.Dispatch(new AddItemToInventory(1, 10, "iron"));

            foreach (InventoryItem item in this.gameManager.store.GetState().inventoryState.inventoryContents) {
                //Success case
                if (item.GetId() == 1) {
                    Assert.AreEqual(11, item.GetQuantity());
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
            this.gameManager.store.Dispatch(new AddItemToInventory(1, 10, "blueprints"));
            this.gameManager.store.Dispatch(new RemoveItemFromInventory(1, 4));

            foreach (InventoryItem item in this.gameManager.store.GetState().inventoryState.inventoryContents) {
                //Success case
                if (item.GetId() == 1) {
                    Assert.AreEqual(6, item.GetQuantity());
                    return;
                }
            }
        }

        [Test]
        public void TestRemoveMoreThanAvailable() {
            // Assert empty to begin with
            if (this.gameManager.store.GetState().inventoryState.inventoryContents == null || this.gameManager.store.GetState().inventoryState.inventoryContents.Length == 0) {
                Assert.Fail();
            }
            
            // Add an item and validate it updates the state
            this.gameManager.store.Dispatch(new AddItemToInventory(1, 10, "chocolate"));
            this.gameManager.store.Dispatch(new RemoveItemFromInventory(1, 11));

            foreach (InventoryItem item in this.gameManager.store.GetState().inventoryState.inventoryContents) {
                //Success case
                if (item.GetId() == 1) {
                    Assert.AreEqual(0, item.GetQuantity());
                    return;
                }
            }
        }
    }
}
