using Presentation.Scene;
using UnityEngine;

namespace App.Services.Spawn
{
    // Сервис спавна босса.
    // Отвечает за выбор точки появления и создание босса
    // через фабрику.
    public class BossSpawnService : IBossSpawnService
    {
        // Фабрика создания врагов
        private readonly IEnemyFactory _enemyFactory;

        // Стратегия выбора точки появления
        private readonly ISpawnPointSelector _spawnPointSelector;

        public BossSpawnService(
            IEnemyFactory enemyFactory,
            ISpawnPointSelector spawnPointSelector)
        {
            _enemyFactory = enemyFactory;
            _spawnPointSelector = spawnPointSelector;
        }

        // Создает босса
        public BaseEnemyView SpawnBoss(
            EnemySpawnDefinition bossDefinition,
            SpawnPoint[] spawnPoints,
            Transform player,
            System.Random random)
        {
            // Проверяем корректность входных данных
            if (bossDefinition == null ||
                spawnPoints == null ||
                spawnPoints.Length == 0)
            {
                return null;
            }

            // Выбираем одну случайную точку появления
            var selectedPoints =
                _spawnPointSelector.Select(
                    spawnPoints,
                    1,
                    random);

            // Если подходящая точка не найдена,
            // создать босса невозможно
            if (selectedPoints.Count == 0)
                return null;

            // Получаем выбранную точку
            var spawnPoint =
                selectedPoints[0];

            // Формируем уникальный идентификатор босса
            string enemyId =
                $"{bossDefinition.IdPrefix}_event";

            // Создаем босса через фабрику
            return _enemyFactory.Create(
                bossDefinition,
                spawnPoint.Position,
                spawnPoint.Rotation,
                enemyId,
                player,
                random);
        }
    }
}