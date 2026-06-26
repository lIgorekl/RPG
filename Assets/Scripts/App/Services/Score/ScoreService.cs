using App.Events;

namespace App.Services.Score
{
    // Сервис подсчета очков.
    // Начисляет очки за убийство врагов
    // и уведомляет систему об изменении счета.
    public sealed class ScoreService : IScoreService
    {
        // Шина игровых событий
        private readonly IGameEventBus _eventBus;

        // Настройки системы очков
        private readonly ScoreSettings _settings;

        public ScoreService(
            IGameEventBus eventBus,
            ScoreSettings settings)
        {
            _eventBus = eventBus;
            _settings = settings;

            // Подписываемся на событие смерти врага
            _eventBus.Subscribe<EnemyDiedEvent>(
                OnEnemyDied);
        }

        // Текущее количество очков
        public int CurrentScore { get; private set; }

        // Вызывается после смерти любого врага
        private void OnEnemyDied(
            EnemyDiedEvent diedEvent)
        {
            // Если настройки отсутствуют,
            // начисление очков невозможно
            if (_settings == null)
                return;

            // Определяем,
            // сколько очков нужно начислить
            int points =
                diedEvent.Category == EnemyCategory.Boss
                    ? _settings.BossPoints
                    : _settings.RegularEnemyPoints;

            // Если очков начислять не нужно,
            // завершаем работу
            if (points <= 0)
                return;

            // Увеличиваем общий счет
            CurrentScore += points;

            // Сообщаем всем системам,
            // что количество очков изменилось
            _eventBus.Publish(
                new ScoreChangedEvent(
                    CurrentScore,
                    points));
        }
    }
}