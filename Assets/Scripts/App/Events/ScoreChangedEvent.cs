namespace App.Events
{
    public readonly struct ScoreChangedEvent
    {
        public ScoreChangedEvent(int totalScore, int pointsAdded)
        {
            TotalScore = totalScore;
            PointsAdded = pointsAdded;
        }

        public int TotalScore { get; }
        public int PointsAdded { get; }
    }
}
