using App.Services;
using App.Services.Spawn;
using Presentation.Player;
using Presentation.Scene;
using UnityEngine;

namespace App.Events
{
    public sealed class GameEventsInstaller
    {
        private readonly EnemyDeathEventPublisher _deathPublisher;
        private readonly RegularEnemyKillCounter _killCounter;
        private readonly BossSpawnOnKillHandler _bossSpawnHandler;
        private readonly VictoryMusicOnKillHandler _victoryMusicHandler;

        public GameEventsInstaller(
            IGameEventBus eventBus,
            GameEventsSettings settings,
            IAudioService audioService,
            IGameEnemyRegistry enemyRegistry,
            SpawnPoint[] spawnPoints,
            PlayerController player)
        {
            _deathPublisher = new EnemyDeathEventPublisher(eventBus);
            _killCounter = new RegularEnemyKillCounter(eventBus);

            var bossSpawnService = new BossSpawnService(
                new EnemyFactory(),
                new RandomSpawnPointSelector());

            var playerTransform = player != null ? player.transform : null;

            _bossSpawnHandler = new BossSpawnOnKillHandler(
                eventBus,
                settings,
                bossSpawnService,
                enemyRegistry,
                spawnPoints,
                playerTransform,
                new System.Random());

            _victoryMusicHandler = new VictoryMusicOnKillHandler(
                eventBus,
                settings,
                audioService);
        }

        public void RegisterEnemy(BaseEnemyView enemyView)
        {
            _deathPublisher.Register(enemyView);
        }
    }
}
