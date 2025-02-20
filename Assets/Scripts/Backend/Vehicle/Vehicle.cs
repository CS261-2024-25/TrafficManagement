namespace Assets.Scripts.Backend.Vehicle 
{
    using Assets.Scripts.Backend.Engine;
    public class Vehicle 
    {
        static uint counter = 0;
        public double VehicleLength { get; }
        public double MinSpaceBehind { get; }
        public uint CreatedAt { get; }
        public uint VehicleId { get; }

        protected Vehicle(double vehicleLength, double minSpaceBehind) 
        {
            VehicleLength = vehicleLength;
            MinSpaceBehind = minSpaceBehind;
            VehicleId = counter++;
        }
    }
}