using System;
using System.Collections.Generic;
using Assets.Scripts.Backend.Queuing;
using JetBrains.Annotations;

namespace Assets.Scripts.Backend.Junction
{
    /// <summary>
    /// This is not concerned with whether there exists lanes to turn into.
    /// This is logic that needs to be implemented later down the line.
    /// 
    /// <para>
    /// This portion of the class is what should be used as an API REFERENCE
    /// </para>
    /// </summary>
    public partial class JunctionEntranceLaneSets
    {
        /// <summary>
        /// eish, use at your own risk
        /// </summary>
        private List<IntoJunctionLane> IntoJunctionLanes;
        /// <summary>
        /// eish, use at your own risk
        /// </summary>
        private List<ExitJunctionLane> ExitJunctionLanes;
        public readonly bool HasLeftTurn;
        public readonly bool HasRightTurn;

        public static JunctionEntranceLaneSets EmptyLaneSet(Engine.Engine engine)
        {
            return new JunctionEntranceLaneSets(
                engine: engine,
                intoJunctionLanes: 0,
                exitJunctionLanes: 0,
                hasLeftTurnLane: false,
                hasRightTurnLane: false
            );
        }

        public uint GetMaxWaitTime()
        {
            uint maxTime = 0;

            foreach (var lane in IntoJunctionLanes)
            {
                maxTime = Math.Max(maxTime, lane.MaximumWaitTime);
            }

            return maxTime;
        }

        public double GetAverageWaitTime()
        {
            double totalTime = 0;
            foreach (var lane in IntoJunctionLanes)
            {
                totalTime += lane.AverageWaitTime;
            }

            return (IntoJunctionLanes.Count == 0) ? 
                0 : totalTime / IntoJunctionLanes.Count;
        }

        public double GetPeakQueueLength()
        {
            double maxLength = 0;
            foreach (var lane in IntoJunctionLanes)
            {
                maxLength = Math.Max(maxLength, lane.PeakQueueLength);
            }

            return maxLength;
        }

