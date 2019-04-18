using System.Collections.Generic;
using Model.Action;
using NUnit.Framework;
using UnityEngine;
using Utils;

namespace Tests {
    public class WireTests {
        private string testJsonsLocation = "Assets/Editor/TestJsons/";
        private string itemSchemaV2 = "item-schema-v2.json";
        
        [SetUp]
        public void SetUp() {
            GameManager.Instance().ResetGame();
            // Mock schema
            GameManager.Instance().sm = SchemaManager.FromFilepath(testJsonsLocation + itemSchemaV2);
        }

        [Test]
        public void TestOriginNeighbours() {
            Vector2 test = new Vector2(0, 0);
            List<Vector2> neighbours = test.HexNeighbours();
            Assert.That(neighbours.Count, Is.EqualTo(6));
            Assert.Contains(new Vector2(-1, 1), neighbours);
            Assert.Contains(new Vector2(0, 1), neighbours);
            Assert.Contains(new Vector2(1, 0), neighbours);
            Assert.Contains(new Vector2(1, -1), neighbours);
            Assert.Contains(new Vector2(0, -1), neighbours);
            Assert.Contains(new Vector2(-1, 0), neighbours);
        }

        [Test]
        public void TestNeighbours() {
            Vector2 test = new Vector2(4, -2);
            List<Vector2> neighbours = test.HexNeighbours();
            Assert.That(neighbours.Count, Is.EqualTo(6));
            Assert.Contains(new Vector2(3, -1), neighbours);
            Assert.Contains(new Vector2(4, -1), neighbours);
            Assert.Contains(new Vector2(5, -2), neighbours);
            Assert.Contains(new Vector2(5, -3), neighbours);
            Assert.Contains(new Vector2(4, -3), neighbours);
            Assert.Contains(new Vector2(3, -2), neighbours);
        }

        [Test]
        public void TestMachineNextToSolarPanel() {
            Vector2 machineLocation = new Vector2(0, 0);
            Vector2 solarLocation = new Vector2(0, 1);
            GameManager.Instance().machineStore.Dispatch(new AddMachine(machineLocation, 26));
            GameManager.Instance().mapStore.Dispatch(new PlaceItem(machineLocation, 26)); 
            GameManager.Instance().mapStore.Dispatch(new PlaceItem(solarLocation, 25));

            Assert.True(GameManager.Instance().machineStore.GetState().grid[machineLocation].HasFuel());
        }

        [Test]
        public void TestMachineOneAwayFromSolarPanelWithoutWire() {
            Vector2 machineLocation = new Vector2(0, 0);
            Vector2 solarLocation = new Vector2(0, 2);
            GameManager.Instance().machineStore.Dispatch(new AddMachine(machineLocation, 26));
            GameManager.Instance().mapStore.Dispatch(new PlaceItem(machineLocation, 26)); 
            GameManager.Instance().mapStore.Dispatch(new PlaceItem(solarLocation, 25));
            
            Assert.False(GameManager.Instance().machineStore.GetState().grid[machineLocation].HasFuel());
        }
        
        [Test]
        public void TestMachineOneAwayFromSolarPanelWithWire() {
            Vector2 machineLocation = new Vector2(0, 0);
            Vector2 wireLocation = new Vector2(0, 1);
            Vector2 solarLocation = new Vector2(0, 2);
            GameManager.Instance().machineStore.Dispatch(new AddMachine(machineLocation, 26));
            GameManager.Instance().mapStore.Dispatch(new PlaceItem(machineLocation, 26)); 
            GameManager.Instance().mapStore.Dispatch(new PlaceItem(solarLocation, 25));
            GameManager.Instance().mapStore.Dispatch(new PlaceItem(wireLocation, 22));
            
            Assert.True(GameManager.Instance().machineStore.GetState().grid[machineLocation].HasFuel());
        }

