using Model.Graph;
using NUnit.Framework;
using UnityEngine;

namespace Tests {
    public class GraphTests {
        private Graph<int> intGraph;
        private Graph<string> stringGraph;

        [SetUp]
        public void Setup() {
            intGraph = new Graph<int>();
            stringGraph = new Graph<string>();
        }

        [Test]
        public void TestGraphAddEdge() {
            intGraph.AddEdge(1, 2);
            Assert.True(intGraph.ContainsEdge(1, 2));
            
            stringGraph.AddEdge("a", "b");
            Assert.True(stringGraph.ContainsEdge("a", "b"));
        }

        [Test]
        public void TestGraphUndirectedEdges() {
            intGraph.AddEdge(1, 2);
            Assert.True(intGraph.ContainsEdge(1, 2));
            Assert.True(intGraph.ContainsEdge(2, 1));
            
            stringGraph.AddEdge("a", "b");
            Assert.True(stringGraph.ContainsEdge("a", "b"));
            Assert.True(stringGraph.ContainsEdge("b", "a"));
        }

        [Test]
        public void TestGraphRemoveEdge() {
            intGraph.AddEdge(1, 2);
            Assert.True(intGraph.ContainsEdge(1, 2));
            intGraph.RemoveEdge(1, 2);
            Assert.False(intGraph.ContainsEdge(1, 2));
             
            stringGraph.AddEdge("a", "b");
            Assert.True(stringGraph.ContainsEdge("a", "b"));
            stringGraph.RemoveEdge("a", "b");
            Assert.False(stringGraph.ContainsEdge("a", "b"));
        }

        [Test]
        public void TestGraphRemoveEdgeUndirected() {
            intGraph.AddEdge(1, 2);
            Assert.True(intGraph.ContainsEdge(1, 2));
            Assert.True(intGraph.ContainsEdge(2, 1));
            intGraph.RemoveEdge(2, 1);
            Assert.False(intGraph.ContainsEdge(1, 2));
            Assert.False(intGraph.ContainsEdge(2, 1));
             
            stringGraph.AddEdge("a", "b");
            Assert.True(stringGraph.ContainsEdge("a", "b"));
            Assert.True(stringGraph.ContainsEdge("b", "a"));
            stringGraph.RemoveEdge("b", "a");
            Assert.False(stringGraph.ContainsEdge("a", "b"));
            Assert.False(stringGraph.ContainsEdge("b", "a"));
        }
        
        
        /*
         * For the following test, we consider the following disconnected graph
         *
         *     A - B        D - E
         *     \   /        \   /
         *       C            F
         * 
         */

        [Test]
        public void TestGraphConnected() {
            stringGraph.AddEdge("a", "b");
            stringGraph.AddEdge("a", "c");
            stringGraph.AddEdge("b", "c");
           
            stringGraph.AddEdge("d", "e");
            stringGraph.AddEdge("d", "f");
            stringGraph.AddEdge("e", "f");
            
            Assert.True(stringGraph.AreConnected("a", "b"));
            Assert.True(stringGraph.AreConnected("a", "c"));
            Assert.True(stringGraph.AreConnected("b", "c"));
            Assert.False(stringGraph.AreConnected("b", "f"));
            Assert.False(stringGraph.AreConnected("b", "e"));
            Assert.False(stringGraph.AreConnected("c", "e"));
            Assert.False(stringGraph.AreConnected("a", "d"));
            
            
            // Add in missing edge to make graph connected
            stringGraph.AddEdge("c", "f");
            Assert.True(stringGraph.AreConnected("b", "f"));
            Assert.True(stringGraph.AreConnected("b", "e"));
            Assert.True(stringGraph.AreConnected("c", "e"));
            Assert.True(stringGraph.AreConnected("a", "d"));
            Assert.True(stringGraph.AreConnected("a", "e"));
        }
        
    }



}