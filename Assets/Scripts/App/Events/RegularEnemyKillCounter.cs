namespace App.Events
{
    // Считает количество убитых обычных врагов.
    // Реагирует на события смерти врагов
    // и публикует событие изменения счетчика.
    public sealed class RegularEnemyKillCounter
    {
        // Шина игровых событий
        private readonly IGameEventBus _eventBus;

        // Текущее количество убитых обычных врагов
        private int _killCount;

        public RegularEnemyKillCounter(
            IGameEventBus eventBus)
        {
            _eventBus = eventBus;

            // Подписываемся на событие смерти врага
            _eventBus.Subscribe<EnemyDiedEvent>(
                OnEnemyDied);
        }

        // Текущее количество убийств
        public int KillCount => _killCount;

        // Вызывается после смерти любого врага
        private void OnEnemyDied(
            EnemyDiedEvent diedEvent)
        {
            // Считаем только обычных врагов
            if (diedEvent.Category !=
                EnemyCategory.Regular)
            {
                return;
            }

            // Увеличиваем счетчик убийств
            _killCount++;

            // Уведомляем остальные системы,
            // что количество убийств изменилось
            _eventBus.Publish(
                new RegularEnemyKillCountChangedEvent(
                    _killCount));
        }
    }
}