        [Test]
        public void TestMachineTwoAwayFromSolarPanelWithWireCycle() {
            Vector2 machineLocation = new Vector2(-1, 2);
            List<Vector2> wireLocations = new List<Vector2> {
                new Vector2(0, 1),
                new Vector2(1, 0),
                new Vector2(1, -1),
                new Vector2(0, -1),
                new Vector2(-1, 0),
                new Vector2(-1, 1)
            };
            Vector2 solarLocation = new Vector2(0, 0);
            
            GameManager.Instance().machineStore.Dispatch(new AddMachine(machineLocation, 26));
            GameManager.Instance().mapStore.Dispatch(new PlaceItem(machineLocation, 26)); 
            GameManager.Instance().mapStore.Dispatch(new PlaceItem(solarLocation, 25));
           
            // Shouldn't be connected before wires are placed
            Assert.False(GameManager.Instance().machineStore.GetState().grid[machineLocation].HasFuel());
            
            foreach (Vector2 wireLocation in wireLocations) {
                GameManager.Instance().mapStore.Dispatch(new PlaceItem(wireLocation, 22));
            }
            Assert.True(GameManager.Instance().machineStore.GetState().grid[machineLocation].HasFuel());
        }

        [Test]
        public void TestMachineFarAwayFromSolarPanelWithComplexWiring() {
            Vector2 machineLocation = new Vector2(5, -3);
            List<Vector2> wireLocations = new List<Vector2> {
                new Vector2(-2, -2),
                new Vector2(-1, -2),
                new Vector2(0, -2),
                new Vector2(0, -1),
                new Vector2(1, -1),
                new Vector2(2, -2),
                new Vector2(3, -3),
                new Vector2(3, -2)
            };
            Vector2 solarLocation = new Vector2(-2, -3);
            
            GameManager.Instance().machineStore.Dispatch(new AddMachine(machineLocation, 26));
            GameManager.Instance().mapStore.Dispatch(new PlaceItem(machineLocation, 26)); 
            GameManager.Instance().mapStore.Dispatch(new PlaceItem(solarLocation, 25));
            // Shouldn't be connected before wires are placed
            Assert.False(GameManager.Instance().machineStore.GetState().grid[machineLocation].HasFuel());
            
            foreach (Vector2 wireLocation in wireLocations) {
                GameManager.Instance().mapStore.Dispatch(new PlaceItem(wireLocation, 22));
            }
            // Should still be disconnected as 1 wire extra is needed to complete
            Assert.False(GameManager.Instance().machineStore.GetState().grid[machineLocation].HasFuel());
            
            // Place final wire needed to complete circuit
            GameManager.Instance().mapStore.Dispatch(new PlaceItem(new Vector2(4, -2), 22));
            Assert.True(GameManager.Instance().machineStore.GetState().grid[machineLocation].HasFuel());
        }

        [Test]
        public void TestMachineFarAwayFromSolarPanelWithComplexWiringThenRemovingWireIsNoLongerPowered () {
            Vector2 machineLocation = new Vector2(5, -3);
            List<Vector2> wireLocations = new List<Vector2> {
                new Vector2(-2, -2),
                new Vector2(-1, -2),
                new Vector2(0, -2),
                new Vector2(0, -1),
                new Vector2(1, -1),
                new Vector2(2, -2),
                new Vector2(3, -3),
                new Vector2(3, -2),
                new Vector2(4, -2)
            };
            Vector2 solarLocation = new Vector2(-2, -3);
            
            GameManager.Instance().machineStore.Dispatch(new AddMachine(machineLocation, 26));
            GameManager.Instance().mapStore.Dispatch(new PlaceItem(machineLocation, 26)); 
            GameManager.Instance().mapStore.Dispatch(new PlaceItem(solarLocation, 25));
            // Shouldn't be connected before wires are placed
            Assert.False(GameManager.Instance().machineStore.GetState().grid[machineLocation].HasFuel());
            
            foreach (Vector2 wireLocation in wireLocations) {
                GameManager.Instance().mapStore.Dispatch(new PlaceItem(wireLocation, 22));
            }
            // should be connected
            Assert.True(GameManager.Instance().machineStore.GetState().grid[machineLocation].HasFuel());
            
            GameManager.Instance().mapStore.Dispatch(new CollectItem(new Vector2(2, -2)));
            
            // should not be connected as circuit is broken
            Assert.False(GameManager.Instance().machineStore.GetState().grid[machineLocation].HasFuel());
        }

