namespace Assets.Scripts.Backend.Queuing {
    using System.Collections.Generic;
    using Assets.Scripts.Backend.Vehicle;

    class Lane 
    {
        private Queue<Vehicle> VehicleQueue;

        public double GetQueueLength()
        {
            double total = 0;

            foreach( var vehicle in VehicleQueue )
            {
                total += vehicle.VehicleLength + vehicle.MinSpaceBehind;
            }

            return total;
        }


    }
}