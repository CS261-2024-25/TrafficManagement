using Assets.Scripts.Util;

namespace Assets.Scripts.Backend.JunctionController
{
    public interface GeneratesCardinalJunction
    {
        public CardinalJunction GenerateJunction();
    }

    public class CardinalJunction
    {
        private JunctionEntrance[] Entrances;
        private Engine.Engine Engine;

        /// <summary>
        /// Do not use this constructor, use CardinalJunctionFactory for safety
        /// </summary>
        public CardinalJunction(
            Engine.Engine engine,
            JunctionEntrance northJunctionEntrance,
            JunctionEntrance eastJunctionEntrance,
            JunctionEntrance southJunctionEntrance,
            JunctionEntrance westJunctionEntrance
        ) {
            Entrances = new JunctionEntrance[4];
            Engine = engine;

            Entrances[(int) CardinalDirection.North % 4] = northJunctionEntrance;
            Entrances[(int) CardinalDirection.East % 4] = eastJunctionEntrance;
            Entrances[(int) CardinalDirection.South % 4] = southJunctionEntrance;
            Entrances[(int) CardinalDirection.West % 4] = westJunctionEntrance;
        }
    }

    
}