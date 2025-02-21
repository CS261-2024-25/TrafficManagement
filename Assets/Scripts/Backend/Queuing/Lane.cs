namespace Assets.Scripts.Backend.Queuing {
    using System;
    using System.Collections.Generic;
    using Assets.Scripts.Backend.Vehicle;

    class Lane 
    {
        private Queue<Vehicle> VehicleQueue;
        public double PeakQueueLength { get; private set; }

        public Lane()
        {
            VehicleQueue = new Queue<Vehicle>();
            PeakQueueLength = 0;
        }

        public double GetQueueLength()
        {
            double total = 0;

            foreach( var vehicle in VehicleQueue )
            {
                total += vehicle.VehicleLength + vehicle.MinSpaceBehind;
            }

            return total;
        }

        /// <summary>
        /// Returns current queue length after adding vehicle
        /// </summary>
        public double VehicleEnter(Vehicle vehicle)
        {
            VehicleQueue.Enqueue(vehicle);

            var currQueueLength = GetQueueLength();
            PeakQueueLength = Math.Max(PeakQueueLength, currQueueLength);

            return currQueueLength;
        }

        public (Vehicle?, double) VehicleExit()
        {
            if (VehicleQueue.Count > 0)
            {
                var leavingVehicle = VehicleQueue.Dequeue();
                return (leavingVehicle, GetQueueLength());
            }
            else
            {
                return (null, GetQueueLength());
            }
        }
    }
}