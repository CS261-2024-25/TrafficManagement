using System.Collections.Generic;
using Assets.Scripts.Util;

namespace Assets.Scripts.Backend.JunctionController
{
    public class CardinalJunction
    {
        private Dictionary<CardinalDirection, JunctionEntrance> Entrances;
        private Engine.Engine Engine;
    }
}