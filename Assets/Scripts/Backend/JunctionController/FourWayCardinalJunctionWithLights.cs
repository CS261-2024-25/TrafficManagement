using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Assets.Scripts.Backend.Junction;
using Assets.Scripts.Util;

namespace Assets.Scripts.Backend.JunctionController
{
    /// <summary>
    /// Four way four exit cardinal junction. Engine controls start stop.
    /// Assumed average vehicle crossing time given <c>NO MERGE</c> is 10 seconds
    /// </summary>
    /// <para>
    /// Merges will cause proportional mean slowdown of rate. Turn of lights
    /// will wait 10s to clear all vehicles ready to leave. Lights on a segment
    /// stay green for 30s, with 10 intermittent leeway as vehicles can cross - ish.
    /// </para>
    /// <para>
    /// Each engine tick for this controller is one "second". Every 2 seconds a vehicle
    /// enters the junction/a vehicle set enters the junction
    /// </para>
    public class FourWayCardinalJunctionWithLights
    {
        private CardinalJunction Junction;
        private Engine.Engine Engine;
        private double[] Priority;

        private CardinalDirection CurrentlyAllows;
        private uint lastSwitchTime;
        private uint lastVehicleBufferedTime;
        private bool SendToBuffer;
        private CancellationTokenSource cancel;
        private double AverageTimeInJunction;
        private uint CountVehiclesPassedJunction;


        private Queue<(Vehicle.Vehicle, uint)> LeftBuffer;
        private Queue<(Vehicle.Vehicle, uint)> RightBuffer;
        private Queue<(Vehicle.Vehicle, uint)> ForwardBuffer;

        public FourWayCardinalJunctionWithLights(
            Engine.Engine engine,
            CardinalJunction junction,
            (CardinalDirection, double)[] priority
        ) {
            Engine = engine;
            Junction = junction;
            CurrentlyAllows = CardinalDirection.North;
            SendToBuffer = true;
            Priority = new double[] {0, 0, 0, 0};

            foreach (var prioPair in priority)
            {
                Priority[(int) prioPair.Item1 % 4] = prioPair.Item2;
            }

            LeftBuffer = new Queue<(Vehicle.Vehicle, uint)>();
            ForwardBuffer = new Queue<(Vehicle.Vehicle, uint)>();
            RightBuffer = new Queue<(Vehicle.Vehicle, uint)>();

            if (priority.Length != 4)
            {
                throw new ArgumentException("Priority array must be of length 4.");
            }

            double prioritySum = 0;
            foreach (var (_, priorityNum) in priority)
            {
                if (priorityNum < 0)
                {
                    throw new ArgumentException("Priority values cannot be negative");
                }

                prioritySum += priorityNum;
            }

            if (!prioritySum.AlmostEqualTo(4.0))
            {
                throw new ArgumentException("Priority array values must sum to 4.");
            }

            if (priority.Distinct().Count() == priority.Length)
            {
                throw new ArgumentException("Priority array must contain unique elements.");
            }

            cancel = new CancellationTokenSource();

            Task.Run(() => {
                EntranceToBuffer(cancel.Token, 2);
            });
            Task.Run(() => {
                ReleaseFromBuffer(cancel.Token, 2);
            });
            Task.Run(() => {
                LightSwitching(cancel.Token, 2);
            });

            lastSwitchTime = engine.SimulationTime;
            lastVehicleBufferedTime = engine.SimulationTime;
        }

#region running tooling
        private void EntranceToBuffer(CancellationToken token, uint ticksPerVehicle)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    lock(Engine.Tick)
                    {
                        Monitor.Wait(Engine.Tick);
                        var currTime = Engine.SimulationTime;
                        while(SendToBuffer && (int) currTime - (int) lastVehicleBufferedTime > (int) ticksPerVehicle)
                        {
                            var leftVehicles = Junction.Entrances[(int) CurrentlyAllows % 4].Item1.VehicleExitForLeftTurn();
                            var forwardVehicles = Junction.Entrances[(int) CurrentlyAllows % 4].Item1.VehicleExitForForward();
                            var rightVehicles = Junction.Entrances[(int) CurrentlyAllows % 4].Item1.VehicleExitForRightTurn();

                            foreach (var vehicle in leftVehicles)
                            {
                                LeftBuffer.Enqueue((vehicle, currTime));
                            }
                            foreach (var vehicle in forwardVehicles)
                            {
                                ForwardBuffer.Enqueue((vehicle, currTime));
                            }
                            foreach (var vehicle in rightVehicles)
                            {
                                RightBuffer.Enqueue((vehicle, currTime));
                            }

                            lastVehicleBufferedTime = currTime;
                        }
                    }
                }
            }
            catch (TaskCanceledException)
            {
                return;
            }
        }

        private void ReleaseFromBuffer(CancellationToken token, uint timeToRelease)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    lock(Engine.Tick)
                    {
                        Monitor.Wait(Engine.Tick);
                        var currTime = Engine.SimulationTime;
                        var newCount = CountVehiclesPassedJunction;
                        var newAverage = AverageTimeInJunction * newCount;
                        foreach (var buffer in Buffers())
                        {
                            while (buffer.Count > 0 && currTime - buffer.First().Item2 > timeToRelease)
                            {
                                var vehicleTime = buffer.Dequeue();
                                newCount++;
                                newAverage += currTime - vehicleTime.Item2;
                            }
                        }

                        CountVehiclesPassedJunction = newCount;
                        AverageTimeInJunction = newAverage / CountVehiclesPassedJunction;
                    }
                }
            }
            catch (TaskCanceledException)
            {
                return;
            }
        }

        private void LightSwitching(CancellationToken token, uint ticksBeforeTryingToSwitch)
        {
            try
            {
                while(!token.IsCancellationRequested)
                {
                    lock(Engine.Tick)
                    {
                        Monitor.Wait(Engine.Tick);
                        var currTime = Engine.SimulationTime;

                        if ((int) currTime - (int) lastSwitchTime > (int) ticksBeforeTryingToSwitch * Priority[(int) CurrentlyAllows % 4])
                        {
                            SendToBuffer = false;

                            var okToRelease = true;
                            foreach (var buffer in Buffers())
                            {
                                okToRelease = okToRelease && buffer.Count == 0;
                            }

                            if (okToRelease)
                            {
                                SendToBuffer = true;
                                SetNextDirection();
                                lastSwitchTime = currTime;
                            }
                        }
                    }
                }
            }
            catch (TaskCanceledException)
            {
                return;
            }
        }

        private IEnumerable<Queue<(Vehicle.Vehicle, uint)>> Buffers()
        {
            yield return LeftBuffer;
            yield return ForwardBuffer;
            yield return RightBuffer;
        }

        private void SetNextDirection()
        {
            switch (CurrentlyAllows)
            {
                case CardinalDirection.North:
                    CurrentlyAllows = CardinalDirection.East;
                    break;
                case CardinalDirection.East:
                    CurrentlyAllows = CardinalDirection.South;
                    break;
                case CardinalDirection.South:
                    CurrentlyAllows = CardinalDirection.West;
                    break;
                case CardinalDirection.West:
                    CurrentlyAllows = CardinalDirection.North;
                    break;
            }
        }

        /// <summary>
        /// Safely cancel asynchronous operations when GC occurs
        /// </summary>
        ~FourWayCardinalJunctionWithLights()
        {
            cancel.Cancel();
        }

#endregion
    }
}
