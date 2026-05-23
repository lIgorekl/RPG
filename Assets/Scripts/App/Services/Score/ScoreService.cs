using App.Events;

namespace App.Services.Score
{
    public sealed class ScoreService : IScoreService
    {
        private readonly IGameEventBus _eventBus;
        private readonly ScoreSettings _settings;

        public ScoreService(IGameEventBus eventBus, ScoreSettings settings)
        {
            _eventBus = eventBus;
            _settings = settings;

            _eventBus.Subscribe<EnemyDiedEvent>(OnEnemyDied);
        }

        public int CurrentScore { get; private set; }

        private void OnEnemyDied(EnemyDiedEvent diedEvent)
        {
            if (_settings == null)
                return;

            int points = diedEvent.Category == EnemyCategory.Boss
                ? _settings.BossPoints
                : _settings.RegularEnemyPoints;

            if (points <= 0)
                return;

            CurrentScore += points;
            _eventBus.Publish(
                new ScoreChangedEvent(CurrentScore, points));
        }
    }
}
