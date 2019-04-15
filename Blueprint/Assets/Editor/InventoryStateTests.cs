using Model.Action;
using NUnit.Framework;

namespace Tests {
    public class InventoryStateTests {
        private GameManager gameManager;

        [SetUp]
        public void Setup() {
            gameManager = GameManager.Instance();
            gameManager.ResetGame();
        }

        [Test]
        public void TestGameManagerExists() {
            Assert.NotNull(gameManager);
        }

        [Test]
        public void TestAddToInventory() {
            // Assert empty to begin with
            if (gameManager.inventoryStore.GetState().inventoryContents.Count > 0) {
                Assert.Fail();
            }
            
            // Add an item and validate it updates the state
            gameManager.inventoryStore.Dispatch(new AddItemToInventory(1, 1, "wood"));
            Assert.IsTrue(gameManager.inventoryStore.GetState().inventoryContents.ContainsKey(1));
            Assert.AreEqual(gameManager.inventoryStore.GetState().inventoryContents[1].Count, 1);
            Assert.AreEqual(gameManager.inventoryStore.GetState().inventoryContents[1][0].quantity, 1);
        }

        [Test]
        public void TestAddToInventoryTwice() {
            // Assert empty to begin with
            if (gameManager.inventoryStore.GetState().inventoryContents.Count > 0) {
                Assert.Fail();
            }

            // Add an item, then add the same item again 
            gameManager.inventoryStore.Dispatch(new AddItemToInventory(1, 1, "wood"));
            gameManager.inventoryStore.Dispatch(new AddItemToInventory(1, 10, "wood"));
            
            // Validate the same stack is updated
            Assert.IsTrue(gameManager.inventoryStore.GetState().inventoryContents.ContainsKey(1));
            Assert.AreEqual(gameManager.inventoryStore.GetState().inventoryContents[1].Count, 1);
            Assert.AreEqual(gameManager.inventoryStore.GetState().inventoryContents[1][0].quantity, 11);
        }

        [Test]
        public void TestRemoveFromInventory() {
            // Assert empty to begin with
            if (gameManager.inventoryStore.GetState().inventoryContents.Count > 0) {
                Assert.Fail();
            }

            // Add an item, then remove some of the items
            this.gameManager.inventoryStore.Dispatch(new AddItemToInventory(1, 10, "wood"));
            this.gameManager.inventoryStore.Dispatch(new RemoveItemFromInventory(1, 4));
           
            Assert.IsTrue(gameManager.inventoryStore.GetState().inventoryContents.ContainsKey(1));
            Assert.AreEqual(gameManager.inventoryStore.GetState().inventoryContents[1].Count, 1);
            Assert.AreEqual(gameManager.inventoryStore.GetState().inventoryContents[1][0].quantity, 6);
        }

        [Test]
        public void TestRemoveAllAvailable() {
            // Assert empty to begin with
            if (gameManager.inventoryStore.GetState().inventoryContents.Count > 0) {
                Assert.Fail();
            }

            // Add an item and validate it updates the state
            this.gameManager.inventoryStore.Dispatch(new AddItemToInventory(1, 10, "wood"));
            this.gameManager.inventoryStore.Dispatch(new RemoveItemFromInventory(1, 10));

            Assert.IsFalse(gameManager.inventoryStore.GetState().inventoryContents.ContainsKey(1));
        }
        
        [Test]
        public void TestRemoveMoreThanAvailable() {
            // Assert empty to begin with
            if (gameManager.inventoryStore.GetState().inventoryContents.Count > 0) {
                Assert.Fail();
            }

            // Add an item and validate it updates the state
            this.gameManager.inventoryStore.Dispatch(new AddItemToInventory(1, 10, "wood"));
            this.gameManager.inventoryStore.Dispatch(new RemoveItemFromInventory(1, 11));

            Assert.IsFalse(gameManager.inventoryStore.GetState().inventoryContents.ContainsKey(1));
        }
    }
}
