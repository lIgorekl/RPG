using Presentation.Scene;
using UnityEngine;

namespace App.Services.Spawn
{
    // Контракт сервиса спавна босса.
    // Определяет единый способ создания босса.
    public interface IBossSpawnService
    {
        // Создает босса по указанному описанию
        BaseEnemyView SpawnBoss(
            EnemySpawnDefinition bossDefinition,
            SpawnPoint[] spawnPoints,
            Transform player,
            System.Random random);
    }
}