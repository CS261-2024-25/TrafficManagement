namespace Assets.Scripts.Backend.Queuing
{
    /// <summary>
    /// Exclusive to coursework, so sealed. This kind of abstraction is 
    /// only really relevant when we talk about a single junction system.
    /// </summary>
    public sealed class ExitJunctionLane : Lane
    {        
        public ExitJunctionLane(Engine.Engine engine) : base(engine) {}

        public override double VehicleEnter(Vehicle.Vehicle vehicle)
        {
            var _ = base.VehicleEnter(vehicle);

            // Really this should only ever be 0
            return VehicleExit().Item2;
        }
    }
}