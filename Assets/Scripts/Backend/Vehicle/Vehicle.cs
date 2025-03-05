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

        
    }

    

    
    public class Car : Vehicle
    {
        public Car(Engine engine, uint vehicleId) : base(engine, 4.5, 1.5, vehicleId) { }
    }

    public class Truck : Vehicle
    {
        public Truck(Engine engine, uint vehicleId) : base(engine, 10.0, 3.0, vehicleId) { }
    }

    public class Bus : Vehicle
    {
        public Bus(Engine engine, uint vehicleId) : base(engine, 12.0, 3.5, vehicleId) { }
    }

    public class Motorcycle : Vehicle
    {
        public Motorcycle(Engine engine, uint vehicleId) : base(engine, 2.0, 1.0, vehicleId) { }
    }

}