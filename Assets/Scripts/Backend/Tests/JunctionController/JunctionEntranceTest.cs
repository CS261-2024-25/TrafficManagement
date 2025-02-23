using NUnit.Framework;
using Assets.Scripts.Backend.Engine;
using Assets.Scripts.Backend.JunctionController;
using System;

public class JunctionEntranceTest
{
    [Test]
    public void ConstructEmptyOk()
    {
        Engine engine = new Engine(10);

        var emptyLaneSet = new JunctionEntranceFactory(engine)
            .SetJunctionLanes(0, 0)
            .GenerateEntrance();

        var _ = new JunctionEntrance(
            engine: engine, 
            junctionEntranceLaneSets: emptyLaneSet, 
            leftValid: false, 
            rightValid: false, 
            forwardValid: false
        );
    }

    [Test]
    public void ConstructEmptyLaneSetWithValidDirectionsNotOk()
    {
        Engine engine = new Engine(10);

        var emptyLaneSet = new JunctionEntranceFactory(engine)
            .SetJunctionLanes(0, 0)
            .GenerateEntrance();

        var ex = Assert.Throws<ArgumentException>(
            () => {
                var _ = new JunctionEntrance(
                    engine: engine,
                    junctionEntranceLaneSets: emptyLaneSet,
                    leftValid: false,
                    forwardValid: true,
                    rightValid: false
                );
            }
        );
    }

    [Test]
    public void LeftLaneExistsIfNotValidIsInvalid()
    {
        var engine = new Engine(5);
        var leftValid = new JunctionEntranceFactory(engine)
            .SetJunctionLanes(
                intoJunctionLanes: 1,
                exitJunctionLanes: 10
            )
            .SetLeftTurnLane()
            .GenerateEntrance();

        var ex = Assert.Throws<ArgumentException>(
            () => {
                var _ = new JunctionEntrance(
                    engine: engine,
                    junctionEntranceLaneSets: leftValid,
                    leftValid: false,
                    forwardValid: false,
                    rightValid: false
                );
            }
        );
    }

    [Test]
    public void NoLanesExistToGoForward()
    {
        // the only lane here is used as exclusive right
        var engine = new Engine(5);
        var rightValid = new JunctionEntranceFactory(engine)
            .SetJunctionLanes(
                intoJunctionLanes: 1,
                exitJunctionLanes: 10
            )
            .SetRightTurnLane()
            .GenerateEntrance();

        var ex = Assert.Throws<ArgumentException>(
            () => {
                var _ = new JunctionEntrance(
                    engine: engine,
                    junctionEntranceLaneSets: rightValid,
                    leftValid: false,
                    forwardValid: true,
                    rightValid: true
                );
            }
        );
    }

    [Test]
    public void AllDirectionsOkayForOneMultipleUseLane()
    {
        var engine = new Engine(3);
        var laneSet = new JunctionEntranceFactory(engine)
            .SetJunctionLanes(
                intoJunctionLanes: 1,
                exitJunctionLanes: 3
            )
            .GenerateEntrance();

        var _ = new JunctionEntrance(engine, laneSet, true, true, true);
    }

    [Test]
    public void TwoExclusiveLanesTooManyDirections()
    {
        var engine = new Engine(5);
        var rightValid = new JunctionEntranceFactory(engine)
            .SetJunctionLanes(
                intoJunctionLanes: 2,
                exitJunctionLanes: 10
            )
            .SetRightTurnLane()
            .SetLeftTurnLane()
            .GenerateEntrance();

        var ex = Assert.Throws<ArgumentException>(
            () => {
                var _ = new JunctionEntrance(
                    engine: engine,
                    junctionEntranceLaneSets: rightValid,
                    leftValid: true,
                    forwardValid: true,
                    rightValid: true
                );
            }
        );
    }

    [Test]
    public void ThreeLanesTwoExclusiveIsFine()
    {
        var engine = new Engine(3);
        var laneSet = new JunctionEntranceFactory(engine)
            .SetJunctionLanes(
                intoJunctionLanes: 3,
                exitJunctionLanes: 3
            )
            .SetLeftTurnLane()
            .SetRightTurnLane()
            .GenerateEntrance();

        var _ = new JunctionEntrance(engine, laneSet, true, true, true);
    }
}