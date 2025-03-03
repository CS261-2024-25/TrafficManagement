namespace Assets.Scripts.Backend.Simulation
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Assets.Scripts.Util;
    public class DirectionConfig
    {
        public CardinalDirection Name { get; set; }
        public DirectionDetails boundDir { get; set; }
        public Road road { get; set; }
        public double TrafficPriority { get; set; }
        
        private TurnDirs _turnDirs; 
        public TurnDirs turnDirs => _turnDirs; // Read-only public property

        public DirectionConfig(CardinalDirection name)
        {
            Name = name;
            _turnDirs = new TurnDirs();

            switch (name)
            {
                case CardinalDirection.South:
                    _turnDirs.Left = CardinalDirection.West;
                    _turnDirs.Right = CardinalDirection.East;
                    _turnDirs.Forward = CardinalDirection.North;
                    break;
                case CardinalDirection.West:
                    _turnDirs.Left = CardinalDirection.North;
                    _turnDirs.Right = CardinalDirection.South;
                    _turnDirs.Forward = CardinalDirection.East;
                    break;
                case CardinalDirection.East:
                    _turnDirs.Left = CardinalDirection.South;
                    _turnDirs.Right = CardinalDirection.North;
                    _turnDirs.Forward = CardinalDirection.West;
                    break;
                default:
                    _turnDirs.Left = CardinalDirection.East;
                    _turnDirs.Right = CardinalDirection.West;
                    _turnDirs.Forward = CardinalDirection.South;
                    break;
            }
        }
    }


    public class TurnDirs 
    {
        public CardinalDirection Left {get; set;}
        public CardinalDirection Forward {get; set;}
        public CardinalDirection Right {get; set;}
    }



    

}
