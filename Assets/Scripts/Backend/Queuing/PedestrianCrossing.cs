using System.Collections.Generic;

namespace Assets.Scripts.Backend.Queuing
{
    public class PedestrianCrossing
    {
        private List<uint> WaitingPedestrians;
        private Engine.Engine Engine;

        public PedestrianCrossing(Engine.Engine engine)
        {
            Engine = engine;
            WaitingPedestrians = new List<uint>();
        }

        /// <summary>
        /// Add a pedestrian to wait at crossing (could be either side)
        /// </summary>
        /// <returns>Number of waiting pedestrians</returns>
        public int PedestrianEnter()
        {
            WaitingPedestrians.Add(Engine.SimulationTime);
            return WaitingPedestrians.Count;
        }

        public (List<uint>, uint) ReleasePedestrians()
        {
            return (WaitingPedestrians, Engine.SimulationTime);
        }
    }
}