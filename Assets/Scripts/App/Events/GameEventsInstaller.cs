using App.Services;
using App.Services.Spawn;
using Presentation.Player;
using Presentation.Scene;
using UnityEngine;

namespace App.Events
{
    // Центральный установщик игровой системы событий
    // Связывает убийства врагов с реакциями системы: счет, боссы, музыка победы
    public sealed class GameEventsInstaller
    {
        // Публикатор событий смерти врагов
        private readonly EnemyDeathEventPublisher _deathPublisher;

        // Подсчет убийств обычных врагов
        private readonly RegularEnemyKillCounter _killCounter;

        // Обработчик спавна босса при достижении условий
        private readonly BossSpawnOnKillHandler _bossSpawnHandler;

        // Обработчик победной музыки при убийствах
        private readonly VictoryMusicOnKillHandler _victoryMusicHandler;

        public GameEventsInstaller(
            IGameEventBus eventBus,
            GameEventsSettings settings,
            IAudioService audioService,
            IGameEnemyRegistry enemyRegistry,
            SpawnPoint[] spawnPoints,
            PlayerController player)
        {
            // Публикует события смерти врагов в event bus
            _deathPublisher = new EnemyDeathEventPublisher(eventBus);

            // Считает убийства обычных врагов
            _killCounter = new RegularEnemyKillCounter(eventBus);

            // Сервис спавна босса
            var bossSpawnService = new BossSpawnService(
                new EnemyFactory(),
                new RandomSpawnPointSelector());

            var playerTransform =
                player != null ? player.transform : null;

            // Обработчик логики появления босса после убийств
            _bossSpawnHandler = new BossSpawnOnKillHandler(
                eventBus,
                settings,
                bossSpawnService,
                enemyRegistry,
                spawnPoints,
                playerTransform,
                new System.Random());

            // Обработчик победной музыки
            _victoryMusicHandler = new VictoryMusicOnKillHandler(
                eventBus,
                settings,
                audioService);
        }

        // Регистрирует врага в системе событий смерти
        public void RegisterEnemy(BaseEnemyView enemyView)
        {
            _deathPublisher.Register(enemyView);
        }
    }
}