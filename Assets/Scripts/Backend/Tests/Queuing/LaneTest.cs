using NUnit.Framework;
using Assets.Scripts.Backend.Engine;
using UnityEngine;
using Assets.Scripts.Backend.Queuing;
using Assets.Scripts.Backend.Vehicle;
using System.Threading.Tasks;
using System.Collections.Generic;

public class LaneTest
{
    [Test]
    public void QueueManyDequeueManyLengthZero()
    {
        var engine = new Engine(10);
        var lane = new IntoJunctionLane(engine);

        for (int i = 0; i < 300; ++i)
        {
            lane.VehicleEnter(engine.CreateVehicle<Car>());
        }

        for (int i = 0; i < 300; ++i)
        {
            lane.VehicleExit();
        }

        Assert.AreEqual(lane.GetQueueLength(), 0);
    }

    [Test]
    public async Task ConcurrentQueueManySequentialDequeueManyLengthZero()
    {
        var engine = new Engine(10);
        var lane = new IntoJunctionLane(engine);

        var entryTasks = new List<Task>();
        for (int i = 0; i < 300; ++i)
        {
            entryTasks.Add(Task.Run(() => {
                lane.VehicleEnter(engine.CreateVehicle<Car>());
            }));
        }

        await Task.WhenAll(entryTasks);

        for (int i = 0; i < 300; ++i)
        {
            lane.VehicleExit();
        }

        Assert.AreEqual(0, lane.GetQueueLength());
    }
}