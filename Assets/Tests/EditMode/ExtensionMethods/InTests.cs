using System.Collections.Generic;
using NUnit.Framework;
using Utility;

namespace Tests.EditMode.ExtensionMethods {
    public class InTests {
        [Test]
        public void ReturnsTrueIfInArguments() {
            Assert.IsTrue(2.In(1, 2, 3));
            Assert.IsTrue("test2".In("test1", "test2", "test3"));
        }

        [Test]
        public void ReturnsFalseIfNotInArguments() {
            Assert.IsFalse(2.In(1, 3));
            Assert.IsFalse("test2".In("test1", "test3"));
        }
        
        [Test]
        public void ReturnsTrueIfInList() {
            var list = new List<int>{1, 2, 3};
            Assert.IsTrue(2.In(list));
        }

        [Test]
        public void ReturnsFalseIfNotInList() {
            var list = new List<int>{1, 3};
            Assert.IsFalse(2.In(list));
        }
    }
}
