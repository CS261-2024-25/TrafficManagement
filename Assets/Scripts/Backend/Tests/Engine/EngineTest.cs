using NUnit.Framework;
using Assets.Scripts.Backend.Engine;
using System.Threading;
using System;
using UnityEngine;

public class EngineTest
{
    [Test]
    public void EngineTickCorrect()
    {
        var ticksPerSecond = 50;
        var sleepTimeMs = 500;
        Engine engine = new Engine(ticksPerSecond);
        engine.StartEngine();
        Thread.Sleep(sleepTimeMs);

        var expectedTicks = Convert.ToInt32(
            ticksPerSecond * (sleepTimeMs / 1000.0)
        );
        
        var timeAfter = engine.SimulationTime;
        Debug.Log($"expected: {expectedTicks}, curr: {timeAfter}");
        
        // some margin of error
        Assert.True(timeAfter > expectedTicks - 3);
        Assert.True(timeAfter < expectedTicks + 3);
        engine.StopEngine();
    }
}
