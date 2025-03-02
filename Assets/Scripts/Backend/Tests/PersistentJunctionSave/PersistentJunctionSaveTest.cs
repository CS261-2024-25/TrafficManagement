using Assets.Scripts.Util;
using NUnit.Framework;
using Assets.Scripts.Backend.PersistentJunctionSave;
using Newtonsoft.Json;

class PersistentJunctionSaveTest
{
    [SetUp]
    public void ResetSaveFile()
    {
        PersistentFileManager.WriteToFile(
            PersistentJunctionSave.JunctionSaveFileName,
            ""
        );
    }

    private (InputParameters, ResultTrafficSimulation) PairMaker(
        uint diffParam
    ) {
        var dirDetails1 = new DirectionDetails(diffParam, 3, 5, 7, 9, true, false);
        var dirDetails2 = new DirectionDetails(diffParam, 3, 5, 7, 9, true, false);
        var dirDetails3 = new DirectionDetails(diffParam, 3, 5, 7, 9, true, false);
        var dirDetails4 = new DirectionDetails(diffParam, 3, 5, 7, 9, true, false);

        var inputParams = new InputParameters(
            dirDetails1, 
            dirDetails2, 
            dirDetails3, 
            dirDetails4,
            new (CardinalDirection, double)[4] {
                (CardinalDirection.North, 1.0),
                (CardinalDirection.East, 1.0),
                (CardinalDirection.South, 1.0),
                (CardinalDirection.West, 1.0)
            }
        );

        var fakeResult = new ResultJunctionEntrance(5.0, 1.0, 10.0);
        var fakeResultSet = new ResultTrafficSimulation(
            fakeResult, 
            fakeResult, 
            fakeResult, 
            fakeResult
        );

        return (inputParams, fakeResultSet);
    }

    [Test]
    public void SerializeSomething()
    {
        var paramPair = PairMaker(1);

        PersistentJunctionSave.SaveResult(paramPair.Item1, paramPair.Item2);
    }

    [Test]
    public void WriteOneThenRead()
    {
        var paramPair = PairMaker(1);

        PersistentJunctionSave.SaveResult(paramPair.Item1, paramPair.Item2);

        (InputParameters, ResultTrafficSimulation)[] result;

        var success = PersistentJunctionSave.LoadAllResults(out result);
        Assert.True(success);

        Assert.AreEqual(result.Length, 1);
        Assert.AreEqual(
            JsonConvert.SerializeObject(paramPair.Item1), 
            JsonConvert.SerializeObject(result[0].Item1));
        Assert.AreEqual(
            JsonConvert.SerializeObject(paramPair.Item2), 
            JsonConvert.SerializeObject(result[0].Item2)); 
        Assert.AreEqual(
            paramPair.Item2.ResultWithDirection(CardinalDirection.North).MaxQueueLength, 
            result[0].Item2.ResultWithDirection(CardinalDirection.North).MaxQueueLength);
    }

    [Test]
    public void WriteManyThenRead()
    {
        var len = 50;
        var paramPairs = new (InputParameters, ResultTrafficSimulation)[len];
        for (uint i = 0; i < len; ++i)
        {
            paramPairs[i] = PairMaker(i + 1);
            PersistentJunctionSave.SaveResult(paramPairs[i].Item1, paramPairs[i].Item2);
        }

        (InputParameters, ResultTrafficSimulation)[] results;
        var success = PersistentJunctionSave.LoadAllResults(out results);
        Assert.True(success);

        Assert.AreEqual(len, results.Length);
        for (int i = 0; i < len; ++i)
        {
            Assert.AreEqual(
                JsonConvert.SerializeObject(paramPairs[i].Item1), 
                JsonConvert.SerializeObject(results[i].Item1));
            Assert.AreEqual(
                JsonConvert.SerializeObject(paramPairs[i].Item2), 
                JsonConvert.SerializeObject(results[i].Item2)); 
            Assert.AreEqual(
                paramPairs[i].Item2.ResultWithDirection(CardinalDirection.East).AverageWaitTime, 
                results[i].Item2.ResultWithDirection(CardinalDirection.East).AverageWaitTime);
        }
    }

    [Test]
    public void WriteManyThenAggregate()
    {
        var len = 50;
        var paramPairs = new (InputParameters, ResultTrafficSimulation)[len];
        for (uint i = 0; i < len; ++i)
        {
            paramPairs[i] = PairMaker(i + 1);
            PersistentJunctionSave.SaveResult(paramPairs[i].Item1, paramPairs[i].Item2);
        }

        System.Collections.Generic.List<(double, (InputParameters, ResultTrafficSimulation))> 
            results;
        var success = PersistentJunctionSave.LoadByEfficiency(1.0, 1.0, 1.0, out results);
        Assert.True(success);

        Assert.AreEqual(results.Count, paramPairs.Length);
    }
}