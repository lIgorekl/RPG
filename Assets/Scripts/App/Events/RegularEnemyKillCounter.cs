namespace App.Events
{
    public sealed class RegularEnemyKillCounter
    {
        private readonly IGameEventBus _eventBus;
        private int _killCount;

        public RegularEnemyKillCounter(IGameEventBus eventBus)
        {
            _eventBus = eventBus;
            _eventBus.Subscribe<EnemyDiedEvent>(OnEnemyDied);
        }

        public int KillCount => _killCount;

        private void OnEnemyDied(EnemyDiedEvent diedEvent)
        {
            if (diedEvent.Category != EnemyCategory.Regular)
                return;

            _killCount++;
            _eventBus.Publish(
                new RegularEnemyKillCountChangedEvent(_killCount));
        }
    }
}
