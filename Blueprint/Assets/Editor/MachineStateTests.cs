using Model;
using Model.Action;
using NUnit.Framework;
using UnityEngine;

namespace Tests {
    public class MachineStateTests {
        private GameManager gameManager;

        [SetUp]
        public void Setup() {
            this.gameManager = GameManager.Instance();
            gameManager.ResetGame();
        }

        [Test]
        public void TestAddMachine() {
            Vector2 location = new Vector2(0, 0);
           
            // Check is initially empty
            Assert.That(!gameManager.machineStore.GetState().grid.ContainsKey(location));
            gameManager.machineStore.Dispatch(new AddMachine(location, 11));
            
            Assert.That(gameManager.machineStore.GetState().grid.ContainsKey(location));
            Assert.That(gameManager.machineStore.GetState().grid[location].id, Is.EqualTo(11));
            Assert.That(!gameManager.machineStore.GetState().grid[location].fuel.IsPresent());
            Assert.That(!gameManager.machineStore.GetState().grid[location].leftInput.IsPresent());
            Assert.That(!gameManager.machineStore.GetState().grid[location].rightInput.IsPresent());
            Assert.That(!gameManager.machineStore.GetState().grid[location].output.IsPresent());
        }

        [Test]
        public void TestAddLeftInput() {
            Vector2 location = new Vector2(-5, 5);
           
            // Check is initially empty
            Assert.That(!gameManager.machineStore.GetState().grid.ContainsKey(location));
            gameManager.machineStore.Dispatch(new AddMachine(location, 11));
            Assert.That(!gameManager.machineStore.GetState().grid[location].leftInput.IsPresent());
           
            InventoryItem leftItem = new InventoryItem("Snape's Cape", 1, 1);
            gameManager.machineStore.Dispatch(new SetLeftInput(location, leftItem));
            Assert.That(gameManager.machineStore.GetState().grid[location].leftInput.IsPresent());
            Assert.That(gameManager.machineStore.GetState().grid[location].leftInput.Get().GetId(), Is.EqualTo(leftItem.GetId()));
            Assert.That(gameManager.machineStore.GetState().grid[location].leftInput.Get().GetQuantity(), Is.EqualTo(leftItem.GetQuantity()));
            Assert.That(gameManager.machineStore.GetState().grid[location].leftInput.Get().GetName(), Is.EqualTo(leftItem.GetName()));
        }
        
        [Test]
        public void TestAddRightInput() {
            Vector2 location = new Vector2(-1, 1);
           
            // Check is initially empty
            Assert.That(!gameManager.machineStore.GetState().grid.ContainsKey(location));
            gameManager.machineStore.Dispatch(new AddMachine(location, 11));
            Assert.That(!gameManager.machineStore.GetState().grid[location].rightInput.IsPresent());
           
            InventoryItem rightInput = new InventoryItem("Philosopher's Stone", 2, 1);
            gameManager.machineStore.Dispatch(new SetRightInput(location, rightInput));
            Assert.That(gameManager.machineStore.GetState().grid[location].rightInput.IsPresent());
            Assert.That(gameManager.machineStore.GetState().grid[location].rightInput.Get().GetId(), Is.EqualTo(rightInput.GetId()));
            Assert.That(gameManager.machineStore.GetState().grid[location].rightInput.Get().GetQuantity(), Is.EqualTo(rightInput.GetQuantity()));
            Assert.That(gameManager.machineStore.GetState().grid[location].rightInput.Get().GetName(), Is.EqualTo(rightInput.GetName()));
        }

        [Test]
        public void TestAddFuel() {
            Vector2 location = new Vector2(-1, 1);
           
            // Check is initially empty
            Assert.That(!gameManager.machineStore.GetState().grid.ContainsKey(location));
            gameManager.machineStore.Dispatch(new AddMachine(location, 11));
            Assert.That(!gameManager.machineStore.GetState().grid[location].fuel.IsPresent());
            
            InventoryItem fuel = new InventoryItem("Dobby's Sock", 16, 1);
            gameManager.machineStore.Dispatch(new SetFuel(location, Optional<InventoryItem>.Of(fuel)));
            Assert.That(gameManager.machineStore.GetState().grid[location].fuel.IsPresent());
            Assert.That(gameManager.machineStore.GetState().grid[location].fuel.Get().GetId(), Is.EqualTo(fuel.GetId()));
            Assert.That(gameManager.machineStore.GetState().grid[location].fuel.Get().GetQuantity(), Is.EqualTo(fuel.GetQuantity()));
            Assert.That(gameManager.machineStore.GetState().grid[location].fuel.Get().GetName(), Is.EqualTo(fuel.GetName()));
        }
        
        [Test]
        public void TestAddLeftRightInput() {
            Vector2 location = new Vector2(-1, 1);
           
            // Check is initially empty
            Assert.That(!gameManager.machineStore.GetState().grid.ContainsKey(location));
            gameManager.machineStore.Dispatch(new AddMachine(location, 11));
            Assert.That(!gameManager.machineStore.GetState().grid[location].leftInput.IsPresent());
            Assert.That(!gameManager.machineStore.GetState().grid[location].rightInput.IsPresent());
            
            InventoryItem leftItem = new InventoryItem("The Blood of a Muggle", 1, 1);
            InventoryItem rightInput = new InventoryItem("A Strand of Ron Weasley's Ginger Hair", 2, 1);
            
            gameManager.machineStore.Dispatch(new SetLeftInput(location, leftItem));
            gameManager.machineStore.Dispatch(new SetRightInput(location, rightInput));
            
            Assert.That(gameManager.machineStore.GetState().grid[location].leftInput.IsPresent());
            Assert.That(gameManager.machineStore.GetState().grid[location].leftInput.Get().GetId(), Is.EqualTo(leftItem.GetId()));
            Assert.That(gameManager.machineStore.GetState().grid[location].leftInput.Get().GetQuantity(), Is.EqualTo(leftItem.GetQuantity()));
            Assert.That(gameManager.machineStore.GetState().grid[location].leftInput.Get().GetName(), Is.EqualTo(leftItem.GetName()));
            
            Assert.That(gameManager.machineStore.GetState().grid[location].rightInput.IsPresent());
            Assert.That(gameManager.machineStore.GetState().grid[location].rightInput.Get().GetId(), Is.EqualTo(rightInput.GetId()));
            Assert.That(gameManager.machineStore.GetState().grid[location].rightInput.Get().GetQuantity(), Is.EqualTo(rightInput.GetQuantity()));
            Assert.That(gameManager.machineStore.GetState().grid[location].rightInput.Get().GetName(), Is.EqualTo(rightInput.GetName()));
        }
        
         
    }
}