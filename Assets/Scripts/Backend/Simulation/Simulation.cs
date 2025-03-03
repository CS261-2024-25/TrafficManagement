namespace Assets.Scripts.Backend.Simulation 
{
    using System.Collections.Generic;
    using System;
    using System.Linq;
    
    using Assets.Scripts.Backend.Engine;
    using Assets.Scripts.Util;
    using Assets.Scripts.Backend.Queuing;
    
    public class Simulation {

        
        
        private DirectionConfig[] Directions = new DirectionConfig[4];
        protected Engine Engine;
        private uint SimulationDuration;

        //Traffic Light data
        private int currPhaseIndex = 0;
        private uint phaseEndTime;
        private bool phaseInitialised = false;

        //Random
        private Random rnum = new Random();

        //Pedestrian crossing data
        private PedestrianCrossing[] PedestrianCrossings = new PedestrianCrossing[4]; 
        private uint[] NextPedestrianCrossingTime = new uint[4]; 
        private bool[] IsPedestrianCrossing = new bool[4]; 
        private uint PedestrianCrossingDuration = 5; 
        private float CrossingRequestsPerHour = 300f; 
        

        public Simulation(Engine engine, InputParameters dirInfo, uint simulationDuration){


            if (engine == null)
            {
                throw new ArgumentNullException(nameof(engine), "Engine cannot be null.");
            }

            this.Engine = engine;

            

            
            Directions = new DirectionConfig[]
            {
                new DirectionConfig(CardinalDirection.South)
                {
                    boundDir = dirInfo.Northbound,
                    road = new Road(engine) 
                    {
                        numLanesInbound = dirInfo.Northbound.LaneCountInbound, 
                        numLanesOutbound = dirInfo.Northbound.LaneCountOutbound,
                        TrafficPriority = dirInfo.Priority.FirstOrDefault(p => p.Item1 == CardinalDirection.North).Item2 
                    }
                },
                new DirectionConfig(CardinalDirection.North)
                {
                    boundDir = dirInfo.Southbound,
                    road = new Road(engine) 
                    {
                        numLanesInbound = dirInfo.Southbound.LaneCountInbound, 
                        numLanesOutbound = dirInfo.Southbound.LaneCountOutbound,
                        TrafficPriority = dirInfo.Priority.FirstOrDefault(p => p.Item1 == CardinalDirection.South).Item2
                    }
                },
                new DirectionConfig(CardinalDirection.East)
                {
                    boundDir = dirInfo.Westbound,
                    road = new Road(engine) 
                    {
                        numLanesInbound = dirInfo.Westbound.LaneCountInbound, 
                        numLanesOutbound = dirInfo.Westbound.LaneCountOutbound,
                        TrafficPriority = dirInfo.Priority.FirstOrDefault(p => p.Item1 == CardinalDirection.West).Item2 
                    }
                },
                new DirectionConfig(CardinalDirection.West)
                {
                    boundDir = dirInfo.Eastbound,
                    road = new Road(engine) 
                    {
                        numLanesInbound = dirInfo.Eastbound.LaneCountInbound, 
                        numLanesOutbound = dirInfo.Eastbound.LaneCountOutbound,
                        TrafficPriority = dirInfo.Priority.FirstOrDefault(p => p.Item1 == CardinalDirection.East).Item2 
                    
                    }
                }    
            };

            SimulationDuration = simulationDuration;
            
        }

        public void RunSimulation(){

            Engine.StartEngine();

            uint endTime  = Engine.SimulationTime + SimulationDuration;

            InitPedestrians();

            while(Engine.SimulationTime < endTime){

                processPedestrians(Engine.SimulationTime);
                double random = rnum.NextDouble();

                foreach(var dir in Directions){

                    double Vps = (dir.boundDir.LeftFlow + dir.boundDir.ForwardFlow + dir.boundDir.RightFlow) / 3600;
                    
                    if(random < Vps){
                        int laneToQueue = dir.road.numLanesInbound > 0 ? rnum.Next(0, (int) dir.road.numLanesInbound-1) : 0;
                        dir.road.IJLanes[laneToQueue].VehicleEnter(generateVehicle());
                    }
                }

                processTrafficLights(Engine.SimulationTime);
                
                if (!IsPedestrianCrossing[currPhaseIndex]){
                    var currDir = Directions[currPhaseIndex];

                    int inboundLaneToDequeue = currDir.road.numLanesInbound > 0 ? rnum.Next(0, (int)currDir.road.numLanesInbound-1) : 0; 
                    
                    int outboundLaneToQueue = currDir.road.numLanesOutbound > 0 ? rnum.Next(0, (int)currDir.road.numLanesOutbound-1) : 0; 

                    int directionToExit = DetermineExitDirection(currDir, inboundLaneToDequeue);

                    

                    if(currDir.road.IJLanes[inboundLaneToDequeue].GetQueueLength() > 0){
                        var currVehicle = currDir.road.IJLanes[inboundLaneToDequeue].VehicleExit().Item1;

                        if(currVehicle != null && directionToExit != -1){
                            currDir.road.EJLanes[directionToExit].VehicleEnter(currVehicle);
                        }
                    }
                    
                    
                    
                }

                Engine.StopEngine();

            }

            
        }

        private void InitPedestrians(){
            for (int i = 0; i < 4; i++)
            {
                PedestrianCrossings[i] = new PedestrianCrossing(Engine);
                IsPedestrianCrossing[i] = Directions[i].boundDir.HasPedestrianCrossing;
                NextPedestrianCrossingTime[i] = (uint) (Engine.SimulationTime + rnum.NextDouble() * (3600 / CrossingRequestsPerHour));
            }
        }

        private void processPedestrians(uint currentTime){
            for (int i = 0; i < 4; i++)
            {
                if (!IsPedestrianCrossing[i] && currentTime >= NextPedestrianCrossingTime[i]) // if pedestrians are not crossing but its time to cross
                {
                    IsPedestrianCrossing[i] = true;
                    PedestrianCrossings[i].PedestrianEnter();
                    NextPedestrianCrossingTime[i] = currentTime + PedestrianCrossingDuration;
                }
                else if(IsPedestrianCrossing[i] && currentTime >= NextPedestrianCrossingTime[i]) // if pdestrians are crossing but the time is up
                {
                    IsPedestrianCrossing[i] = false;
                    PedestrianCrossings[i].ReleasePedestrians();
                    NextPedestrianCrossingTime[i] = currentTime + (uint)(rnum.NextDouble() * (3600 / CrossingRequestsPerHour));
                }
            }

        }


        private int DetermineExitDirection(DirectionConfig dir, int index)
        {

            if(dir.boundDir.HasLeftTurn && index == 0){ // to force the first lane to always turn left in the case we have a left turn lane and are working with the first lane

                for(int i = 0; i < Directions.Length; i++){
                    if(Directions[i].Name.CompareTo(dir.turnDirs.Left) == 0){
                        return i;
                    }
                }
                return -1;

            }else{

                var exits = new Dictionary<CardinalDirection, uint> // gets each exit direction's vph
                { 
                    {dir.turnDirs.Left, dir.boundDir.LeftFlow},
                    {dir.turnDirs.Right, dir.boundDir.RightFlow},
                    {dir.turnDirs.Forward, dir.boundDir.ForwardFlow}
                };

                long total = exits.Values.Sum(x => (long) x);
                if (total == 0) return -1;
                long random = (long)(rnum.NextDouble() * total);
                long cumulative = 0;
                
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
                    if (random < cumulative){

                        for(int i = 0; i < Directions.Length; i++)
                        {

                            if(Directions[i].Name.CompareTo(exit.Key) == 0){
                                return i;
                            }

                        }
                    }
                        
                }
                return -1; //for debugging purposes
            }
            
        }

        private Vehicle.Vehicle generateVehicle(){
            
            int random = rnum.Next(0,3);

            switch(random){
                case 0:
                    return Engine.CreateVehicle<Vehicle.Car>();
                case 1 :
                    return Engine.CreateVehicle<Vehicle.Truck>();
                case 2 :
                    return Engine.CreateVehicle<Vehicle.Bus>();
                default:
                    return Engine.CreateVehicle<Vehicle.Motorcycle>();
            }

            
            
                
        }

        private void processTrafficLights(uint currentTime){
            //Note: Phases here are green lights for each lane. So if we are on phase 0, The traffic light is green for Northbound road and red for everything else.
            //used to make sure phaseEndTime does not increase indefinitely and so the second if block can run
            if(!phaseInitialised){
                phaseEndTime = currentTime + calculatePhaseDuration(Directions[currPhaseIndex].TrafficPriority);
                phaseInitialised = true;
            }

            if(currentTime>=phaseEndTime){
                currPhaseIndex = (currPhaseIndex + 1) % Directions.Length; // switches to next phase 

                phaseEndTime = currentTime + calculatePhaseDuration(Directions[currPhaseIndex].TrafficPriority);
            }
            

                    
                    
                
        }

        private uint calculatePhaseDuration(double priority){
            return (uint) (30f +(priority * 5f)); // as the priority increases , the phase duration increases by 5 starting from 30 (priority 0)
        }


    }

}