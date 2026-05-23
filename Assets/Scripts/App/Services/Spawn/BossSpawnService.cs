using Presentation.Scene;
using UnityEngine;

namespace App.Services.Spawn
{
    public class BossSpawnService : IBossSpawnService
    {
        private readonly IEnemyFactory _enemyFactory;
        private readonly ISpawnPointSelector _spawnPointSelector;

        public BossSpawnService(
            IEnemyFactory enemyFactory,
            ISpawnPointSelector spawnPointSelector)
        {
            _enemyFactory = enemyFactory;
            _spawnPointSelector = spawnPointSelector;
        }

        public BaseEnemyView SpawnBoss(
            EnemySpawnDefinition bossDefinition,
            SpawnPoint[] spawnPoints,
            Transform player,
            System.Random random)
        {
            if (bossDefinition == null ||
                spawnPoints == null ||
                spawnPoints.Length == 0)
            {
                return null;
            }

            var selectedPoints = _spawnPointSelector.Select(
                spawnPoints,
                1,
                random);

            if (selectedPoints.Count == 0)
                return null;

            var spawnPoint = selectedPoints[0];
            string enemyId = $"{bossDefinition.IdPrefix}_event";

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
