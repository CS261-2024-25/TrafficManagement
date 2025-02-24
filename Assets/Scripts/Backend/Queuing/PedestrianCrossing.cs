using System;
using System.Collections.Generic;
using System.Threading;

namespace Assets.Scripts.Backend.Queuing
{
    public class PedestrianCrossing
    {
        private List<uint> WaitingPedestrians;
        private Engine.Engine Engine;
        private double AverageWait;
        private uint PeakWait;
        private uint PedestriansCrossed;

        /// <summary>
        /// Synchronisation primitive. To use, you must first
        /// lock the CrossingRequested with lock() keyword, and then
        /// call `Monitor.Wait(CrossingRequested)` to wait on it. This
        /// releases the lock.
        /// </summary>
        public readonly object CrossingRequested;

        public PedestrianCrossing(Engine.Engine engine)
        {
            Engine = engine;
            AverageWait = 0;
            PeakWait = 0;
            CrossingRequested = new object();
        }

        /// <summary>
        /// Add a pedestrian to wait at crossing (could be either side)
        /// </summary>
        /// <returns>Number of waiting pedestrians</returns>
        public int PedestrianEnter()
        {
            WaitingPedestrians.Add(Engine.SimulationTime);
            lock (CrossingRequested)
            {
                Monitor.Pulse(CrossingRequested);
            }
            return WaitingPedestrians.Count;
        }

        /// <summary>
        /// Release pedestrians at a crossing
        /// </summary>
        /// <returns>tuple (average wait, peak wait)</returns>
        public (double, uint) ReleasePedestrians()
        {
            var releaseTime = Engine.SimulationTime;
            var newAverage = AverageWait * PedestriansCrossed;
            foreach (var pedestrianSpawnTime in WaitingPedestrians)
            {
                var timeWaited = releaseTime - pedestrianSpawnTime;
                PeakWait = Math.Max(PeakWait, timeWaited);

                newAverage += releaseTime - pedestrianSpawnTime;
            }

            PedestriansCrossed += (uint) WaitingPedestrians.Count;
            AverageWait = (PedestriansCrossed != 0) ? 
                newAverage / PedestriansCrossed : 0;
            
            return (AverageWait, PeakWait);
        }
    }
}