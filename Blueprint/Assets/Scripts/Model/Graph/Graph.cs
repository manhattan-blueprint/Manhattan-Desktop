using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Graph {

    // An undirected graph
    public class Graph<T> where T : IEquatable<T> {
        private Dictionary<Vertex<T>, List<Vertex<T>>> adjMatrix;
        
        public Graph() {
            this.adjMatrix = new Dictionary<Vertex<T>, List<Vertex<T>>>();
        }

        public void AddEdge(T a, T b) {
            Vertex<T> vertexA = new Vertex<T>(a);
            Vertex<T> vertexB = new Vertex<T>(b);
            
            if (adjMatrix.ContainsKey(vertexA)) {
                adjMatrix[vertexA].Add(vertexB); 
            } else {
                adjMatrix.Add(vertexA, new List<Vertex<T>> { vertexB });
            }
            
            if (adjMatrix.ContainsKey(vertexB)) {
                adjMatrix[vertexB].Add(vertexA); 
            } else {
                adjMatrix.Add(vertexB, new List<Vertex<T>> { vertexA });
            }
            
        }

        public bool ContainsEdge(T a, T b) {
            Vertex<T> vertexA = new Vertex<T>(a);
            Vertex<T> vertexB = new Vertex<T>(b);
            return adjMatrix.ContainsKey(vertexA) && adjMatrix[vertexA].Contains(vertexB);
        }

        public void RemoveEdge(T a, T b) {
            Vertex<T> vertexA = new Vertex<T>(a);
            Vertex<T> vertexB = new Vertex<T>(b);
            adjMatrix[vertexA].Remove(vertexB);
            adjMatrix[vertexB].Remove(vertexA);
        }
        
        public bool AreConnected(T startID, T finishID) {
            HashSet<Vertex<T>> visited = new HashSet<Vertex<T>>();
            
            Vertex<T> start = adjMatrix.Keys.First(v => v.id.Equals(startID));
            Vertex<T> finish = adjMatrix.Keys.First(v => v.id.Equals(finishID));
            visited.Add(start);
            
            // Perform BFS 
            Queue<Vertex<T>> queue = new Queue<Vertex<T>>();
            queue.Enqueue(start);
            while (queue.Count != 0) {
                Vertex<T> u = queue.Dequeue();
                foreach (Vertex<T> v in adjMatrix[u]) {
                    if (v.Equals(finish)) return true;
                    
                    if (!visited.Contains(v)) {
                        visited.Add(v);
                        queue.Enqueue(v);
                    }
                }
            }
            return false;
        }
    }
}