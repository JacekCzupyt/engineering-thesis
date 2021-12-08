using System.Collections;
using System.Collections.Generic;
using Game_Systems;
using Game_Systems.Utility;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class ObjectThrowerTests
{
    [Test]
    public void ReturnsStaticThrow() {
        const ThrowType throwType = ThrowType.Static;
        var throwMethod = ObjectThrower.Throw(throwType);
        Assert.AreEqual((ObjectThrower.ThrowMethod)ObjectThrower.StaticThrow, throwMethod);
    }
}
