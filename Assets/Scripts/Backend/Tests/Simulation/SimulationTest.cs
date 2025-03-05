using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Assets.Scripts.Util;
using Assets.Scripts.Backend.Engine;
using Assets.Scripts.Backend.Simulation;
class SimulationTest {

    [Test]
    public void RunSimulation_DoesNotThrowException(){
        var engine = new Engine(100);

        var north = new DirectionDetails(200, 50, 50, 2, 2, true, false);
        var east  = new DirectionDetails(150, 50, 50, 2, 2, true, false);
        var south = new DirectionDetails(50, 50, 50, 2, 2, true, false);
        var west  = new DirectionDetails(50, 25, 25, 2, 2, true, false);

        (CardinalDirection, double)[] priority = new (CardinalDirection, double)[]
            {
                (CardinalDirection.North, 0.5),
                (CardinalDirection.East,  0.75),
                (CardinalDirection.South, 1),
                (CardinalDirection.West,  1.75)
            };

        var inputParams = new InputParameters(north, east, south, west, priority);
        

        var simulation = new Simulation(engine, inputParams, 1000);
        Assert.DoesNotThrow(() => simulation.RunSimulation());

    }

    [Test]
    public void Simulation_ZeroFlow_ProcessNoVehicles()
    {
        
        var engine = new Engine(100); 
        var zeroDir = new DirectionDetails(0, 0, 0, 2, 2, false, false);
        (CardinalDirection, double)[] priority = {
            (CardinalDirection.North, 0.25),
            (CardinalDirection.East, 0.75),
            (CardinalDirection.South, 1.25),
            (CardinalDirection.West, 1.75)
        };
        var inputParams = new InputParameters(zeroDir, zeroDir, zeroDir, zeroDir, priority);

    
        var simulation = new Simulation(engine, inputParams, 1000); // 100 seconds
        
        var simulationResults = simulation.RunSimulation();

        var northresult = simulationResults.ResultWithDirection(CardinalDirection.North);
        var eastresult = simulationResults.ResultWithDirection(CardinalDirection.East);
        var southresult = simulationResults.ResultWithDirection(CardinalDirection.South);
        var westresult = simulationResults.ResultWithDirection(CardinalDirection.West);

       
        Assert.AreEqual(0, northresult.AverageWaitTime, "North AverageWaitTime should be zero");
        Assert.AreEqual(0, northresult.MaxWaitTime, "North MaxWaitTime should be zero");
        Assert.AreEqual(0, northresult.MaxQueueLength, "North PeakQueueLength should be zero");

        
        Assert.AreEqual(0, eastresult.AverageWaitTime, "East AverageWaitTime should be zero");
        Assert.AreEqual(0, eastresult.MaxWaitTime, "East MaxWaitTime should be zero");
        Assert.AreEqual(0, eastresult.MaxQueueLength, "East PeakQueueLength should be zero");

        
        Assert.AreEqual(0, southresult.AverageWaitTime, "South AverageWaitTime should be zero");
        Assert.AreEqual(0, southresult.MaxWaitTime, "South MaxWaitTime should be zero");
        Assert.AreEqual(0, southresult.MaxQueueLength, "South PeakQueueLength should be zero");

        
        Assert.AreEqual(0, westresult.AverageWaitTime, "West AverageWaitTime should be zero");
        Assert.AreEqual(0, westresult.MaxWaitTime, "West MaxWaitTime should be zero");
        Assert.AreEqual(0, westresult.MaxQueueLength, "West PeakQueueLength should be zero");
            
    }

    

    [Test]
    public void Simulation_PedestrianCrossngTriggered(){
         var engine = new Engine(100);
         var noFlow = new DirectionDetails(0, 0, 0, 2, 2, false, true);
         (CardinalDirection, double)[] priority = new (CardinalDirection, double)[]
            {
                (CardinalDirection.North, 0.5),
                (CardinalDirection.East,  0.75),
                (CardinalDirection.South, 1),
                (CardinalDirection.West,  1.75)
            };
        
        var inputParams = new InputParameters(noFlow, noFlow, noFlow, noFlow, priority);
        var simulation = new Simulation(engine, inputParams, 300);

        simulation.RunSimulation();
        
        Assert.IsTrue(simulation.PedestriansCrossedAtLeastOnce,"Pedestrians should have crossed at least once");

    }

    [Test]
    public void Simulation_TafficLightPhaseChangeTriggered(){
         var engine = new Engine(100);
         var noFlow = new DirectionDetails(0, 0, 0, 2, 2, false, true);
         (CardinalDirection, double)[] priority = new (CardinalDirection, double)[]
            {
                (CardinalDirection.North, 0.5),
                (CardinalDirection.East,  0.75),
                (CardinalDirection.South, 1),
                (CardinalDirection.West,  1.75)
            };
        
        var inputParams = new InputParameters(noFlow, noFlow, noFlow, noFlow, priority);
        var simulation = new Simulation(engine, inputParams, 300);

        simulation.RunSimulation();
        
        Assert.IsTrue(simulation.PhaseChangedAtLeastOnce,"Phase Should have chnaged at least once");
        
    }

     
}