        [Test]
        public void TestMultipleConnectedSolarPanels() {
            Vector2 machineLocation = new Vector2(-5, 3); 
            Vector2 solarLocation1 = new Vector2(-3, 1);
            Vector2 solarLocation2 = new Vector2(-2, 1);
            List<Vector2> wireLocations = new List<Vector2> {
                new Vector2(-4, 2),
                new Vector2(-3, 2)
            };
            GameManager.Instance().machineStore.Dispatch(new AddMachine(machineLocation, 26));
            GameManager.Instance().mapStore.Dispatch(new PlaceItem(machineLocation, 26)); 
            GameManager.Instance().mapStore.Dispatch(new PlaceItem(solarLocation1, 25));
            GameManager.Instance().mapStore.Dispatch(new PlaceItem(solarLocation2, 25));
            
            // Shouldn't be connected before wires are placed
            Assert.False(GameManager.Instance().machineStore.GetState().grid[machineLocation].HasFuel());
            foreach (Vector2 wireLocation in wireLocations) {
                GameManager.Instance().mapStore.Dispatch(new PlaceItem(wireLocation, 22));
            }
            // Should be connected
            Assert.True(GameManager.Instance().machineStore.GetState().grid[machineLocation].HasFuel());
            
            // Remove solar panel
            GameManager.Instance().mapStore.Dispatch(new CollectItem(solarLocation1));
            
            // Should still be connected to other solar panel
            Assert.True(GameManager.Instance().machineStore.GetState().grid[machineLocation].HasFuel());
        }

        [Test]
        public void TestDoubleCycle() {
            Vector2 machineLocation = new Vector2(3, -3);
            List<Vector2> wireLocations = new List<Vector2> {
                new Vector2(0, 1),
                new Vector2(1, 0),
                new Vector2(1, -1),
                new Vector2(0, -1),
                new Vector2(-1, 0),
                new Vector2(-1, 1),
                new Vector2(-1, 2),
                new Vector2(0, 2),
                new Vector2(1, 1),
                new Vector2(2, 0),
                new Vector2(2, -1),
                new Vector2(2, -2),
                new Vector2(1, -2),
                new Vector2(0, -2),
                new Vector2(-1, -1),
                new Vector2(-2, 0),
                new Vector2(-2, 1),
                new Vector2(-2, 2),
                new Vector2(-1, 2)
            };
            Vector2 solarLocation = new Vector2(0, 0);
            
            GameManager.Instance().machineStore.Dispatch(new AddMachine(machineLocation, 26));
            GameManager.Instance().mapStore.Dispatch(new PlaceItem(machineLocation, 26)); 
            GameManager.Instance().mapStore.Dispatch(new PlaceItem(solarLocation, 25));
            // Shouldn't be connected before wires are placed
            Assert.False(GameManager.Instance().machineStore.GetState().grid[machineLocation].HasFuel());
            
            foreach (Vector2 wireLocation in wireLocations) {
                GameManager.Instance().mapStore.Dispatch(new PlaceItem(wireLocation, 22));
            }
            // should be connected
            Assert.True(GameManager.Instance().machineStore.GetState().grid[machineLocation].HasFuel());
        }

        [Test]
        // This originated from @adam-c-fox's PR comment
        public void TestCrossover() {
            List<Vector2> machineLocations = new List<Vector2> {
                new Vector2(1, -1),
                new Vector2(1, 2)
            };
            
            List<Vector2> wireLocations = new List<Vector2> {
                new Vector2(-2, 3),
                new Vector2(-1, 2),
                new Vector2(0, 2),
                new Vector2(0,1),
                new Vector2(1, 0),
                new Vector2(-1, 1),
                new Vector2(-2, 1),
            };
            
            List<Vector2> solarLocations = new List<Vector2> {
                new Vector2(-2, 0),
                new Vector2(-3, 3)
            };

            foreach (Vector2 machineLocation in machineLocations) {
                GameManager.Instance().machineStore.Dispatch(new AddMachine(machineLocation, 29));
                GameManager.Instance().mapStore.Dispatch(new PlaceItem(machineLocation, 25));
            }
            
            foreach (Vector2 solarLocation in solarLocations) {
                GameManager.Instance().mapStore.Dispatch(new PlaceItem(solarLocation, 25));
            }
            
            Assert.False(GameManager.Instance().machineStore.GetState().grid[machineLocations[0]].HasFuel());
            Assert.False(GameManager.Instance().machineStore.GetState().grid[machineLocations[1]].HasFuel());
            
            foreach (Vector2 wireLocation in wireLocations) {
                GameManager.Instance().mapStore.Dispatch(new PlaceItem(wireLocation, 22));
            }
            
            Assert.True(GameManager.Instance().machineStore.GetState().grid[machineLocations[0]].HasFuel());
            Assert.True(GameManager.Instance().machineStore.GetState().grid[machineLocations[1]].HasFuel());
        }
    }
}