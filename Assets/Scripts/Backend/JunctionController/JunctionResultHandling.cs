using System;
using System.Collections.Generic;
using Assets.Scripts.Backend.Queuing;

namespace Assets.Scripts.Backend.JunctionController
{

    public class JunctionEntranceResult{
        // Private fields to hold  data
        private double maxWaitTime;
        private double averageWaitTime;
        private double maxQueueLength;

        // Constructor 
        public JunctionEntranceResult(double maxWait, double avgWait, double maxQueue)
        {
            this.maxWaitTime = maxWait;
            this.averageWaitTime = avgWait;
            this.maxQueueLength = maxQueue;
        }

        // Method to get maximum wait time
        public double getMaxWaitTime()
        {
            return maxWaitTime;
        }

        // Method to get average wait time
        public double getAverageWaitTime()
        {
            return averageWaitTime;
        }

        // Method to get maximum queue length
        public double getMaxQueueLength()
        {
            return maxQueueLength;
        }

    }

    public static class TrafficSimulationResult
    {
        // Static dictionary to hold junction results
        public static Dictionary<string, JunctionEntranceResult> Results = new Dictionary<string, JunctionEntranceResult>();

        // Populating   dictionary with  data-->Backend Integration
        //I have written the baseline code for population
        //Method design for populating dictionary subject to backend implementation 
        // Results.Add("Northbound",JunctionEntranceResultsNorth );
        // Results.Add("Southbound",JunctionEntranceResultsSouth );
        // Results.Add("Eastbound",JunctionEntranceResultsEast );
        // Results.Add("Westbound",JunctionEntranceResultsWest );
        
    }



}