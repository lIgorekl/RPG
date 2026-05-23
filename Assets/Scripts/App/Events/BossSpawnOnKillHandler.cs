using App.Services.Spawn;
using Presentation.Scene;
using UnityEngine;

namespace App.Events
{
    public sealed class BossSpawnOnKillHandler
    {
        private readonly IGameEventBus _eventBus;
        private readonly GameEventsSettings _settings;
        private readonly IBossSpawnService _bossSpawnService;
        private readonly IGameEnemyRegistry _enemyRegistry;
        private readonly SpawnPoint[] _spawnPoints;
        private readonly Transform _player;
        private readonly System.Random _random;
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

            _eventBus.Subscribe<RegularEnemyKillCountChangedEvent>(
                OnKillCountChanged);
        }

        private void OnKillCountChanged(
            RegularEnemyKillCountChangedEvent countEvent)
        {
            if (_bossSpawned)
                return;

            if (countEvent.KillCount < _settings.BossSpawnKillCount)
                return;

            var bossDefinition = _settings.BossSpawnDefinition;
            if (bossDefinition == null)
            {
                Debug.LogWarning(
                    "BossSpawnOnKillHandler: BossSpawnDefinition is not set.");
                return;
            }

            _bossSpawned = true;

            var bossView = _bossSpawnService.SpawnBoss(
                bossDefinition,
                _spawnPoints,
                _player,
                _random);

            if (bossView == null)
                return;

            _enemyRegistry.RegisterSpawnedEnemy(bossView);
            _eventBus.Publish(new BossSpawnedEvent(bossView));
        }
    }
}
