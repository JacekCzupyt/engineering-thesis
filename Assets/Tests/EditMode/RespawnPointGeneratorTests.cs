using Game_Systems.Utility;
using NUnit.Framework;

namespace Tests.EditMode {
    public class RespawnPointGeneratorTests {
        [Test]
        public void GeneratesPoints() {
            var points = RespawnPointGenerator.generatePoints(2, 5);
            
            Assert.AreEqual(2, points.Count);
            Assert.AreEqual(5, points[0].magnitude);
            Assert.AreEqual(5, points[1].magnitude);
            Assert.AreNotEqual(points[0], points[1]);
        }
    }
}
