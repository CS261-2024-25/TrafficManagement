using NUnit.Framework;
using Assets.Scripts.Backend.Engine;
using Assets.Scripts.Backend.Junction;
using System;

public class CardinalJunctionFactoryTest
{
    [Test]
    public void EmptyConstruct()
    {
        var engine = new Engine(10);

        // cant turn cant do anything
        var _ = new CardinalJunctionFactory(engine)
            .GenerateJunction();
    }

    [Test]
    public void StraightJunctionOk()
    {
        // kind of pointless but nice api test
        var engine = new Engine(10);

        var northEntrance = new JunctionEntrance(
            engine, new JunctionEntranceLaneSets(
                engine, 1, 1, false, false
            ),
            leftValid: false,
            forwardValid: true,
            rightValid: false
        );

        var southEntrance = new JunctionEntrance(
            engine, new JunctionEntranceLaneSets(
                engine, 1, 1, false, false
            ),
            leftValid: false,
            forwardValid: true,
            rightValid: false
        );

        var northSouthJunction = new CardinalJunctionFactory(engine)
            .AddNorthEntrance(northEntrance)
            .AddSouthEntrance(southEntrance)
            .GenerateJunction();

        var eastEntrance = new JunctionEntrance(
            engine, new JunctionEntranceLaneSets(
                engine, 1, 1, false, false
            ),
            leftValid: false,
            forwardValid: true,
            rightValid: false
        );

        var westEntrance = new JunctionEntrance(
            engine, new JunctionEntranceLaneSets(
                engine, 1, 1, false, false
            ),
            leftValid: false,
            forwardValid: true,
            rightValid: false
        );

        var eastWestJunction = new CardinalJunctionFactory(engine)
            .AddEastEntrance(eastEntrance)
            .AddWestEntrance(westEntrance)
            .GenerateJunction();
    }

    [Test]
    public void ThreeWayJunctionNorthTriesToAccessIncorrect()
    {
        var engine = new Engine(4);

        var northEntrance = new JunctionEntrance(
            engine, new JunctionEntranceLaneSets(
                engine, 1, 1, false, false
            ),
            true, true, true
        );

        var eastEntrance = new JunctionEntrance(
            engine, new JunctionEntranceLaneSets(
                engine, 1, 1, false, false
            ),
            true, false, true
        );

        var southEntrance = new JunctionEntrance(
            engine, new JunctionEntranceLaneSets(
                engine, 1, 1, false, false
            ),
            false, true, true
        );

        var ex = Assert.Throws<ArgumentException>(
            () => {
                var _ = new CardinalJunctionFactory(engine)
                    .AddNorthEntrance(northEntrance)
                    .AddEastEntrance(eastEntrance)
                    .AddSouthEntrance(southEntrance)
                    .GenerateJunction();
            }
        );
    }

    [Test]
    public void FourWayJunctionOk()
    {
        var engine = new Engine(4);

        var northEntrance = new JunctionEntrance(
            engine, new JunctionEntranceLaneSets(
                engine, 3, 2, true, true
            ),
            true, true, true
        );

        var eastEntrance = new JunctionEntrance(
            engine, new JunctionEntranceLaneSets(
                engine, 2, 1, true, false
            ),
            true, true, true
        );

        var southEntrance = new JunctionEntrance(
            engine, new JunctionEntranceLaneSets(
                engine, 1, 1, false, false
            ),
            true, true, true
        );

        var westEntrance = new JunctionEntrance(
            engine, new JunctionEntranceLaneSets(
                engine, 2, 1, false, true
            ),
            true, true, true
        );

        var _ = new CardinalJunctionFactory(engine)
            .AddSouthEntrance(southEntrance)
            .AddWestEntrance(westEntrance)
            .AddNorthEntrance(northEntrance)
            .AddEastEntrance(eastEntrance)
            .GenerateJunction();
    }

    [Test]
    public void CannotDoubleDefineAnEntrance()
    {
        var engine = new Engine(100);

        var northEntrance = new JunctionEntrance(
            engine, new JunctionEntranceLaneSets(
                engine, 3, 2, true, true
            ),
            true, true, true
        );

        var ex = Assert.Throws<ArgumentException>(
            () => {
                var _ = new CardinalJunctionFactory(engine)
                    .AddNorthEntrance(northEntrance)
                    .AddNorthEntrance(northEntrance);
            }  
        );
    }

    [Test]
    public void CannotReferenceSameObjectInMemory()
    {
        var engine = new Engine(100);

        var northEntrance = JunctionEntrance.ClosedEntrance(engine);

        var ex = Assert.Throws<ArgumentException>(
            () => {
                var _ = new CardinalJunctionFactory(engine)
                    .AddNorthEntrance(northEntrance)
                    .AddEastEntrance(northEntrance)
                    .GenerateJunction();
            }  
        );

        var dummy = JunctionEntrance.ClosedEntrance(engine);

        // but this is ok bc they are unique objects
        var _ = new CardinalJunctionFactory(engine)
            .AddNorthEntrance(northEntrance)
            .AddEastEntrance(dummy)
            .GenerateJunction();
    }
}