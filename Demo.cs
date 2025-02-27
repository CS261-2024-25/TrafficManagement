
using System.Collections.Generic;
using System;
using System.Linq;

public class Demo 
{
    //Settings
    public float simulationDuration = 3600f;
    private DateTime startTime;

    // Traffic direction configurations
    private DirectionConfig[] directions;

    //Random
    private Random rnum = new Random();
    


    //Pedestrian Crossing data
    public bool[] IsPedestrianCrossing = new bool[4];
    public float PedestrianCrossingTime = 5f;
    public float  CrossingRequestsPerHour = 300f;
    private DateTime[] NextPedestrianCrossingTime = new DateTime[4];

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
        directions = new DirectionConfig[]
        {
            new DirectionConfig
            {
                Name = "Northbound",
                IncomingVph = 300,
                road = new Road{numLanes = 3, RoadLength = 40f},
                TrafficPriority = 1,
                HasLeftTurnLane = true,
                ExitNorth = 200,
                ExitEast = 50,
                ExitWest = 50,
                ExitSouth = 0
            },
            new DirectionConfig
            {
                Name = "Southbound",
                IncomingVph = 250,
                road = new Road{numLanes = 3, RoadLength = 35f},
                TrafficPriority = 2,
                HasLeftTurnLane = true,
                ExitSouth = 150,
                ExitEast = 50,
                ExitWest = 50,
                ExitNorth = 0
            },
            new DirectionConfig
            {
                Name = "Eastbound",
                IncomingVph = 150,
                road = new Road{numLanes = 3, RoadLength = 50f},
                TrafficPriority = 2,
                HasLeftTurnLane = true,
                ExitNorth = 50,
                ExitEast = 50,
                ExitSouth = 50,
                ExitWest = 0
            },
            new DirectionConfig
            {
                Name = "Westbound",
                IncomingVph = 100,
                road = new Road{numLanes = 3, RoadLength = 30f},
                TrafficPriority = 4,
                HasLeftTurnLane = true,
                ExitNorth = 25,
                ExitSouth = 25,
                ExitWest = 50,
                ExitEast = 0
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
            currPhaseIndex = (currPhaseIndex + 1) % directions.Length; // switches to next phase 

            phaseEndTime = currentTime.AddSeconds(calculatePhaseDuration(directions[currPhaseIndex].TrafficPriority));
        }
        

                
                
            
    }

    private float calculatePhaseDuration(int priority){
        return 30f +(priority * 5f); // as the priority increases , the phase duration increases by 5 starting from 30 (priority 0)
    }

    private string DetermineExitDirection(DirectionConfig dir)
    {
        var exits = new Dictionary<string, int> // gets each exit direction's vph
        { 
            {"North", dir.ExitNorth},
            {"South", dir.ExitSouth},
            {"East", dir.ExitEast},
            {"West", dir.ExitWest}
        };

        int total = exits.Values.Sum();
        int random = rnum.Next(0, total-1);
        int cumulative = 0;
        
        /*  
            Note: The idea here is that each exit direction has its own range on a graph and if the random number falls within its range then it is selected ,
            E.g N = 0 , S = 150 , E = 50 , W = 50 with random = 150. Graph range -> N = none , S = 0 - 149, E = 150 - 199 , W = 200 - 249
            150 !< 0,
            150 !< 150,
            150 < 200 
            So East is picked.
        */
        foreach (var exit in exits)
        {
            cumulative += exit.Value;
            if (random < cumulative)
                return exit.Key;
        }
        return ""; 
    }

    

    private void RunSimulation()
    {
        DateTime currentTime = startTime;
        DateTime endTime = startTime.AddSeconds(simulationDuration);

        while(currentTime < endTime) {

            
            //Enqueue vehicles
            foreach (var dir in directions){

                double Vps = dir.IncomingVph/3600; // convert to vheicles per second as we only queue in one car at a time

                if(rnum.NextDouble() < Vps){

                    Vehicle car = new Vehicle {EntryTime = currentTime, vehicleLength = rnum.NextDouble() * ((dir.road.RoadLength/10)-1) + 1};// creates car object and randomly generates a vehicleLength for it (logic for length can be chnaged)
                    int car_lane = rnum.Next(0,dir.road.numLanes-1); // randomly picks a lane to enqueue a car to.
                    Road _road = dir.road; 

                    if(_road.RoadLength <= _road.Lanes[car_lane].currLaneLength + car.vehicleLength){ // checks if when a new vehicle is added , it exceeds the length of the road.

                        _road.Lanes[car_lane].currLaneLength += car.vehicleLength; // updates the current length
                        _road.Lanes[car_lane].vehicleQueue.Enqueue(car); // adds car to queue
                        
                    }
                }
            }

            //update the traffic lights
            processTrafficLights(currentTime);

            
            DirectionConfig currDir = directions[currPhaseIndex];
            string Exit = DetermineExitDirection(currDir);
            int curr_lane = rnum.Next(0,currDir.road.numLanes-1);
            Queue<Vehicle> vehicleQueue = currDir.road.Lanes[curr_lane].vehicleQueue;



            //If block used to overrite the exit in the case that we have a left turn lane and we are working with the 1st lane.
            if(currDir.HasLeftTurnLane && curr_lane == 0){
                    Exit = currDir.LeftTurnDir;
            }

             if (vehicleQueue.Count > 0){
                Vehicle currVehicle = vehicleQueue.Dequeue();
            }

            
            

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
        
        
        // Vehicles exiting in different directions
        public int ExitNorth { get; set; }  
        public int ExitEast { get; set; }   
        public int ExitWest { get; set; }
        public int ExitSouth {get; set; }   

        private string _LeftTurnDir {get; set;}

        public string LeftTurnDir {
            get{return _LeftTurnDir;}
            set{
                switch(Name){
                    case "Northbound":
                        _LeftTurnDir = "West";
                        break;
                    case "Eastbound":
                        _LeftTurnDir = "North";
                        break;
                    case "Westbound":
                        _LeftTurnDir = "South";
                        break;
                    default : 
                        _LeftTurnDir = "East";
                        break;
                }
            }
        }
    }

    
    public class Road{

        private int number_lanes {get; set;} // Number of lanes
        public Dictionary<int,VehicleLane> Lanes= new Dictionary<int,VehicleLane>() ;

        public double RoadLength {get; set;}

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
                Lanes.Add(i,new VehicleLane{currLaneLength = 0});
            }

        }

        
        
    }

    public class VehicleLane {

        public double currLaneLength {get; set;}
        public Queue<Vehicle> vehicleQueue = new Queue<Vehicle>();
    }

    

    //Vehicle structure containing exit and entry time to calculate wait time.
    public class Vehicle
    {
        public double vehicleLength {get; set;}
        public DateTime EntryTime { get; set; }
        public DateTime ExitTime { get; set; }
        public string ExitDir {get; set;}
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
