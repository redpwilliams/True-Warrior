using System;

namespace Util
{
    /// A set of english and japanese translations that make up
    /// a single line in a haiku
    [Serializable]
    public struct LinePair
    {
        public string en;
        public string jp;

        public LinePair(string en, string jp)
        {
            this.en = en;
            this.jp = jp;
        }
    }
}