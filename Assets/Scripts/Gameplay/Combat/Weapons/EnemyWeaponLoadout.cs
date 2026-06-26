using System;
using UnityEngine;

namespace Gameplay.Combat.Weapons
{
    // Набор возможного оружия для врага.
    // Хранится как ScriptableObject и используется
    // для случайного выбора оружия при создании врага.
    [CreateAssetMenu(
        fileName = "EnemyWeaponLoadout",
        menuName = "RPG/Combat/Enemy Weapon Loadout")]
    public class EnemyWeaponLoadout : ScriptableObject
    {
        // Список доступных вариантов оружия
        [SerializeField] private EnemyWeaponConfig[] weapons;

        // Возвращает случайную конфигурацию оружия
        public EnemyWeaponConfig PickRandom(System.Random random)
        {
            // Если список пуст, вернуть нечего
            if (weapons == null || weapons.Length == 0)
                return null;

            // Выбираем случайный индекс массива
            int index = random.Next(0, weapons.Length);

            // Возвращаем выбранную конфигурацию оружия
            return weapons[index];
        }
    }
}