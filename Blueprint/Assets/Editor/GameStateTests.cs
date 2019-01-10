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
            Assert.False(this.gameManager.store.GetState().inventoryState.inventoryContents.ContainsKey(1));
            
            // Add an item and validate it updates the state
            this.gameManager.store.Dispatch(new AddItemToInventory(1, 1));
            Assert.True(this.gameManager.store.GetState().inventoryState.inventoryContents.ContainsKey(1));
            Assert.AreEqual(1, this.gameManager.store.GetState().inventoryState.inventoryContents[1]);
        }

        [Test]
        public void TestAddToInventoryTwice() {
            // Assert empty to begin with
            Assert.False(this.gameManager.store.GetState().inventoryState.inventoryContents.ContainsKey(1));
            
            // Add an item and validate it updates the state
            this.gameManager.store.Dispatch(new AddItemToInventory(1, 1));
            this.gameManager.store.Dispatch(new AddItemToInventory(1, 10));
            Assert.True(this.gameManager.store.GetState().inventoryState.inventoryContents.ContainsKey(1));
            Assert.AreEqual(11, this.gameManager.store.GetState().inventoryState.inventoryContents[1]);
        }

        [Test]
        public void TestRemoveFromInventory() {
            // Assert empty to begin with
            Assert.False(this.gameManager.store.GetState().inventoryState.inventoryContents.ContainsKey(1));
            
            // Add an item and validate it updates the state
            this.gameManager.store.Dispatch(new AddItemToInventory(1, 10));
            this.gameManager.store.Dispatch(new RemoveItemFromInventory(1, 4));
            Assert.True(this.gameManager.store.GetState().inventoryState.inventoryContents.ContainsKey(1));
            Assert.AreEqual(6, this.gameManager.store.GetState().inventoryState.inventoryContents[1]);
        }

        [Test]
        public void TestRemoveMoreThanAvailable() {
            // Assert empty to begin with
            Assert.False(this.gameManager.store.GetState().inventoryState.inventoryContents.ContainsKey(1));
            
            // Add an item and validate it updates the state
            this.gameManager.store.Dispatch(new AddItemToInventory(1, 10));
            this.gameManager.store.Dispatch(new RemoveItemFromInventory(1, 11));
            Assert.True(this.gameManager.store.GetState().inventoryState.inventoryContents.ContainsKey(1));
            Assert.AreEqual(0, this.gameManager.store.GetState().inventoryState.inventoryContents[1]);
        }

    }
}
