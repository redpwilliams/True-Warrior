namespace Characters
{
    public struct ReactionInfo
    {
        public Character Winner;
        public string ReactionTime;

        public ReactionInfo(Character winner, string formattedReactionTime)
        {
            Winner = winner;
            ReactionTime = formattedReactionTime;
        }
    }
}