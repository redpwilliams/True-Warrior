using System;

namespace Characters
{
    /// A set of english and japanese translations that make up
    /// a single line in a haiku
    [Serializable]
    public struct LinePair
    {
        public string en { get; }
        public string jp { get; }

        public LinePair(string en, string jp)
        {
            this.en = en;
            this.jp = jp;
        }
    }
}