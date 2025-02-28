using Assets.Scripts.Backend.Junction;
using Assets.Scripts.Backend.Junction.Unsafe;
using Assets.Scripts.Backend.JunctionController;
using Assets.Scripts.Util;

namespace Assets.Scripts.Backend.UserInputHandling
{
    public class UserInputHandling
    {
        public static FourWayCardinalJunctionWithLights ToFourWayCardinalJunctionWithLights(Engine.Engine engine, InputParameters inputParameters)
        {
            var northDetails = inputParameters.Northbound;
            var eastDetails = inputParameters.Eastbound;
            var westDetails = inputParameters.Westbound;
            var southDetails = inputParameters.Southbound;

            var northEntrance = new JunctionEntrance(
                engine: engine,
                junctionEntranceLaneSets: CreateLaneSetAllTurnsValid(engine, northDetails),
                leftValid: true,
                forwardValid: true,
                rightValid: true
            );

            var eastEntrance = new JunctionEntrance(
                engine: engine,
                junctionEntranceLaneSets: CreateLaneSetAllTurnsValid(engine, eastDetails),
                leftValid: true,
                forwardValid: true,
                rightValid: true
            );

            var southEntrance = new JunctionEntrance(
                engine: engine,
                junctionEntranceLaneSets: CreateLaneSetAllTurnsValid(engine, southDetails),
                leftValid: true,
                forwardValid: true,
                rightValid: true
            );

            var westEntrance = new JunctionEntrance(
                engine: engine,
                junctionEntranceLaneSets: CreateLaneSetAllTurnsValid(engine, westDetails),
                leftValid: true,
                forwardValid: true,
                rightValid: true
            );

            var junction = new CardinalJunction(
                engine: engine,
                northJunctionEntrance: northEntrance,
                eastJunctionEntrance: eastEntrance,
                southJunctionEntrance: southEntrance,
                westJunctionEntrance: westEntrance,
                northPedestrianCrossing: northDetails.HasPedestrianCrossing,
                eastPedestrianCrossing: eastDetails.HasPedestrianCrossing,
                southPedestrianCrossing: southDetails.HasPedestrianCrossing,
                westPedestrianCrossing: westDetails.HasPedestrianCrossing
            );

            return new FourWayCardinalJunctionWithLights(
                engine,
                junction,
                inputParameters.Priority
            );
        }

        private static JunctionEntranceLaneSets CreateLaneSetAllTurnsValid(Engine.Engine engine, DirectionDetails details)
        {
            var partial = new JunctionEntranceFactory(engine)
                .SetJunctionLanes(
                    details.LaneCountInbound, 
                    details.LaneCountOutbound
                );

            if (details.HasLeftTurn)
            {
                return partial.SetLeftTurnLane().GenerateEntrance();
            }
            else
            {
                return partial.GenerateEntrance();
            }
        }
    }
}