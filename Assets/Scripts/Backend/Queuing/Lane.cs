namespace Assets.Scripts.Backend.Queuing {
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

        private double QueueLength;

        protected Lane(Engine.Engine engine)
        {
            VehicleQueue = new Queue<(Vehicle, uint)>();
            PeakQueueLength = 0;
            Engine = engine;
            Mutex = new Mutex();
            QueueLength = 0;
        }

        /// <summary>
        /// Implementation dependent, so explicit getter.
        /// 
        /// Currently runs in O(1).
        /// </summary>
        /// <returns>Queue length</returns>
        public double GetQueueLength()
        {
            return QueueLength;
        }

        /// <summary>
        /// Returns current queue length after adding vehicle.
        /// 
        /// O(1)
        /// </summary>
        public virtual double VehicleEnter(Vehicle vehicle)
        {
            Mutex.WaitOne();

            if (VehicleQueue.Count > 0)
            {
                QueueLength += VehicleQueue.Last().Item1.MinSpaceBehind;
            }
            
            VehicleQueue.Enqueue((vehicle, Engine.SimulationTime));
            QueueLength += vehicle.VehicleLength;
            Mutex.ReleaseMutex();

            var currQueueLength = GetQueueLength();
            PeakQueueLength = Math.Max(PeakQueueLength, currQueueLength);

            return currQueueLength;
        }

#nullable enable
        /// <summary>
        /// O(1)
        /// </summary>
        /// <returns></returns>
        public virtual (Vehicle?, double) VehicleExit()
        {
            Mutex.WaitOne();
            if (VehicleQueue.Count > 0)
            {
                var leavingVehicle = VehicleQueue.Dequeue();

                if  (VehicleQueue.Count >= 1)
                {
                    QueueLength -= leavingVehicle.Item1.MinSpaceBehind;
                }
                
                QueueLength -= leavingVehicle.Item1.VehicleLength;
                Mutex.ReleaseMutex();

                return (leavingVehicle.Item1, GetQueueLength());
            }
            else
            {
                Mutex.ReleaseMutex();
                return (null, GetQueueLength());
            }
        }
#nullable disable
    }
}