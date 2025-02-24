namespace Assets.Scripts.Backend.Queuing {
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Assets.Scripts.Backend.Vehicle;

    /// <summary>
    /// Threadsafe lane
    /// </summary>
    public abstract class Lane 
    {
        private Queue<(Vehicle, uint)> VehicleQueue;
        public double PeakQueueLength { get; private set; }
        protected Engine.Engine Engine;
        private Mutex Mutex;

        protected Lane(Engine.Engine engine)
        {
            VehicleQueue = new Queue<(Vehicle, uint)>();
            PeakQueueLength = 0;
            Engine = engine;
            Mutex = new Mutex();
        }

        public double GetQueueLength()
        {
            Mutex.WaitOne();
            double total = 0;

            foreach( var vehicle in VehicleQueue )
            {
                total += vehicle.Item1.VehicleLength + vehicle.Item1.MinSpaceBehind;
            }

            Mutex.ReleaseMutex();
            return total;
        }

        /// <summary>
        /// Returns current queue length after adding vehicle
        /// </summary>
        public virtual double VehicleEnter(Vehicle vehicle)
        {
            Mutex.WaitOne();
            VehicleQueue.Enqueue((vehicle, Engine.SimulationTime));
            Mutex.ReleaseMutex();

            var currQueueLength = GetQueueLength();
            PeakQueueLength = Math.Max(PeakQueueLength, currQueueLength);

            return currQueueLength;
        }

#nullable enable
        public virtual (Vehicle?, double) VehicleExit()
        {
            if (VehicleQueue.Count > 0)
            {
                Mutex.WaitOne();
                var leavingVehicle = VehicleQueue.Dequeue();
                Mutex.ReleaseMutex();

                return (leavingVehicle.Item1, GetQueueLength());
            }
            else
            {
                return (null, GetQueueLength());
            }
        }
#nullable disable
    }
}