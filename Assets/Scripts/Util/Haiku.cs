using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace Characters
{
    [Serializable]
    public struct Haiku
    {
        public List<LinePair> lines;
    }
}