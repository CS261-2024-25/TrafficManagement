
using System.Collections.Generic;
using System;
using System.Linq;

public class Demo 
{
    //Settings
    public float simulationDuration = 3600f;
    private DateTime startTime;

    // Traffic direction configurations
    private List<DirectionConfig> directions;
    


    //Pedestrian Crossing data
    public bool IsPedestrianCrossing = false;
    public float PedestrianCrossingTime = 30f;
    public float  CrossingRequestsPerHour = 300f;
    private float NextPedestrianCrossingTime;

    //Traffic Light data

    private int currPhaseIndex = 0;
    private DateTime phaseEndTime;
    private bool phaseInitialised = false;
    
    

    // Output
    private Metrics metrics;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    private void InitSimulation()
    {
        startTime = DateTime.Now;
        //Not finalised
        directions = new List<DirectionConfig>
        {
            new DirectionConfig
            {
                Name = "Northbound",
                IncomingVph = 300,
                road = new Road{numLanes = 3},
                TrafficPriority = 1,
                HasLeftTurnLane = true,
                HasBusCycleLane = true,
                BusCycleVph = 50,
                ExitNorth = 200,
                ExitEast = 50,
                ExitWest = 50
            },
            new DirectionConfig
            {
                Name = "Southbound",
                IncomingVph = 250,
                road = new Road{numLanes = 3},
                TrafficPriority = 2,
                HasLeftTurnLane = true,
                HasBusCycleLane = true,
                BusCycleVph = 30,
                ExitSouth = 150,
                ExitEast = 50,
                ExitWest = 50
            },
            new DirectionConfig
            {
                Name = "Eastbound",
                IncomingVph = 150,
                road = new Road{numLanes = 3},
                TrafficPriority = 2,
                HasLeftTurnLane = true,
                HasBusCycleLane = true,
                BusCycleVph = 10,
                ExitNorth = 50,
                ExitEast = 50,
                ExitSouth = 50
            },
            new DirectionConfig
            {
                Name = "Westbound",
                IncomingVph = 100,
                road = new Road{numLanes = 3},
                TrafficPriority = 4,
                HasLeftTurnLane = true,
                HasBusCycleLane = true,
                BusCycleVph = 25,
                ExitNorth = 25,
                ExitSouth = 25,
                ExitWest = 50
            }

        };

         

        metrics = new Metrics();

        

    }

    private void processTrafficLights(DateTime currentTime){
        //Note: Phases here are green lights for each lane. So if we are on phase 0, The traffic light is green for Northbound road and red for everything else.
        //used to make sure phaseEndTime does not increase indefinitely and so the second if block can run
        if(!phaseInitialised){
            phaseEndTime = currentTime.AddSeconds(calculatePhaseDuration(directions[currPhaseIndex].TrafficPriority));
            phaseInitialised = true;
        }

        if(currentTime>=phaseEndTime){
            currPhaseIndex = (currPhaseIndex + 1) % directions.Count; // switches to next phase 

            phaseEndTime = currentTime.AddSeconds(calculatePhaseDuration(directions[currPhaseIndex].TrafficPriority));
        }
        

                
                
            
    }

    private float calculatePhaseDuration(int priority){
        return 30f +(priority * 5f); // as the priority increases , the phase duration increases by 5 starting from 30 (priority 0)
    }

    

    private void RunSimulation()
    {
        DateTime currentTime = startTime;
        DateTime endTime = startTime.AddSeconds(simulationDuration);

        while(startTime < endTime) {

            foreach (var dir in directions){
                
                double Vps = dir.IncomingVph/3600;
                Random rnum = new Random();
                if(rnum.NextDouble() < Vps){
                    Vehicle car = new Vehicle();
                    car.EntryTime = currentTime;
                    int car_lane = rnum.Next(0,dir.road.numLanes-1);
                    dir.road.Lanes[car_lane].Enqueue(car);
                }
            }

            processTrafficLights(currentTime);
            
            

        }
    }

    //Helper classes

    //Input parameters
    public class DirectionConfig
    {
        public string Name { get; set; }  // e.g., "Northbound", "Southbound"
        public int IncomingVph { get; set; }  // Vehicles per hour arriving at junction

        //change 
        public Road road {get; set;}
        
        public int TrafficPriority { get; set; }  // 0-4 priority 

        public bool HasLeftTurnLane {get; set;}
        public bool HasBusCycleLane {get; set;}
        public int BusCycleVph {get; set;}
        
        // Vehicles exiting in different directions
        public int ExitNorth { get; set; }  
        public int ExitEast { get; set; }   
        public int ExitWest { get; set; }
        public int ExitSouth {get; set; }   
    }

    //change
    public class Road{

        private int number_lanes {get; set;} // Number of lanes
        public Dictionary<int,Queue<Vehicle>> Lanes= new Dictionary<int,Queue<Vehicle>>() ;

        public int numLanes {
            get{return number_lanes;}
            set{
                if(value<1 || value>5){
                     throw new ArgumentOutOfRangeException("Number of lanes must be between 1 and 5.");  //validation of number of lanes
                } else {
                    number_lanes = value;
                    InitLanes();
                }
            }
        }

        

        private void InitLanes(){
            Lanes.Clear();
            for(int i = 0; i < number_lanes; i++){
                Lanes.Add(i,new Queue<Vehicle>());
            }

        }

        
        
    }

    

    //Vehicle structure containing exit and entry time to calculate wait time.
    public class Vehicle
    {
        public DateTime EntryTime { get; set; }
        public DateTime ExitTime { get; set; }
    }


    
    public class Metrics
    {
        public Dictionary<string, List<double>> WaitTimes { get; } = new();

        public Dictionary<string, int> MaxQueueLengths { get; } = new();

        public void RecordWaitTime(string direction, double seconds)
        {
            if (!WaitTimes.ContainsKey(direction)) WaitTimes[direction] = new List<double>();
            WaitTimes[direction].Add(seconds);
        }

        public double GetAverageWaitTime(string direction)
        {
            if (WaitTimes.ContainsKey(direction) && WaitTimes[direction].Count > 0)
                return WaitTimes[direction].Average();
            return 0;
        }

        public double GetMaximumWaitTime(string direction)
        {
            if (WaitTimes.ContainsKey(direction) && WaitTimes[direction].Count > 0)
                return WaitTimes[direction].Max();
            return 0;
        }

        public void RecordMaxQueueLength(string direction, int queueLength)
        {
            if (!MaxQueueLengths.ContainsKey(direction)) MaxQueueLengths[direction] = 0;

            if (queueLength > MaxQueueLengths[direction])
            {
                MaxQueueLengths[direction] = queueLength;
            }
        }

        public int GetMaxQueueLength(string direction)
        {
            if (MaxQueueLengths.ContainsKey(direction))
                return MaxQueueLengths[direction];
            return 0;
        }
    }

}