        /// <summary>
        /// Enter a vehicle to a valid left turning lane
        /// </summary>
        /// <param name="vehicle"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Thrown if it is not possible to turn this direction</exception>
        public double VehicleEnterForLeftTurn(Vehicle.Vehicle vehicle)
        {
            if (HasLeftTurn)
            {
                return IntoJunctionLanes[0].VehicleEnter(vehicle);
            }
            else if (HasRightTurn && IntoJunctionLanesCount() > 1)
            {
                return IntoJunctionLanes[0].VehicleEnter(vehicle);
            }
            else
            {
                throw new InvalidOperationException(
                    $"No lanes exist to turn left from for vehicle {vehicle.VehicleId}"
                );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vehicle"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Thrown if it is not possible to turn this direction</exception>
        public double VehicleEnterForRightTurn(Vehicle.Vehicle vehicle)
        {
            if (HasRightTurn)
            {
                return IntoJunctionLanes[IntoJunctionLanesCount() - 1].VehicleEnter(vehicle);
            }
            else if (HasLeftTurn && IntoJunctionLanesCount() > 1)
            {
                return IntoJunctionLanes[IntoJunctionLanesCount() - 1].VehicleEnter(vehicle);
            }
            else
            {
                throw new InvalidOperationException(
                    $"No lanes exist to turn right from for vehicle {vehicle.VehicleId}"
                );
            }
        }

        /// <summary>
        /// Enter a vehicle into a lane set going forward
        /// </summary>
        /// <param name="vehicle"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Thrown if there are no routes to go forward from</exception>
        public double VehicleEnterForForward(Vehicle.Vehicle vehicle)
        {
            if (HasRightTurn && HasLeftTurn && IntoJunctionLanesCount() <= 2)
            {
                throw new InvalidOperationException(
                    $"No lanes exist to go forward from for vehicle {vehicle.VehicleId}"
                );
            }
            else if (IntoJunctionLanesCount() == 1 && (HasRightTurn || HasLeftTurn))
            {
                throw new InvalidOperationException(
                    $"No lanes exist to go forward from for vehicle {vehicle.VehicleId}"
                );
            }
            else if (HasRightTurn && HasLeftTurn)
            {
                return UnsafeVehicleEnterLanesWithRange(vehicle, 1, IntoJunctionLanesCount() - 1);
            }
            else if (HasLeftTurn)
            {
                return UnsafeVehicleEnterLanesWithRange(vehicle, 1, IntoJunctionLanesCount());
            }
            else if (HasRightTurn)
            {
                return UnsafeVehicleEnterLanesWithRange(vehicle, 0, IntoJunctionLanesCount() - 1);
            }
            else
            {
                return UnsafeVehicleEnterLanesWithRange(vehicle, 0, IntoJunctionLanesCount());
            }
        }

        public List<Vehicle.Vehicle> VehicleExitForLeftTurn()
        {
            if (
                HasLeftTurn || 
                (HasRightTurn && IntoJunctionLanesCount() > 1)
            ) {
                var exited = new List<Vehicle.Vehicle>();
                var vehicle = IntoJunctionLanes[0].VehicleExit().Item1;

                if (vehicle != null)
                {
                    exited.Add(vehicle);
                }

                return exited;
            }
            else
            {
                throw new InvalidOperationException(
                    "No lanes exist to exit right from."
                );
            }
        }

        public List<Vehicle.Vehicle> VehicleExitForForward()
        {
            if (HasRightTurn && HasLeftTurn && IntoJunctionLanesCount() <= 2)
            {
                throw new InvalidOperationException(
                    "No lanes exist to go forward from"
                );
            }
            else if (IntoJunctionLanesCount() == 1 && (HasRightTurn || HasLeftTurn))
            {
                throw new InvalidOperationException(
                    "No lanes exist to go forward from"
                );
            }
            else if (HasRightTurn && HasLeftTurn)
            {
                return UnsafeVehicleExitLanesWithRange(1, IntoJunctionLanesCount() - 1);
            }
            else if (HasLeftTurn)
            {
                return UnsafeVehicleExitLanesWithRange(1, IntoJunctionLanesCount());
            }
            else if (HasRightTurn)
            {
                return UnsafeVehicleExitLanesWithRange(0, IntoJunctionLanesCount() - 1);
            }
            else
            {
                return UnsafeVehicleExitLanesWithRange(0, IntoJunctionLanesCount());
            }
        }

        public List<Vehicle.Vehicle> VehicleExitForRightTurn()
        {
            if (
                HasRightTurn || (HasLeftTurn && IntoJunctionLanesCount() > 1)
            ) {
                var exited = new List<Vehicle.Vehicle>();
                var vehicle = IntoJunctionLanes[0].VehicleExit().Item1;

                if (vehicle != null)
                {
                    exited.Add(vehicle);
                }

                return exited;
            }
            else
            {
                throw new InvalidOperationException(
                    "No lanes exist to exit left from."
                );
            }
        }

        public int IntoJunctionLanesCount()
        {
            return IntoJunctionLanes.Count;
        }

        public int ExitJunctionLanesCount()
        {
            return ExitJunctionLanes.Count;
        }

        /// <summary>
        /// Ensures equal queue length approximately per lane
        /// </summary>
        /// <param name="vehicle">Vehicle to add</param>
        /// <param name="l">Lower bound lane index</param>
        /// <param name="r">1 + upper bound lane index</param>
        /// <returns>Queue length after insertion</returns>
        private double UnsafeVehicleEnterLanesWithRange(Vehicle.Vehicle vehicle, int l, int r)
        {
            IntoJunctionLane bestLane = IntoJunctionLanes[l];
            double bestQueueLength = bestLane.GetQueueLength();

            for (int i = l + 1; i < r; ++i)
            {
                double newQueueLength = IntoJunctionLanes[i].GetQueueLength();
                if (newQueueLength < bestQueueLength)
                {
                    bestQueueLength = newQueueLength;
                    bestLane = IntoJunctionLanes[i];
                }
            }

            return bestLane.VehicleEnter(vehicle);
        }

#nullable enable
        private List<Vehicle.Vehicle> UnsafeVehicleExitLanesWithRange(int l, int r)
        {
            var exited = new List<Vehicle.Vehicle>();

            for (int i = l + 1; i < r; ++i)
            {
                var vehicle = IntoJunctionLanes[i].VehicleExit().Item1;

                if (vehicle != null)
                {
                    exited.Add(vehicle);
                }
            }

            return exited;
        }
#nullable disable
    }
}