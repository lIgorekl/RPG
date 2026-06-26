using Presentation.Scene;
using UnityEngine;

namespace App.Services.Spawn
{
    // Интерфейс фабрики создания врагов.
    // Определяет контракт для классов,
    // которые умеют создавать и инициализировать врагов.
    public interface IEnemyFactory
    {
        // Создает нового врага по описанию спавна
        BaseEnemyView Create(
            EnemySpawnDefinition definition,
            Vector3 position,
            Quaternion rotation,
            string enemyId,
            Transform player,
            System.Random random);
    }
}