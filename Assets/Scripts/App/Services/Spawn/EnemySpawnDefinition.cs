using System.Linq;
using UnityEngine;
using App.Services;

namespace App.Services.Spawn
{
    // Боевая роль врага.
    // Используется системой спавна для гарантированного
    // появления ближних и дальних противников.
    public enum EnemySpawnCombatRole
    {
        // Враг ближнего боя
        Melee,

        // Враг дальнего боя
        Ranged
    }

    // Описание типа врага для системы спавна.
    // Хранится как ScriptableObject и содержит все данные,
    // необходимые для создания врага.
    [CreateAssetMenu(
        fileName = "EnemySpawnDefinition",
        menuName = "RPG/Spawn/Enemy Spawn Definition")]
    public class EnemySpawnDefinition : ScriptableObject
    {
        // Префаб врага
        [SerializeField] private GameObject prefab;

        // Боевая роль врага
        [SerializeField] private EnemySpawnCombatRole combatRole =
            EnemySpawnCombatRole.Melee;

        // Вес при случайном выборе
        [SerializeField] private int spawnWeight = 1;

        // Режимы игры,
        // в которых разрешено появление врага
        [SerializeField] private GameMode[] allowedGameModes =
        {
            GameMode.Normal,
            GameMode.Peaceful
        };

        // Является ли данный враг боссом
        [SerializeField] private bool isBoss;

        // Префикс для генерации уникального ID
        [SerializeField] private string idPrefix = "enemy";

        // Префаб врага
        public GameObject Prefab => prefab;

        // Боевая роль
        public EnemySpawnCombatRole CombatRole =>
            combatRole;

        // Вес спавна.
        // Не может быть меньше единицы
        public int SpawnWeight =>
            Mathf.Max(1, spawnWeight);

        // Является ли враг боссом
        public bool IsBoss =>
            isBoss;

        // Префикс уникального идентификатора
        public string IdPrefix =>
            idPrefix;

        // Проверяет,
        // разрешено ли создавать данного врага
        // в указанном режиме игры
        public bool IsAllowedIn(GameMode mode)
        {
            // Если список режимов пуст,
            // считаем,
            // что враг разрешен всегда
            if (allowedGameModes == null ||
                allowedGameModes.Length == 0)
            {
                return true;
            }

            // Проверяем,
            // содержится ли текущий режим
            // в списке разрешенных
            return allowedGameModes.Contains(mode);
        }
    }
}