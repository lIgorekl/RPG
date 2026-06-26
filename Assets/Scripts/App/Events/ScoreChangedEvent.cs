namespace App.Events
{
    // Событие изменения количества очков.
    // Передается через шину событий после начисления очков,
    // чтобы другие системы могли обновить свое состояние.
    public readonly struct ScoreChangedEvent
    {
        // Создает событие изменения счета
        public ScoreChangedEvent(
            int totalScore,
            int pointsAdded)
        {
            TotalScore = totalScore;
            PointsAdded = pointsAdded;
        }

        // Общее количество очков
        // после начисления
        public int TotalScore { get; }

        // Количество очков,
        // начисленных за последнее действие
        public int PointsAdded { get; }
    }
}