namespace Assets.Scripts.Backend.Simulation 
{
    using System.Collections.Generic;
    using System;
    using System.Linq;
    using UnityEngine;
    using SysRandom = System.Random;
    
    using Assets.Scripts.Backend.Engine;
    using Assets.Scripts.Util;
    using Assets.Scripts.Backend.Queuing;
    using Assets.Scripts.Backend.JunctionController;
    
    public class Simulation {


        protected Engine Engine;

        
        private CardinalJunction Junction;



        private double[] priorityArr = new double[4];

        private uint SimulationDuration;

        private (uint,uint,uint)[] flows = new (uint,uint,uint)[4];

        

        //Random
        private SysRandom rnum = new SysRandom();

        //Traffic Light data
        private int currPhaseIndex = 0;
        private uint phaseEndTime;
        private bool phaseInitialised = false;

        //Pedestrian crossing data
        private PedestrianCrossing[] PedestrianCrossings = new PedestrianCrossing[4]; 
        private uint[] NextPedestrianCrossingTime = new uint[4]; 
        private bool[] IsPedestrianCrossing = new bool[4]; 
        private uint PedestrianCrossingDuration = 5; 
        private float CrossingRequestsPerHour = 300f; 


        //Testing Values
        public bool PedestriansCrossedAtLeastOnce;
        public bool PhaseChangedAtLeastOnce;
        
        public Simulation(Engine engine,InputParameters dirInfo, uint simulationDuration){

            Engine = engine;

            CardinalJunctionFactory JunctionFac = new CardinalJunctionFactory(Engine);

            Junction = JunctionFac
                .AddNorthEntrance
                (   
                    new JunctionEntrance
                    (
                        Engine,
                        new JunctionEntranceLaneSets
                        (
                            Engine,
                            dirInfo.Southbound.LaneCountInbound,
                            dirInfo.Southbound.LaneCountOutbound,
                            dirInfo.Southbound.HasLeftTurn,
                            false
                        ),
                        true, true, true
                    )
                    
                )
                .AddEastEntrance
                (
                    new JunctionEntrance
                    (
                        Engine,
                        new JunctionEntranceLaneSets
                        (
                            Engine,
                            dirInfo.Westbound.LaneCountInbound,
                            dirInfo.Westbound.LaneCountOutbound,
                            dirInfo.Westbound.HasLeftTurn,
                            false
                        ),
                        true, true, true
                    )
                    
                )
                .AddSouthEntrance
                (
                    new JunctionEntrance
                    (
                        Engine,
                        new JunctionEntranceLaneSets
                        (
                            Engine,
                            dirInfo.Northbound.LaneCountInbound,
                            dirInfo.Northbound.LaneCountOutbound,
                            dirInfo.Northbound.HasLeftTurn,
                            false
                        ),
                        true, true, true
                    )
                    
                )
                .AddWestEntrance
                (
                    new JunctionEntrance
                    (
                        Engine,
                        new JunctionEntranceLaneSets
                        (
                            Engine,
                            dirInfo.Eastbound.LaneCountInbound,
                            dirInfo.Eastbound.LaneCountOutbound,
                            dirInfo.Eastbound.HasLeftTurn,
                            false
                        ),
                        true, true, true
                    )
                    
                )
                .GenerateJunction();

                priorityArr[(int) CardinalDirection.North % 4] = dirInfo.Priority[(int) CardinalDirection.North % 4].Item2;
                priorityArr[(int) CardinalDirection.East % 4] = dirInfo.Priority[(int) CardinalDirection.East % 4].Item2;
                priorityArr[(int) CardinalDirection.South % 4] = dirInfo.Priority[(int) CardinalDirection.South % 4].Item2;
                priorityArr[(int) CardinalDirection.West % 4] = dirInfo.Priority[(int) CardinalDirection.West % 4].Item2;  

                flows[(int) CardinalDirection.North % 4] = (dirInfo.Southbound.LeftFlow,dirInfo.Southbound.RightFlow, dirInfo.Southbound.ForwardFlow);
                flows[(int) CardinalDirection.East % 4] = (dirInfo.Westbound.LeftFlow,dirInfo.Westbound.RightFlow, dirInfo.Westbound.ForwardFlow);
                flows[(int) CardinalDirection.South % 4] = (dirInfo.Northbound.LeftFlow,dirInfo.Northbound.RightFlow, dirInfo.Northbound.ForwardFlow);
                flows[(int) CardinalDirection.West % 4] = (dirInfo.Eastbound.LeftFlow,dirInfo.Eastbound.RightFlow, dirInfo.Eastbound.ForwardFlow);


                for (int i = 0; i < 4; i++)
                {
                    PedestrianCrossings[i] = new PedestrianCrossing(engine);
                    NextPedestrianCrossingTime[i] = (uint) (engine.SimulationTime + rnum.NextDouble() * (3600 / CrossingRequestsPerHour));
                }

                IsPedestrianCrossing[(int) CardinalDirection.North % 4] = dirInfo.Southbound.HasPedestrianCrossing;
                IsPedestrianCrossing[(int) CardinalDirection.East % 4] = dirInfo.Westbound.HasPedestrianCrossing;
                IsPedestrianCrossing[(int) CardinalDirection.South % 4] = dirInfo.Northbound.HasPedestrianCrossing;
                IsPedestrianCrossing[(int) CardinalDirection.West % 4] = dirInfo.Eastbound.HasPedestrianCrossing;

            

                SimulationDuration = simulationDuration;

                PedestriansCrossedAtLeastOnce = false;
                PhaseChangedAtLeastOnce = false;

        }

