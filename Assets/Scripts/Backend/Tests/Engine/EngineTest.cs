using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using Assets.Scripts.Backend.Engine;
using System.Threading;
using System.Threading.Tasks;

public class EngineTest
{
    [Test]
    public void EngineTickCorrect()
    {
        Engine engine = new Engine(10);
        engine.StartEngine();
        Thread.Sleep(1000);
        var timeAfter = engine.SimulationTime;
        Assert.True(timeAfter > 8);
        Assert.True(timeAfter < 12);
        engine.StopEngine();
    }
}
