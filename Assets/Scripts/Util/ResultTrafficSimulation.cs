namespace Assets.Scripts.Util
{
    public class ResultTrafficSimulation
    {
        private ResultJunctionEntrance[] results;

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
    }

    public class ResultJunctionEntrance
    {
        public readonly double MaxWaitTime;
        public readonly double AverageWaitTime;
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