        /// <summary>
        /// Runs the simulation by first converting converting a uniformly distributed random number into an exponentially distributed time t which is then added to generate
        /// the next queue time. This method is also known as a poisson method and it ensures that queuing of vehicles in this context are independent of each other and random.
        /// This method goes on to model a junction and also dequeues vehicles depending on the traffic light signal returning the metrics calculated for each junction
        /// </summary>
        /// <returns>vehicle simulation results which is a container for each junctions results</returns>
        public ResultTrafficSimulation RunSimulation(){
            Engine.StartEngine();

            uint endTime  = Engine.SimulationTime + SimulationDuration;

            JunctionEntrance[] Entrances = Junction.GetEntrances();

            double[] nextArrivalTimes = new double[4];
            for (int i = 0; i < 4; i++){
                // Compute arrival rate (vehicles per second) for lane i 
                double lambda = (flows[i].Item1 + flows[i].Item2 + flows[i].Item3) / 3600.0;
                // To Avoid Division by 0
                nextArrivalTimes[i] = lambda > 0 
                    ? Engine.SimulationTime + (-Math.Log(rnum.NextDouble()) / lambda)
                    : double.MaxValue;
            }


            // Prepare queues to hold vehicles
            Queue<int>[] exitPathIndexes = new Queue<int>[4];
            Queue<int>[] intoPathIndexes = new Queue<int>[4];
            for (int i = 0; i < 4; i++){
                exitPathIndexes[i] = new Queue<int>();
                intoPathIndexes[i] = new Queue<int>();
            }

            while(Engine.SimulationTime < endTime){

                processPedestrians(Engine.SimulationTime);
                
                

                // For each lane, check if it's time for a vehicle arrival
                for (int i = 0; i < 4; i++){

                    double lambda = (flows[i].Item1 + flows[i].Item2 + flows[i].Item3) / 3600.0;

                    // Check if the current time (cast to double) meets or exceeds the scheduled arrival

                    if((double)Engine.SimulationTime >= nextArrivalTimes[i]){

                        int exitPath = DetermineExitDirection(i);

                        if(exitPath != -1){

                            int exitIndex = (i + exitPath + 1) % 4;

                            if(Entrances[exitIndex].ExitJunctionLanesCount() > 0){

                                exitPathIndexes[i].Enqueue(exitIndex);

                                if(exitPath == 0 && Entrances[i].LeftValid){

                                    intoPathIndexes[i].Enqueue(Entrances[i].VehicleEnterForLeftTurn(generateVehicle()).Item2);

                                } else if(exitPath == 1 && Entrances[i].ForwardValid){

                                    intoPathIndexes[i].Enqueue(Entrances[i].VehicleEnterForForward(generateVehicle()).Item2);

                                } else if(exitPath == 2 && Entrances[i].RightValid){

                                    intoPathIndexes[i].Enqueue(Entrances[i].VehicleEnterForRightTurn(generateVehicle()).Item2);

                                }
                            }

                            
                        }
                        // Schedule the next arrival for lane i using the  (poisson) exponential distribution.
                        nextArrivalTimes[i] = Engine.SimulationTime + (-Math.Log(rnum.NextDouble()) / lambda);
                    }
                }

                processTrafficLights(Engine.SimulationTime);

                // Process the current traffic phase if vehicles are waiting and pedestrian arent crossing
                if(!IsPedestrianCrossing[currPhaseIndex] 
                    && exitPathIndexes[currPhaseIndex].Count > 0 
                    && intoPathIndexes[currPhaseIndex].Count > 0){

                    var currDir = Entrances[currPhaseIndex];
                    int currExitIndex = exitPathIndexes[currPhaseIndex].Dequeue();
                    int currIntoIndex = intoPathIndexes[currPhaseIndex].Dequeue();
                    var exitDir = Entrances[currExitIndex];
                    if(currExitIndex != -1){
                        exitDir.VehicleEnterToLeave(currDir.Exit(currIntoIndex), rnum.Next(exitDir.ExitJunctionLanesCount()));
                    }
                }

                
            }

            Engine.StopEngine();

            ResultJunctionEntrance northresult = new ResultJunctionEntrance
            (
                Entrances[(int) CardinalDirection.North % 4].GetMaxWaitTime(),
                Entrances[(int) CardinalDirection.North % 4].GetAverageWaitTime(),
                Entrances[(int) CardinalDirection.North % 4].GetPeakQueueLength()
            );

            ResultJunctionEntrance eastresult = new ResultJunctionEntrance
            (
                Entrances[(int) CardinalDirection.East % 4].GetMaxWaitTime(),
                Entrances[(int) CardinalDirection.East % 4].GetAverageWaitTime(),
                Entrances[(int) CardinalDirection.East % 4].GetPeakQueueLength()
            );

            ResultJunctionEntrance southresult = new ResultJunctionEntrance
            (
                Entrances[(int) CardinalDirection.South % 4].GetMaxWaitTime(),
                Entrances[(int) CardinalDirection.South % 4].GetAverageWaitTime(),
                Entrances[(int) CardinalDirection.South % 4].GetPeakQueueLength()
            );

            ResultJunctionEntrance westresult = new ResultJunctionEntrance
            (
                Entrances[(int) CardinalDirection.West % 4].GetMaxWaitTime(),
                Entrances[(int) CardinalDirection.West % 4].GetAverageWaitTime(),
                Entrances[(int) CardinalDirection.West % 4].GetPeakQueueLength()
            );
            
            return new ResultTrafficSimulation(northresult,eastresult,southresult,westresult);
        }


        
        /// <summary>
        /// Updates pedestrian simulation
        /// </summary>
        /// <param name="currentTime">current simulation time</param>
        private void processPedestrians(uint currentTime){
            for (int i = 0; i < 4; i++)
            {
                if (!IsPedestrianCrossing[i] && currentTime >= NextPedestrianCrossingTime[i]) // if pedestrians are not crossing but its time to cross
                {
                    IsPedestrianCrossing[i] = true;
                    PedestrianCrossings[i].PedestrianEnter();
                    if(!PedestriansCrossedAtLeastOnce){
                        PedestriansCrossedAtLeastOnce = true;
                    }
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

        /// <summary>
        /// Gets corresponding direction to exit where in this case 0 represents a left , 1 represents a forward and 2 represents a right
        /// </summary>
        /// <param name="dirIndex">direction Index</param>
        /// <returns>direction to exit/returns>
        private int DetermineExitDirection(int dirIndex)
        {
                uint total = flows[dirIndex].Item1 + flows[dirIndex].Item2 + flows[dirIndex].Item3;
                if (total == 0) return -1;
                uint random = (uint)(rnum.NextDouble() * total);
                uint cumulative = 0;

                
                
                /*  
                    Note: The idea here is that each exit direction has its own range on a graph and if the random number falls within its range then it is selected ,
                    E.g N = 0 , S = 150 , E = 50 , W = 50 with random = 150. Graph range -> N = none , S = 0 - 149, E = 150 - 199 , W = 200 - 249
                    150 !< 0,
                    150 !< 150,
                    150 < 200 
                    So East is picked.
                */
                cumulative += flows[dirIndex].Item1;
                if(random < cumulative){
                    return 0;
                }else{
                    cumulative += flows[dirIndex].Item2;
                    if(random < cumulative){
                        return 1;
                    }else{
                        cumulative += flows[dirIndex].Item3;
                        if(random < cumulative){
                            return 2;
                        }
                    }
                }
                return -1;
                    
                        
                
            }
        
        /// <summary>
        /// Generates a random vehicle based on the number assigned to each 
        /// </summary>
        /// <returns>Generates a vehicle</returns>

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

        /// <summary>
        /// Updates traffic lights assuming currphaseindex is a green for that direction
        /// </summary>
        /// <param name="currentTime">current simulation time </param>
        private void processTrafficLights(uint currentTime){
            //Note: Phases here are green lights for each lane. So if we are on phase 0, The traffic light is green for Northbound road and red for everything else.
            //used to make sure phaseEndTime does not increase indefinitely and so the second if block can run
            if(!phaseInitialised){
                phaseEndTime = currentTime + calculatePhaseDuration(priorityArr[currPhaseIndex]);
                phaseInitialised = true;
            }

            if(currentTime>=phaseEndTime){
                currPhaseIndex = (currPhaseIndex + 1) % 4; // switches to next phase 
                if(!PhaseChangedAtLeastOnce){
                   PhaseChangedAtLeastOnce = true;
                } 
                phaseEndTime = currentTime + calculatePhaseDuration(priorityArr[currPhaseIndex]);
            }
            

                    
                    
                
        }

        /// <summary>
        /// Calculates priorirty solely pased on priority
        /// </summary>
        /// <param name="priority">determines how long its phase will be </param>
        /// <returns>phase duration</returns>
        private uint calculatePhaseDuration(double priority){
            return (uint) (30f +(priority * 15f)); // as the priority increases , the phase duration increases by 5 starting from 30 (priority 0)
        }

        
            
    }







        

    

}