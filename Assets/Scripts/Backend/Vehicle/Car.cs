namespace Assets.Scripts.Backend.Vehicle 
{
    using Assets.Scripts.Backend.Engine;
    public class Car: Vehicle {
        public Car(Engine engine, uint vehicleId) : base(engine, 2.0, 1.5, vehicleId)
        {

        }
    }
}