namespace Assets.Scripts.Backend.Vehicle 
{
    using Assets.Scripts.Backend.Engine;
    public class Vehicle 
    {
        public double VehicleLength { get; }
        public double MinSpaceBehind { get; }
        public uint CreatedAt { get; }
        public uint VehicleId { get; }

        protected Vehicle(Engine engine, double vehicleLength, double minSpaceBehind, uint vehicleId) 
        {
            VehicleLength = vehicleLength;
            MinSpaceBehind = minSpaceBehind;
            CreatedAt = engine.SimulationTime;
            VehicleId = vehicleId;
        }
    }
}