namespace Assets.Scripts.Backend.Vehicle 
{
    using System;
    using System.Collections.Generic;
    using Assets.Scripts.Backend.Engine;
    abstract public class Vehicle 
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

        //Dictionary of all possible vehicles
        public static readonly Dictionary<string, (double length, double spaceBehind)> VehicleTypes =
            new()
            {
                { "Car", (4.5, 1.5) },
                { "Truck", (10.0, 3.0) },
                { "Bus", (12.0, 3.5) },
                { "Motorcycle", (2.0, 1.0) }
            };
    }

    //Generic vehicle class so we can make use of the abstract vehicle class
    public class GenericVehicle : Vehicle
    {
        public GenericVehicle(Engine engine, double vehicleLength, double minSpaceBehind, uint vehicleId)
            : base(engine, vehicleLength, minSpaceBehind, vehicleId) { }
    }
}