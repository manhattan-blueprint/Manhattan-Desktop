using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using Utils;

namespace Tests {
    public class WireTests {

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
    }
}