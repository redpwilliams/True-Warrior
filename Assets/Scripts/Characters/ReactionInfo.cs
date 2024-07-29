namespace Characters
{
    public struct ReactionInfo
    {
        public readonly Character Winner;
        public readonly string ReactionTime;

        public ReactionInfo(Character winner, string formattedReactionTime)
        {
            Winner = winner;
            ReactionTime = formattedReactionTime;
        }
    }
}