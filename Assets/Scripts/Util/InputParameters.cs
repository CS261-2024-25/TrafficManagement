using System;
using System.Linq;
using Newtonsoft.Json;

namespace Assets.Scripts.Util
{
#region user input spec
    [Serializable]
    public class InputParameters 
    {
        [JsonProperty]
        public DirectionDetails Northbound { get; }
        [JsonProperty]
        public DirectionDetails Eastbound { get; }
        [JsonProperty]
        public DirectionDetails Southbound { get; }
        [JsonProperty]
        public DirectionDetails Westbound { get; }
        [JsonProperty]
        public (CardinalDirection, double)[] Priority { get; }

        /// <summary>
        /// Constructor for a somewhat valid set of input parameters.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if something constructed is unexpected</exception>
        public InputParameters
        (
            DirectionDetails northbound,
            DirectionDetails eastbound,
            DirectionDetails southbound,
            DirectionDetails westbound,
            (CardinalDirection, double)[] priority
        ) {
            Northbound = northbound;
            Eastbound = eastbound;
            Southbound = southbound;
            Westbound = westbound;

            if (priority.Length != 4)
            {
                throw new ArgumentException("Priority array must be of length 4.");
            }

            double prioritySum = 0;
            foreach (var (_, priorityNum) in priority)
            {
                if (priorityNum < 0)
                {
                    throw new ArgumentException("Priority values cannot be negative");
                }

                prioritySum += priorityNum;
            }

            if (!prioritySum.AlmostEqualTo(4.0))
            {
                throw new ArgumentException("Priority array values must sum to 4.");
            }

            if (priority.Distinct().Count() != priority.Length)
            {
                throw new ArgumentException("Priority array must contain unique elements.");
            }
    
            Priority = priority;
        }
    }
#endregion

#region directional type
    [Serializable]
    public class DirectionDetails
    {
        [JsonProperty]
        public uint LeftFlow { get; }
        [JsonProperty]
        public uint ForwardFlow { get; }
        [JsonProperty]
        public uint RightFlow { get; }
        [JsonProperty]
        public uint LaneCountOutbound { get; }
        [JsonProperty]
        public uint LaneCountInbound { get; }
        [JsonProperty]
        public bool HasLeftTurn { get; }
        [JsonProperty]
        public bool HasPedestrianCrossing { get; }

        public DirectionDetails
        (
            uint leftFlow,
            uint forwardFlow, 
            uint rightFlow,
            uint laneCountOutbound,
            uint laneCountInbound,
            bool hasLeftTurn,
            bool hasPedestrianCrossing
        ) {
            LeftFlow = leftFlow;
            ForwardFlow = forwardFlow;
            RightFlow = rightFlow;
            LaneCountOutbound = laneCountOutbound;
            LaneCountInbound = laneCountInbound;
            HasLeftTurn = hasLeftTurn;
            HasPedestrianCrossing = hasPedestrianCrossing;
        }
    }

#endregion
}

