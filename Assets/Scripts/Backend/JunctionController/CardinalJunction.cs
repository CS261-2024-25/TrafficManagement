using Assets.Scripts.Backend.Queuing;
using Assets.Scripts.Util;

namespace Assets.Scripts.Backend.JunctionController
{
    public interface GeneratesCardinalJunction
    {
        public CardinalJunction GenerateJunction();
    }

    public class CardinalJunction
    {
#nullable enable
        private (JunctionEntrance, PedestrianCrossing?)[] Entrances;

        private Engine.Engine Engine;

        private static readonly int NorthIndex = (int) CardinalDirection.North % 4;
        private static readonly int EastIndex = (int) CardinalDirection.East % 4;
        private static readonly int SouthIndex = (int) CardinalDirection.South % 4;
        private static readonly int WestIndex = (int) CardinalDirection.West % 4;

        /// <summary>
        /// Do not use this constructor, use CardinalJunctionFactory for safety
        /// </summary>
        public CardinalJunction(
            Engine.Engine engine,
            JunctionEntrance northJunctionEntrance,
            JunctionEntrance eastJunctionEntrance,
            JunctionEntrance southJunctionEntrance,
            JunctionEntrance westJunctionEntrance,
            bool northPedestrianCrossing,
            bool eastPedestrianCrossing,
            bool southPedestrianCrossing,
            bool westPedestrianCrossing
        ) {
            Entrances = new (JunctionEntrance, PedestrianCrossing?)[4];
            Engine = engine;

            Entrances[NorthIndex] = (northJunctionEntrance, northPedestrianCrossing ? 
                new PedestrianCrossing(engine) : null);
            Entrances[EastIndex] = (eastJunctionEntrance, eastPedestrianCrossing ? 
                new PedestrianCrossing(engine) : null);
            Entrances[WestIndex] = (westJunctionEntrance, westPedestrianCrossing ? 
                new PedestrianCrossing(engine) : null);
            Entrances[SouthIndex] = (southJunctionEntrance, southPedestrianCrossing ? 
                new PedestrianCrossing(engine) : null);
        }
#nullable disable
    }
}