using System;
using System.Collections.Generic;
using Assets.Scripts.Backend.Queuing;

namespace Assets.Scripts.Backend.JunctionController
{
    /// <summary>
    /// This is not concerned with whether there exists lanes to turn into.
    /// This is logic that needs to be implemented later down the line.
    /// 
    /// <para>
    /// This portion of the class is what should be used as an API REFERENCE
    /// </summary>
    public partial class JunctionEntranceLaneSets
    {
        /// <summary>
        /// eish, use at your own risk
        /// </summary>
        public List<IntoJunctionLane> IntoJunctionLanes { get; private set; }
        /// <summary>
        /// eish, use at your own risk
        /// </summary>
        public List<ExitJunctionLane> ExitJunctionLanes { get; private set; }
        public readonly bool HasLeftTurn;

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

    }
}