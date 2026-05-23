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
        [SerializeField] [Range(0f, 1f)] private float bossSpawnChance = 0.25f;

        public int MinSpawnCount => Mathf.Max(1, minSpawnCount);
        public int MaxSpawnCount => Mathf.Max(MinSpawnCount, maxSpawnCount);
        public float BossSpawnChance => Mathf.Clamp01(bossSpawnChance);
    }
}
