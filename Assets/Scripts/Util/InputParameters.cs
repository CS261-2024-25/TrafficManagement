using System.Diagnostics;
using System.Linq;

namespace Assets.Scripts.Util
{
#region user input spec
    public class InputParameters 
    {
        public DirectionDetails Northbound { get; }
        public DirectionDetails Eastbound { get; }
        public DirectionDetails Southbound { get; }
        public DirectionDetails Westbound { get; }
        public CardinalDirection[] Priority { get; }

        public InputParameters
        (
            DirectionDetails northbound,
            DirectionDetails eastbound,
            DirectionDetails southbound,
            DirectionDetails westbound,
            CardinalDirection[] priority
        ) {
            Northbound = northbound;
            Eastbound = eastbound;
            Southbound = southbound;
            Westbound = westbound;

            Debug.Assert(
                priority.Length == 4, 
                "Priority array must be of length 4."
            );

            Debug.Assert(
                priority.Distinct().Count() == priority.Length, 
                "Priority array must contain unique elements."
            );

            Priority = priority;
        }
    }
#endregion

#region directional type
    public class DirectionDetails
    {
        public uint LeftFlow { get; }
        public uint ForwardFlow { get; }
        public uint RightFlow { get; }
        public uint StraightLaneCount { get; }
        public bool HasLeftTurn { get; }
        public bool HasPedestrianCrossing { get; }

        public DirectionDetails
        (
            uint leftFlow,
            uint forwardFlow, 
            uint rightFlow,
            uint straightLaneCount,
            bool hasLeftTurn,
            bool hasPedestrianCrossing
        ) {
            LeftFlow = leftFlow;
            ForwardFlow = forwardFlow;
            RightFlow = rightFlow;
            StraightLaneCount = straightLaneCount;
            HasLeftTurn = hasLeftTurn;
            HasPedestrianCrossing = hasPedestrianCrossing;
        }
    }

#endregion
}

