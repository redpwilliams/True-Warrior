namespace Characters
{
    public struct ReactionInfo
    {
        public readonly bool IsWinner;
        public readonly string ReactionTime;

        public ReactionInfo(bool isWinner, string formattedReactionTime)
        {
            IsWinner = isWinner;
            ReactionTime = formattedReactionTime;
        }
    }
}