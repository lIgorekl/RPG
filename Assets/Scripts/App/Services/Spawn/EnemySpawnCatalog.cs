using UnityEngine;

namespace App.Services.Spawn
{
    [CreateAssetMenu(
        fileName = "EnemySpawnCatalog",
        menuName = "RPG/Spawn/Enemy Spawn Catalog")]
    public class EnemySpawnCatalog : ScriptableObject
    {
        [SerializeField] private EnemySpawnDefinition[] definitions;

        public EnemySpawnDefinition[] Definitions => definitions;
    }
}
