using App.Services.Spawn;
using Presentation.Scene;
using UnityEngine;

namespace App.Events
{
    // Обработчик события достижения необходимого количества убийств.
    // После выполнения условия создает босса и публикует событие
    // о его появлении.
    public sealed class BossSpawnOnKillHandler
    {
        // Шина игровых событий
        private readonly IGameEventBus _eventBus;

        // Настройки игровых событий
        private readonly GameEventsSettings _settings;

        // Сервис создания босса
        private readonly IBossSpawnService _bossSpawnService;

        // Реестр врагов игры
        private readonly IGameEnemyRegistry _enemyRegistry;

        // Доступные точки появления босса
        private readonly SpawnPoint[] _spawnPoints;

        // Ссылка на игрока
        private readonly Transform _player;

        // Генератор случайных чисел
        private readonly System.Random _random;

        // Флаг, предотвращающий повторное появление босса
        private bool _bossSpawned;

        public BossSpawnOnKillHandler(
            IGameEventBus eventBus,
            GameEventsSettings settings,
            IBossSpawnService bossSpawnService,
            IGameEnemyRegistry enemyRegistry,
            SpawnPoint[] spawnPoints,
            Transform player,
            System.Random random)
        {
            _eventBus = eventBus;
            _settings = settings;
            _bossSpawnService = bossSpawnService;
            _enemyRegistry = enemyRegistry;
            _spawnPoints = spawnPoints;
            _player = player;
            _random = random;

            // Подписываемся на событие изменения
            // количества убитых обычных врагов
            _eventBus.Subscribe<RegularEnemyKillCountChangedEvent>(
                OnKillCountChanged);
        }

        // Вызывается при изменении количества убийств
        private void OnKillCountChanged(
            RegularEnemyKillCountChangedEvent countEvent)
        {
            // Если босс уже появился,
            // ничего не делаем
            if (_bossSpawned)
                return;

            // Если убийств еще недостаточно,
            // ожидаем дальнейших событий
            if (countEvent.KillCount <
                _settings.BossSpawnKillCount)
            {
                return;
            }

            // Получаем описание босса
            var bossDefinition =
                _settings.BossSpawnDefinition;

            // Если описание отсутствует,
            // выводим предупреждение
            if (bossDefinition == null)
            {
                Debug.LogWarning(
                    "BossSpawnOnKillHandler: BossSpawnDefinition is not set.");
                return;
            }

            // Запоминаем,
            // что босс уже был создан
            _bossSpawned = true;

            // Создаем босса
            var bossView =
                _bossSpawnService.SpawnBoss(
                    bossDefinition,
                    _spawnPoints,
                    _player,
                    _random);

            // Если создать босса не удалось,
            // завершаем работу
            if (bossView == null)
                return;

            // Регистрируем нового врага
            // в системе игры
            _enemyRegistry.RegisterSpawnedEnemy(
                bossView);

            // Сообщаем всем системам,
            // что босс появился
            _eventBus.Publish(
                new BossSpawnedEvent(
                    bossView));
        }
    }
}