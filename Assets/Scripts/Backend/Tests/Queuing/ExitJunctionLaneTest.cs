using NUnit.Framework;
using Assets.Scripts.Backend.Engine;
using Assets.Scripts.Backend.Queuing;
using Assets.Scripts.Backend.Vehicle;
using System.Threading.Tasks;
using System.Collections.Generic;

/// <summary>
/// cars that enter an exit lane dont do any queuing
/// </summary>
public class ExitJunctionLaneTest
{
    [Test]
    public void QueueManyDequeueManyLengthZero()
    {
        var engine = new Engine(10);
        var lane = new ExitJunctionLane(engine);

        for (int i = 0; i < 300; ++i)
        {
            lane.VehicleEnter(engine.CreateVehicle<Car>());
        }

        for (int i = 0; i < 300; ++i)
        {
            Assert.AreEqual(lane.VehicleExit().Item1, null);
        }

        Assert.AreEqual(lane.GetQueueLength(), 0);
    }

    [Test]
    public async Task ConcurrentQueueManySequentialDequeueManyLengthZero()
    {
        var engine = new Engine(10);
        var lane = new ExitJunctionLane(engine);

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
            Assert.AreEqual(lane.VehicleExit().Item1, null);
        }

        Assert.AreEqual(0, lane.GetQueueLength());
    }
}