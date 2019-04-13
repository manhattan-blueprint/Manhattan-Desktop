using System;

namespace Model.Graph {
    public class Vertex<V> where V : IEquatable<V> {
        public readonly V id;

        public Vertex(V id) {
            this.id = id;
        }

        public override bool Equals(object obj) {
            Vertex<V> vertex = obj as Vertex<V>;
            return vertex != null && id.Equals(vertex.id);
        }

        public override int GetHashCode() {
            return id.GetHashCode();
        }
    }
}