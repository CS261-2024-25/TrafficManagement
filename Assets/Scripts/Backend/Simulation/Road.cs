namespace Assets.Scripts.Backend.Simulation 
{
    using System.Collections.Generic;
    using System;
    using System.Linq;
    using Assets.Scripts.Backend.Engine;
    using Assets.Scripts.Backend.Queuing;
    using Assets.Scripts.Backend.Vehicle;
    public class Road{

        private uint number_lanes_inbound; // Number of lanes
        private uint number_lanes_outbound;
        public double TrafficPriority;
        protected Engine Engine;
        public Dictionary<int,IntoJunctionLane> IJLanes= new Dictionary<int,IntoJunctionLane>();
        public Dictionary<int,ExitJunctionLane> EJLanes= new Dictionary<int,ExitJunctionLane>();

        public Road(Engine engine)
        {
            this.Engine = engine ?? throw new ArgumentNullException(nameof(engine), "Engine cannot be null.");
        }
        
        public uint numLanesInbound {
            get{return number_lanes_inbound;}
            set{
                if(value<1 || value>5){
                     throw new ArgumentOutOfRangeException("Number of lanes must be between 1 and 5.");  //validation of number of lanes
                } 
                if (number_lanes_inbound != value) { 
                    number_lanes_inbound = value;
                    InitLanesInbound();
                }
            }
        }

        public uint numLanesOutbound{
            get{return number_lanes_outbound;}
            set{
                if(value<1 || value>5){
                     throw new ArgumentOutOfRangeException("Number of lanes must be between 1 and 5.");  //validation of number of lanes
                } 
                if (number_lanes_outbound != value) { // Only update if changed
                    number_lanes_outbound = value;
                    InitLanesOutbound();
                }
    
            }
        }

    

        

        private void InitLanesInbound(){
            IJLanes.Clear();
            for(int i = 0; i < number_lanes_inbound; i++){
                IJLanes.Add(i,new IntoJunctionLane(Engine));
            }

        }

        private void InitLanesOutbound(){
            EJLanes.Clear();
            for(int i = 0; i < number_lanes_outbound; i++){
                EJLanes.Add(i,new ExitJunctionLane(Engine));
            }

        }

        
        
    }
}
