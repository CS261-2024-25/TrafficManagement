using NUnit.Framework;
using Assets.Scripts.Util;
using System.Threading;
using System;
using UnityEngine;
using System.Collections.Generic;

public class EngineTest
{
    [Test]
    public void EnumsHaveDifferentValuesModFour()
    {
        var dirSet = new HashSet<int>();

        dirSet.Add((int) CardinalDirection.North % 4);
        dirSet.Add((int) CardinalDirection.East % 4);
        dirSet.Add((int) CardinalDirection.South % 4);
        dirSet.Add((int) CardinalDirection.West % 4);

        Assert.True(dirSet.Count == 4);
    }
}
