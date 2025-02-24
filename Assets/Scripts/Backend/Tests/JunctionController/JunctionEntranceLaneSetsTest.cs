using NUnit.Framework;
using Assets.Scripts.Backend.Engine;
using Assets.Scripts.Backend.JunctionController;
using System;

public class JunctionEntranceLaneSetsTest
{
    [Test]
    public void TooFewLanesForExclusiveUseBoth()
    {
        // this is fine
        Engine engine = new Engine(10);

        // This will throw an exception because we cannot create both 
        // an exclusive left and right turn lane
        var ex = Assert.Throws<ArgumentException>(
            () => {
                var laneSet = new JunctionEntranceFactory(engine)
                    .SetJunctionLanes(
                        intoJunctionLanes: 1, 
                        exitJunctionLanes: 3
                    )
                    .SetLeftTurnLane()
                    .SetRightTurnLane()
                    .GenerateEntrance();
            }
        );
    }

    [Test]
    public void ZeroIncomingOrOutgoingIsFine()
    {
        Engine engine = new Engine(ticksPerSecond: 5);

        var _okLaneSet = new JunctionEntranceFactory(engine)
            .SetJunctionLanes(
                intoJunctionLanes: 0,
                exitJunctionLanes: 0
            )
            .GenerateEntrance();
    }

    [Test]
    public void TooFewLanesForExclusiveSingle()
    {
        Engine engine = new Engine(ticksPerSecond: 1);

        var _x = Assert.Throws<ArgumentException>(
            () => {
                var laneSet = new JunctionEntranceFactory(engine)
                    .SetJunctionLanes(0, 1)
                    .SetLeftTurnLane()
                    .GenerateEntrance();
            }
        );

        var _y = Assert.Throws<ArgumentException>(
            () => {
                var laneSet = new JunctionEntranceFactory(engine)
                    .SetJunctionLanes(0, 1)
                    .SetRightTurnLane()
                    .GenerateEntrance();
            }
        );
    }

    [Test]
    public void SetTwoExclusiveIsFineIfEnoughLanes()
    {
        Engine engine = new Engine(100);

        var _ = new JunctionEntranceFactory(engine)
            .SetJunctionLanes(5, 0)
            .SetRightTurnLane()
            .SetLeftTurnLane()
            .GenerateEntrance();
    }
}