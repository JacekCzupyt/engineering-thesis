using System.Collections.Generic;
using NUnit.Framework;
using Utility;

namespace Tests.EditMode.ExtensionMethods {
    public class RandomElementTests
    {
        [Test]
        public void ReturnsTrueIfInArguments() {
            var list = new List<int> {1, 2, 3};
            Assert.Contains(list.RandomElement(), list);
        }
    }
}
