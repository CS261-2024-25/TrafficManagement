using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Backend.Engine {
    public class Engine
    {
        public uint SimulationTime { get; private set; }
        private readonly double TicksPerSecond;
        private CancellationTokenSource cancel;

        public Engine(double ticksPerSecond)
        {
            SimulationTime = 0;
            TicksPerSecond = ticksPerSecond;
        }

        public void StartEngine()
        {
            Debug.Log("Start engine called");
            cancel = new CancellationTokenSource();
            Task.Run(async () =>
            {
                Debug.Log("inside closure");
                await UpdateTimeAsync(cancel.Token);
            });
        }

        public void StopEngine()
        {
            Debug.Log("Stop engine called");
            cancel.Cancel();
        }

        private async Task UpdateTimeAsync(CancellationToken token)
        {
            double tickIntervalMs = 1000.0 / TicksPerSecond;

            try
            {
                while (!token.IsCancellationRequested)
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(tickIntervalMs), token);
                    SimulationTime++;
                    Debug.Log("tick");
                }
            }
            catch (TaskCanceledException)
            {
                Debug.Log("Stopped engine");
            }
        }
    }
}