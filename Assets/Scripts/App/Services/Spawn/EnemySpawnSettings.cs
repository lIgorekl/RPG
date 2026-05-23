using UnityEngine;

namespace App.Services.Spawn
{
    [CreateAssetMenu(
        fileName = "EnemySpawnSettings",
        menuName = "RPG/Spawn/Enemy Spawn Settings")]
    public class EnemySpawnSettings : ScriptableObject
    {
        [SerializeField] private int minSpawnCount = 3;
        [SerializeField] private int maxSpawnCount = 7;

        public int MinSpawnCount => Mathf.Max(1, minSpawnCount);
        public int MaxSpawnCount => Mathf.Max(MinSpawnCount, maxSpawnCount);
    }
}
