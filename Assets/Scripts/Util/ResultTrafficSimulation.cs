using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Assets.Scripts.Util
{
    [Serializable]
    public class ResultTrafficSimulation
    {
        private ResultJunctionEntrance[] results;

        // these exist to serialize in a nice format
        [JsonProperty]
        private ResultJunctionEntrance NorthResult;
        [JsonProperty]
        private ResultJunctionEntrance EastResult;
        [JsonProperty]
        private ResultJunctionEntrance SouthResult;
        [JsonProperty]
        private ResultJunctionEntrance WestResult;

        private static readonly int NorthIndex = (int) CardinalDirection.North % 4;
        private static readonly int EastIndex = (int) CardinalDirection.East % 4;
        private static readonly int SouthIndex = (int) CardinalDirection.South % 4;
        private static readonly int WestIndex = (int) CardinalDirection.West % 4;

        public ResultTrafficSimulation(
            ResultJunctionEntrance northResult,
            ResultJunctionEntrance eastResult,
            ResultJunctionEntrance southResult,
            ResultJunctionEntrance westResult
        ) {
            results = new ResultJunctionEntrance[4];

            results[NorthIndex] = northResult;
            results[EastIndex] = eastResult;
            results[SouthIndex] = southResult;
            results[WestIndex] = westResult;
            NorthResult = northResult;
            EastResult = eastResult;
            WestResult = westResult;
            SouthResult = southResult;
        }

        public ResultJunctionEntrance ResultWithDirection(CardinalDirection direction)
        {
            return results[(int) direction % 4];
        }

        /// <summary>
        /// Dean's junction efficiency score
        /// </summary>
        /// <param name="direction">Direction you want to check</param>
        /// <param name="w_1">Coefficient of AverageWait</param>
        /// <param name="w_2">Coefficient of MaxWait</param>
        /// <param name="w_3">Coefficient of MaxQueueLength</param>
        /// <returns>Junction Efficiency score</returns>
        public double EfficiencyWithDirection(
            CardinalDirection direction,
            double w_1,
            double w_2,
            double w_3
        ) {
            return ResultWithDirection(direction)
                .getJunctionEfficiency(w_1, w_2, w_3);
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context) {
            results = new ResultJunctionEntrance[4];

            results[NorthIndex] = NorthResult;
            results[EastIndex] = EastResult;
            results[SouthIndex] = SouthResult;
            results[WestIndex] = WestResult;
        }
    }

    [Serializable]
    public class ResultJunctionEntrance
    {
        [JsonProperty]
        public readonly double MaxWaitTime;
        [JsonProperty]
        public readonly double AverageWaitTime;
        [JsonProperty]
        public readonly double MaxQueueLength;

        public ResultJunctionEntrance(
            double maxWaitTime, 
            double averageWaitTime, 
            double maxQueueLength
        ) {
            MaxWaitTime = maxWaitTime;
            AverageWaitTime = averageWaitTime;
            MaxQueueLength = maxQueueLength;
        }

        /// <summary>
        /// Dean's junction efficiency score
        /// </summary>
        /// <param name="w_1">Coefficient of AverageWait</param>
        /// <param name="w_2">Coefficient of MaxWait</param>
        /// <param name="w_3">Coefficient of MaxQueueLength</param>
        /// <returns>Junction Efficiency score</returns>
        public double getJunctionEfficiency(
            double w_1,
            double w_2,
            double w_3
        ) {
            return
                w_1 * (1.0 / AverageWaitTime) +
                w_2 * (1.0 / MaxWaitTime) + 
                w_3 * (1.0 / MaxQueueLength);
        }
    }
}