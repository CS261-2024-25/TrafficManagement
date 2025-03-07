using System;
using Assets.Scripts.Backend.Queuing;

namespace Assets.Scripts.Backend.JunctionController
{
    /// <summary>
    /// This is concerned about what directions are valid to turn into
    /// </summary>
    public class JunctionEntrance
    {
        private Engine.Engine Engine;
        public readonly bool LeftValid;
        public readonly bool ForwardValid;
        public readonly bool RightValid;

        private JunctionEntranceLaneSets LaneSets;

        /// <summary>
        /// Constructor with state guarantees:
        /// <list type="bullet">
        ///     <item>
        ///         <description>If the LaneSet has exclusive turning, then the turn on the entrance must be valid</description>
        ///     </item>
        ///     <item>
        ///         <description>If the LaneSet has one lane, then it cannot be exclusive unless that is the only valid direction</description>
        ///     </item>
        ///     <item>
        ///         <description>If the LaneSet has two lanes, then if both are exclusive, forward is not valid</description>
        ///     </item>
        /// </list>
        /// </summary>
        /// <exception cref="ArgumentException">Throws if some lane set/junction property is invalid</exception>
        public JunctionEntrance(
            Engine.Engine engine,
            JunctionEntranceLaneSets junctionEntranceLaneSets,
            bool leftValid,
            bool forwardValid,
            bool rightValid
        ) {
            Engine = engine;

            if (
                junctionEntranceLaneSets.IntoJunctionLanesCount() == 0 &&
                (leftValid || rightValid || forwardValid)
            ) {
                throw new ArgumentException("Empty lane sets cannot have valid turnings");
            }
            else if (!leftValid && junctionEntranceLaneSets.HasLeftTurn)
            {
                throw new ArgumentException("Cannot have a left turn lane when left turns are not valid");
            }
            else if (!rightValid && junctionEntranceLaneSets.HasRightTurn)
            {
                throw new ArgumentException("Cannot have a right turn lane when right turns are not valid");
            }
            else if (
                junctionEntranceLaneSets.IntoJunctionLanesCount() == 2 &&
                forwardValid && rightValid && leftValid &&
                junctionEntranceLaneSets.HasLeftTurn &&
                junctionEntranceLaneSets.HasRightTurn
            ) {
                throw new ArgumentException(
                    "Incoming lane set has 2 lanes used for exclusive left and right, " +
                    "but the entrance wants to allow for forward turns too. Not enough lanes."
                );
            }
            else if (
                junctionEntranceLaneSets.IntoJunctionLanesCount() == 1 &&
                (leftValid && rightValid || forwardValid) && 
                junctionEntranceLaneSets.HasLeftTurn
            ) {
                throw new ArgumentException(
                    "Incoming lane set has 1 lane for exclusive left, " +
                    "but the entrance wants to allow for forward/right turns too. Not enough lanes."
                );
            }
            else if (
                junctionEntranceLaneSets.IntoJunctionLanesCount() == 1 &&
                (rightValid && leftValid || forwardValid) &&
                junctionEntranceLaneSets.HasRightTurn
            ) {
                throw new ArgumentException(
                    "Incoming lane set has 1 lane for exclusive right, " +
                    "but the entrance wants to allow for forward/left turns too. Not enough lanes."
                );
            }

            LeftValid = leftValid;
            RightValid = rightValid;
            ForwardValid = forwardValid;
            LaneSets = junctionEntranceLaneSets;
        }

        public static JunctionEntrance ClosedEntrance(Engine.Engine engine)
        {
            return new JunctionEntrance(
                engine: engine,
                JunctionEntranceLaneSets.EmptyLaneSet(engine),
                false,
                false,
                false
            );
        }

        /// <summary>
        /// Attempt to enter a vehicle to turn left
        /// </summary>
        /// <param name="vehicle"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Thrown if it is not possible to turn this direction</exception>
        public (double,int) VehicleEnterForLeftTurn(Vehicle.Vehicle vehicle) {
            try {
                if (!LeftValid)
                {
                    throw new InvalidOperationException(
                        "JunctionEntrance defines left as an invalid direction"  
                    );
                }
                return LaneSets.VehicleEnterForLeftTurn(vehicle);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Attempt to enter a vehicle to turn right
        /// </summary>
        /// <param name="vehicle"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Thrown if it is not possible to turn this direction</exception>
        public (double,int) VehicleEnterForRightTurn(Vehicle.Vehicle vehicle) {
            try {
                if (!RightValid)
                {
                    throw new InvalidOperationException(
                        "JunctionEntrance defines right as an invalid direction"  
                    );
                }
                return LaneSets.VehicleEnterForRightTurn(vehicle);
            } catch {
                throw;
            }
        }

        /// <summary>
        /// Attempt to enter a vehicle to turn left
        /// </summary>
        /// <param name="vehicle"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Thrown if it is not possible to go into this direction</exception>
        public (double,int) VehicleEnterForForward(Vehicle.Vehicle vehicle) {
            try {
                if (!ForwardValid)
                {
                    throw new InvalidOperationException(
                        "JunctionEntrance defines forward as an invalid direction"  
                    );
                }
                return LaneSets.VehicleEnterForForward(vehicle);
            } catch {
                throw;
            }
        }

        public double VehicleEnterToLeave(Vehicle.Vehicle vehicle, int j){

           return LaneSets.VehicleEnterToLeave(vehicle,j);

        }

        public Vehicle.Vehicle Exit(int i){
            return LaneSets.Exit(i);
        }

        public uint GetMaxWaitTime()
        {
            return LaneSets.GetMaxWaitTime();
        }

        public double GetAverageWaitTime()
        {
            return LaneSets.GetAverageWaitTime();
        }

        public double GetPeakQueueLength()
        {
            return LaneSets.GetPeakQueueLength();
        }

        public int IntoJunctionLanesCount()
        {
            return LaneSets.IntoJunctionLanesCount();
        }

        public int ExitJunctionLanesCount()
        {
            return LaneSets.ExitJunctionLanesCount();
        }
    }
}