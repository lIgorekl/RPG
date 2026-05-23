using Presentation.Scene;
using UnityEngine;

namespace App.Services.Spawn
{
    public interface IEnemyFactory
    {
        BaseEnemyView Create(
            EnemySpawnDefinition definition,
            Vector3 position,
            Quaternion rotation,
            string enemyId,
            Transform player,
            System.Random random);
    }
}
