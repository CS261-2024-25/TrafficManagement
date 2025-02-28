using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Assets.Scripts.Backend.Junction;
using Assets.Scripts.Backend.Vehicle;
using Assets.Scripts.Util;

namespace Assets.Scripts.Backend.Engine
{
    public class EngineAllTurnsCardinalDeterministicFlow : Engine
    {
        private DirectionDetails[] DirectionDetails;
        private static readonly int NorthIndex = (int) CardinalDirection.North % 4;
        private static readonly int EastIndex = (int) CardinalDirection.East % 4;
        private static readonly int SouthIndex = (int) CardinalDirection.South % 4;
        private static readonly int WestIndex = (int) CardinalDirection.West % 4;

        public EngineAllTurnsCardinalDeterministicFlow(
            double ticksPerSecond,
            DirectionDetails northDetails,
            DirectionDetails eastDetails,
            DirectionDetails westDetails,
            DirectionDetails southDetails
        ) : base(ticksPerSecond) {
            if (northDetails == null || eastDetails == null || westDetails == null || southDetails == null)
            {
                throw new ArgumentException("Didn't expect null in constructor details");
            }

            DirectionDetails[NorthIndex] = northDetails;
            DirectionDetails[EastIndex] = eastDetails;
            DirectionDetails[WestIndex] = westDetails;
            DirectionDetails[SouthIndex] = southDetails;
        }

        public override void StartEngine()
        {
            base.StartEngine();

            // Task.Run(() => {
            //     SpawnLeftGoingVehicles<Car>(cancel, )
            // });
        }

        private void SpawnLeftGoingVehicles<T>(
            CancellationToken token,
            JunctionEntrance entrance,
            uint vehiclesPerHour
        ) where T: Vehicle.Vehicle {
            try 
            {
                double lastVehicleSpawned = SimulationTime;
                double vehiclesPerTick = Math.Ceiling((double) vehiclesPerHour / 3600);
                double ticksPerVehicle = 1 / vehiclesPerTick;
                while (!token.IsCancellationRequested)
                {
                    lock (Tick)
                    {
                        Monitor.Wait(Tick);

                        if (SimulationTime - lastVehicleSpawned >= Math.Ceiling(ticksPerVehicle))
                        {
                            for (int i = 0; i < vehiclesPerTick; ++i)
                            {
                                entrance.VehicleEnterForLeftTurn(CreateVehicle<T>());
                            }
                        }
                    }
                }
            }
            catch (TaskCanceledException)
            {
                return;
            }
        }

        private void SpawnForwardGoingVehicles<T>(
            CancellationToken token,
            JunctionEntrance entrance,
            uint vehiclesPerHour
        ) where T: Vehicle.Vehicle {
            try 
            {
                double lastVehicleSpawned = SimulationTime;
                double vehiclesPerTick = Math.Ceiling((double) vehiclesPerHour / 3600);
                double ticksPerVehicle = 1 / vehiclesPerTick;
                while (!token.IsCancellationRequested)
                {
                    lock (Tick)
                    {
                        Monitor.Wait(Tick);

                        if (SimulationTime - lastVehicleSpawned >= Math.Ceiling(ticksPerVehicle))
                        {
                            for (int i = 0; i < vehiclesPerTick; ++i)
                            {
                                entrance.VehicleEnterForForward(CreateVehicle<T>());
                            }
                        }
                    }
                }
            }
            catch (TaskCanceledException)
            {
                return;
            }
        }

        private void SpawnRightGoingVehicles<T>(
            CancellationToken token,
            JunctionEntrance entrance,
            uint vehiclesPerHour
        ) where T: Vehicle.Vehicle {
            try 
            {
                double lastVehicleSpawned = SimulationTime;
                double vehiclesPerTick = Math.Ceiling((double) vehiclesPerHour / 3600);
                double ticksPerVehicle = 1 / vehiclesPerTick;
                while (!token.IsCancellationRequested)
                {
                    lock (Tick)
                    {
                        Monitor.Wait(Tick);

                        if (SimulationTime - lastVehicleSpawned >= Math.Ceiling(ticksPerVehicle))
                        {
                            for (int i = 0; i < vehiclesPerTick; ++i)
                            {
                                entrance.VehicleEnterForRightTurn(CreateVehicle<T>());
                            }
                        }
                    }
                }
            }
            catch (TaskCanceledException)
            {
                return;
            }
        }
    }
}