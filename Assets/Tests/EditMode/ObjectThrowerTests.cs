using Game_Systems.Utility;
using NUnit.Framework;

namespace Tests.EditMode {
    public class ObjectThrowerTests
    {
        [Test]
        public void ReturnsStaticThrow() {
            const ThrowType throwType = ThrowType.Static;
            var throwMethod = ObjectThrower.Throw(throwType);
            Assert.AreEqual((ObjectThrower.ThrowMethod)ObjectThrower.StaticThrow, throwMethod);
        }
    }
}
