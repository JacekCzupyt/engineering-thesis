using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ObjectThrowerTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void ObjectThrowerTestsSimplePasses()
    {
        Assert.AreEqual(1, 1);
    }

    [Test]
    public void Fail() {
        Assert.AreEqual(1, 0);
    }

    // // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // // `yield return null;` to skip a frame.
    // [UnityTest]
    // public IEnumerator ObjectThrowerTestsWithEnumeratorPasses()
    // {
    //     // Use the Assert class to test conditions.
    //     // Use yield to skip a frame.
    //     yield return null;
    // }
}
