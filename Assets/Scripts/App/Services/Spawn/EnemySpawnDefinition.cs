using System.Linq;
using UnityEngine;
using App.Services;

namespace App.Services.Spawn
{
    public enum EnemySpawnCombatRole
    {
        Melee,
        Ranged
    }

    [CreateAssetMenu(
        fileName = "EnemySpawnDefinition",
        menuName = "RPG/Spawn/Enemy Spawn Definition")]
    public class EnemySpawnDefinition : ScriptableObject
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private EnemySpawnCombatRole combatRole =
            EnemySpawnCombatRole.Melee;
        [SerializeField] private int spawnWeight = 1;
        [SerializeField] private GameMode[] allowedGameModes =
        {
            GameMode.Normal,
            GameMode.Peaceful
        };
        [SerializeField] private bool isBoss;
        [SerializeField] private string idPrefix = "enemy";

        public GameObject Prefab => prefab;
        public EnemySpawnCombatRole CombatRole => combatRole;
        public int SpawnWeight => Mathf.Max(1, spawnWeight);
        public bool IsBoss => isBoss;
        public string IdPrefix => idPrefix;

        public bool IsAllowedIn(GameMode mode)
        {
            if (allowedGameModes == null || allowedGameModes.Length == 0)
                return true;

            return allowedGameModes.Contains(mode);
        }
    }
}
