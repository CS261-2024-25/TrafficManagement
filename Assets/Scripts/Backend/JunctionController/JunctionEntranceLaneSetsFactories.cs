using System;
using Assets.Scripts.Backend.Queuing;

namespace Assets.Scripts.Backend.JunctionController
{

    public partial class JunctionEntranceFactory
    {
        public JunctionEntranceFactory(Engine.Engine engine) 
        {
            Engine = engine;
        }

        public JunctionEntranceFactoryL SetJunctionLanes(
            uint intoJunctionLanes, 
            uint exitJunctionLanes
        ) {
            return new JunctionEntranceFactoryL(
                Engine,
                intoJunctionLanes,
                exitJunctionLanes
            );
        }
    }

    /// <summary>
    /// Junction Entrance Factory with Lanes
    /// 
    /// 
    /// ** ALWAYS START CONSTRUCTION FROM ENTRANCE FACTORY**
    /// </summary>
    public partial class JunctionEntranceFactoryL : IGeneratesEntrance
    {
        public JunctionEntranceLaneSets GenerateEntrance() 
        {
            return new JunctionEntranceLaneSets(Engine, IntoJunctionLanes, ExitJunctionLanes, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Constructed factory object</returns>
        /// <exception cref="ArgumentException">Throws if IntoJunctionLanes is less than 1</exception>
        public JunctionEntranceFactoryLL SetLeftTurnLane()
        {
            if (IntoJunctionLanes < 1)
            {
                throw new ArgumentException("Must have atleast 1 lane to have a left turn lane");
            }

            return new JunctionEntranceFactoryLL(Engine, IntoJunctionLanes, ExitJunctionLanes);
        }
    }

    /// <summary>
    /// Junction entrance with both a left turn lane and lanes defined.
    /// 
    /// Do NOT directly construct this class as it does not guarantee correctness.
    /// </summary>
    public partial class JunctionEntranceFactoryLL : IGeneratesEntrance
    {
        public JunctionEntranceLaneSets GenerateEntrance()
        {
            return new JunctionEntranceLaneSets(Engine, IntoJunctionLanes, ExitJunctionLanes, true);
        }
    }

#region DO NOT USE ANY APIS IN THIS REGION
    public interface IGeneratesEntrance
    {
        public JunctionEntranceLaneSets GenerateEntrance();
    }

    public partial class JunctionEntranceLaneSets
    {
        private Engine.Engine Engine;

        /// <summary>
        /// USE THE FACTORY `JunctionEntranceFactory` INSTEAD
        /// </summary>
        public JunctionEntranceLaneSets(
            Engine.Engine engine,
            uint intoJunctionLanes, 
            uint exitJunctionLanes,
            bool hasLeftTurnLane
        ) {
            Engine = engine;
            HasLeftTurn = hasLeftTurnLane;

            while (intoJunctionLanes > 0)
            {
                IntoJunctionLanes.Add(new IntoJunctionLane(engine));
                intoJunctionLanes--;
            }

            while (exitJunctionLanes > 0)
            {
                ExitJunctionLanes.Add(new ExitJunctionLane(engine));
                exitJunctionLanes--;
            }
        }
    }

    public partial class JunctionEntranceFactory
    {
        private Engine.Engine Engine;
    }

    /// <summary>
    /// Junction Entrance Factory with Lanes
    /// 
    /// 
    /// ** ALWAYS START CONSTRUCTION FROM ENTRANCE FACTORY**
    /// </summary>
    public partial class JunctionEntranceFactoryL : IGeneratesEntrance
    {
        private uint IntoJunctionLanes;
        private uint ExitJunctionLanes;
        private Engine.Engine Engine;

        /// <summary>
        /// This can be a zero-lane junction (ie. cars do not enter or leave)
        /// </summary>
        public JunctionEntranceFactoryL(
            Engine.Engine engine, 
            uint intoJunctionLanes, 
            uint exitJunctionLanes
        ) {
            IntoJunctionLanes = intoJunctionLanes;
            ExitJunctionLanes = exitJunctionLanes;
            Engine = engine;
        }
    }

    /// <summary>
    /// Junction entrance with both a left turn lane and lanes defined.
    /// 
    /// Do NOT directly construct this class as it does not guarantee correctness.
    /// </summary>
    public partial class JunctionEntranceFactoryLL : IGeneratesEntrance
    {
        private uint IntoJunctionLanes;
        private uint ExitJunctionLanes;
        private Engine.Engine Engine;
        public JunctionEntranceFactoryLL(
            Engine.Engine engine, 
            uint intoJunctionLanes, 
            uint exitJunctionLanes
        ) {
            IntoJunctionLanes = intoJunctionLanes;
            ExitJunctionLanes = exitJunctionLanes;
            Engine = engine;
        }
    }
#endregion
}