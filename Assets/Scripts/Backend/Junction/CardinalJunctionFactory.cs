using System;
using System.Collections.Generic;
using Assets.Scripts.Backend.Queuing;
using Assets.Scripts.Util;

namespace Assets.Scripts.Backend.Junction
{
    public class CardinalJunctionFactory : GeneratesCardinalJunction
    {
        private readonly Engine.Engine Engine;
        private List<(Lazy<bool>, string)> Assertions;
        private JunctionEntrance[] Entrances;
        private bool[] UpdatedEntrances;
        private bool[] PedestrianCrossings;

        private static readonly int NorthIndex = (int) CardinalDirection.North % 4;
        private static readonly int EastIndex = (int) CardinalDirection.East % 4;
        private static readonly int SouthIndex = (int) CardinalDirection.South % 4;
        private static readonly int WestIndex = (int) CardinalDirection.West % 4;

        public CardinalJunctionFactory(Engine.Engine engine)
        {
            Engine = engine;
            Assertions = new List<(Lazy<bool>, string)>();
            UpdatedEntrances = new bool[4] {false, false, false, false};
            PedestrianCrossings = new bool[4] {false, false, false, false};
            Entrances = new JunctionEntrance[4] {
                JunctionEntrance.ClosedEntrance(Engine),
                JunctionEntrance.ClosedEntrance(Engine),
                JunctionEntrance.ClosedEntrance(Engine),
                JunctionEntrance.ClosedEntrance(Engine)
            };
        }

        public CardinalJunction GenerateJunction()
        {
            foreach (var assertion in Assertions)
            {
                if (!assertion.Item1.Value)
                {
                    throw new ArgumentException($"Constructor found err: {assertion.Item2}");
                }
            }

            for (int i = 0; i < Entrances.Length; ++i)
            {
                for (int j = i + 1; j < Entrances.Length; ++j)
                {
                    if (ReferenceEquals(Entrances[i], Entrances[j]))
                    {
                        throw new ArgumentException(
                            "Two entrances for a CardinalJunction instance " + 
                            "reference the same underlying object illegaly. "
                        );
                    }
                }
            }

            return new CardinalJunction(
                engine: Engine,
                northJunctionEntrance: Entrances[NorthIndex],
                eastJunctionEntrance: Entrances[EastIndex],
                southJunctionEntrance: Entrances[SouthIndex],
                westJunctionEntrance: Entrances[WestIndex],
                northPedestrianCrossing: PedestrianCrossings[NorthIndex],
                eastPedestrianCrossing: PedestrianCrossings[EastIndex],
                southPedestrianCrossing: PedestrianCrossings[SouthIndex],
                westPedestrianCrossing: PedestrianCrossings[WestIndex]
            );
        }

        public CardinalJunctionFactory AddNorthEntrance(
            JunctionEntrance entrance, 
            bool pedestrianCrossingEnabled=false
        ) {
            if (UpdatedEntrances[NorthIndex])
            {
                throw new ArgumentException("Tried to define north entrance for a CardinalJunction instance twice");
            }
            else
            {
                UpdatedEntrances[NorthIndex] = true;
                Entrances[NorthIndex] = entrance;
                PedestrianCrossings[NorthIndex] = pedestrianCrossingEnabled;
            }

            if (entrance.LeftValid)
            {
                Assertions.Add((
                    new Lazy<bool>(() => Entrances[EastIndex].ExitJunctionLanesCount() > 0), 
                    "North entrance requires a valid left turn but east entrance has no exit lanes."
                ));
            }

            if (entrance.ForwardValid)
            {
                Assertions.Add((
                    new Lazy<bool>(() => Entrances[SouthIndex].ExitJunctionLanesCount() > 0),
                    "North entrance requires valid forward but south entrance has no exit lanes." 
                ));
            }

            if (entrance.RightValid)
            {
                Assertions.Add((
                    new Lazy<bool>(() => Entrances[WestIndex].ExitJunctionLanesCount() > 0), 
                    "North entrance requires a valid right turn but east entrance has no exit lanes."
                ));
            }

            return this;
        }

