namespace Assets.Scripts.Backend.Queuing {
    using System;
    using System.Collections.Generic;
    using Assets.Scripts.Backend.Vehicle;

    abstract class Lane 
    {
        private Queue<(Vehicle, uint)> VehicleQueue;
        public double PeakQueueLength { get; private set; }
        protected Engine.Engine Engine;

        protected Lane(Engine.Engine engine)
        {
            VehicleQueue = new Queue<(Vehicle, uint)>();
            PeakQueueLength = 0;
            Engine = engine;
        }

        public double GetQueueLength()
        {
            double total = 0;

            foreach( var vehicle in VehicleQueue )
            {
                total += vehicle.Item1.VehicleLength + vehicle.Item1.MinSpaceBehind;
            }

            return total;
        }

        /// <summary>
        /// Returns current queue length after adding vehicle
        /// </summary>
        public virtual double VehicleEnter(Vehicle vehicle)
        {
            VehicleQueue.Enqueue((vehicle, Engine.SimulationTime));

            var currQueueLength = GetQueueLength();
            PeakQueueLength = Math.Max(PeakQueueLength, currQueueLength);

            return currQueueLength;
        }

        public virtual (Vehicle?, double) VehicleExit()
        {
            if (VehicleQueue.Count > 0)
            {
                var leavingVehicle = VehicleQueue.Dequeue();
                return (leavingVehicle.Item1, GetQueueLength());
            }
            else
            {
                return (null, GetQueueLength());
            }
        }
    }
}