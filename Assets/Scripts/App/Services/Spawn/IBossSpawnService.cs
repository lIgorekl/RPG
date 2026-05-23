using Presentation.Scene;
using UnityEngine;

namespace App.Services.Spawn
{
    public interface IBossSpawnService
    {
        BaseEnemyView SpawnBoss(
            EnemySpawnDefinition bossDefinition,
            SpawnPoint[] spawnPoints,
            Transform player,
            System.Random random);
    }
}
