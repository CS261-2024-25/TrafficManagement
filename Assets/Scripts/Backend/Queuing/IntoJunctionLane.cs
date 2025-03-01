


using System;
using System.Diagnostics.CodeAnalysis;

namespace Assets.Scripts.Backend.Queuing
{
    /// <summary>
    /// Exclusive to coursework, so sealed. This kind of abstraction is 
    /// only really relevant when we talk about a single junction system.
    /// </summary>
    public sealed class IntoJunctionLane : Lane
    {
        
        /// <summary>
        /// Time is in engine ticks
        /// </summary>
        public double AverageWaitTime { get; private set; }
        /// <summary>
        /// Time is in engine ticks
        /// </summary>
        public uint MaximumWaitTime { get; private set; }
        /// <summary>
        /// This only exists to aid with calculation of average wait time
        /// </summary>
        private uint VehiclesExited;
        
        public IntoJunctionLane(Engine.Engine engine) : base(engine) 
        {
            VehiclesExited = 0;
            AverageWaitTime = 0;
            MaximumWaitTime = 0;
        }

#nullable enable
        public override (Vehicle.Vehicle?, double) VehicleExit()
        {
            var (vehicle, queueLength) = base.VehicleExit();

            if (vehicle != null)
            {
                double totalCurrentWait = AverageWaitTime * VehiclesExited;
                uint thisVehicleWait = Engine.SimulationTime - vehicle.CreatedAt;
                MaximumWaitTime = Math.Max(thisVehicleWait, MaximumWaitTime);
                totalCurrentWait += thisVehicleWait;
                totalCurrentWait /= ++VehiclesExited;

                AverageWaitTime = totalCurrentWait;
            }

            return (vehicle, queueLength);
        }
#nullable disable
    }
}