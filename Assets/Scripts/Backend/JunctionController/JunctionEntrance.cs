using System;

namespace Assets.Scripts.Backend.JunctionController
{
    /// <summary>
    /// This is concerned about what directions are valid to turn into
    /// </summary>
    class JunctionEntrance
    {
        private Engine.Engine Engine;
        public readonly bool LeftValid;
        public readonly bool ForwardValid;
        public readonly bool RightValid;

        public JunctionEntranceLaneSets LaneSets { get; }

        public JunctionEntrance(
            Engine.Engine engine,
            JunctionEntranceLaneSets junctionEntranceLaneSets,
            bool leftValid,
            bool forwardValid,
            bool rightValid
        ) {
            Engine = engine;

            if (!leftValid && junctionEntranceLaneSets.HasLeftTurn)
            {
                throw new ArgumentException("Cannot have a left turn lane when left turns are not valid");
            }

            LeftValid = leftValid;
            RightValid = rightValid;
            ForwardValid = forwardValid;
        }
    }
}