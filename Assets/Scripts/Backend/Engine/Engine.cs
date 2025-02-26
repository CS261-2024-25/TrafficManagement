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
        private uint VehicleId;

        public Engine(double ticksPerSecond)
        {
            SimulationTime = 0;
            TicksPerSecond = ticksPerSecond;
            VehicleId = 0;
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

        public T CreateVehicle<T>() where T : Vehicle.Vehicle
        {
            return (T)Activator.CreateInstance(typeof(T), this, VehicleId++);
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
                    Monitor.PulseAll(SimulationTime);
                }
            }
            catch (TaskCanceledException)
            {
                Debug.Log("Stopped engine");
            }
        }
    }
}