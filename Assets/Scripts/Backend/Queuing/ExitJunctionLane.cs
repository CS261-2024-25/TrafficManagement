namespace Assets.Scripts.Backend.Queuing
{
    /// <summary>
    /// Exclusive to coursework, so sealed. This kind of abstraction is 
    /// only really relevant when we talk about a single junction system.
    /// </summary>
    public sealed class ExitJunctionLane : Lane
    {        
        public ExitJunctionLane(Engine.Engine engine) : base(engine) {}

        /// <summary>
        /// Any vehicle that enters the lane is by default immediately exited.
        /// </summary>
        /// <param name="vehicle">Vehicle to enter the lane</param>
        /// <returns>If this is nonzero, something is wrong with the lane.</returns>
        public override double VehicleEnter(Vehicle.Vehicle vehicle)
        {
            var _ = base.VehicleEnter(vehicle);

            // Really this should only ever be 0
            return VehicleExit().Item2;
        }
    }
}