        public CardinalJunctionFactory AddEastEntrance(
            JunctionEntrance entrance, 
            bool pedestrianCrossingEnabled=false
        ) {
            if (UpdatedEntrances[EastIndex])
            {
                throw new ArgumentException("Tried to define east entrance for a CardinalJunction instance twice");
            }
            else
            {
                UpdatedEntrances[EastIndex] = true;
                Entrances[EastIndex] = entrance;
                PedestrianCrossings[EastIndex] = pedestrianCrossingEnabled;
            }

            if (entrance.LeftValid)
            {
                Assertions.Add((
                    new Lazy<bool>(() => Entrances[SouthIndex].ExitJunctionLanesCount() > 0), 
                    "East entrance requires a valid left turn but south entrance has no exit lanes."
                ));
            }

            if (entrance.ForwardValid)
            {
                Assertions.Add((
                    new Lazy<bool>(() => Entrances[WestIndex].ExitJunctionLanesCount() > 0),
                    "East entrance requires valid forward but west entrance has no exit lanes." 
                ));
            }

            if (entrance.RightValid)
            {
                Assertions.Add((
                    new Lazy<bool>(() => Entrances[NorthIndex].ExitJunctionLanesCount() > 0), 
                    "East entrance requires a valid right turn but north entrance has no exit lanes."
                ));
            }

            return this;
        }

        public CardinalJunctionFactory AddSouthEntrance(
            JunctionEntrance entrance, 
            bool pedestrianCrossingEnabled=false
        ) {
            if (UpdatedEntrances[SouthIndex])
            {
                throw new ArgumentException("Tried to define south entrance for a CardinalJunction instance twice");
            }
            else
            {
                UpdatedEntrances[SouthIndex] = true;
                Entrances[SouthIndex] = entrance;
                PedestrianCrossings[SouthIndex] = pedestrianCrossingEnabled;
            }

            if (entrance.LeftValid)
            {
                Assertions.Add((
                    new Lazy<bool>(() => Entrances[WestIndex].ExitJunctionLanesCount() > 0), 
                    "South entrance requires a valid left turn but west entrance has no exit lanes."
                ));
            }

            if (entrance.ForwardValid)
            {
                Assertions.Add((
                    new Lazy<bool>(() => Entrances[NorthIndex].ExitJunctionLanesCount() > 0),
                    "South entrance requires valid forward but north entrance has no exit lanes." 
                ));
            }

            if (entrance.RightValid)
            {
                Assertions.Add((
                    new Lazy<bool>(() => Entrances[EastIndex].ExitJunctionLanesCount() > 0), 
                    "South entrance requires a valid right turn but east entrance has no exit lanes."
                ));
            }

            return this;
        }

        public CardinalJunctionFactory AddWestEntrance(
            JunctionEntrance entrance, 
            bool pedestrianCrossingEnabled=false
        ) {
            if (UpdatedEntrances[WestIndex])
            {
                throw new ArgumentException("Tried to define west entrance for a CardinalJunction instance twice");
            }
            else
            {
                UpdatedEntrances[WestIndex] = true;
                Entrances[WestIndex] = entrance;
                PedestrianCrossings[WestIndex] = pedestrianCrossingEnabled;
            }

            if (entrance.LeftValid)
            {
                Assertions.Add((
                    new Lazy<bool>(() => Entrances[NorthIndex].ExitJunctionLanesCount() > 0), 
                    "West entrance requires a valid left turn but north entrance has no exit lanes."
                ));
            }

            if (entrance.ForwardValid)
            {
                Assertions.Add((
                    new Lazy<bool>(() => Entrances[EastIndex].ExitJunctionLanesCount() > 0),
                    "West entrance requires valid forward but east entrance has no exit lanes." 
                ));
            }

            if (entrance.RightValid)
            {
                Assertions.Add((
                    new Lazy<bool>(() => Entrances[SouthIndex].ExitJunctionLanesCount() > 0), 
                    "West entrance requires a valid right turn but south entrance has no exit lanes."
                ));
            }

            return this;
        }
    }
}