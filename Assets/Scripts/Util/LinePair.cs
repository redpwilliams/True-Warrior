namespace Characters
{
    /// A set of english and japanese translations that make up
    /// a single line in a haiku
    public struct LinePair
    {
        public string En { get; }
        public string Jp { get; }

        public LinePair(string en, string jp)
        {
            this.En = en;
            this.Jp = jp;
        }
    }
}