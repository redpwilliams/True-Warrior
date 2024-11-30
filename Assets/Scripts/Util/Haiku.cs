using System;
using System.Collections.Generic;

namespace Util
{
    [Serializable]
    public struct Haiku
    {
        public List<LinePair> one;
        public List<LinePair> two;
        public List<LinePair> three;
    }

}