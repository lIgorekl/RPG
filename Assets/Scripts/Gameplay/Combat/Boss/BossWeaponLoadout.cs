using System;
using Gameplay.Combat.Weapons;
using UnityEngine;

namespace Gameplay.Combat.Boss
{
    // Набор возможного оружия босса.
    // Хранится как ScriptableObject и используется
    // для случайного выбора оружия при создании босса.
    [CreateAssetMenu(
        fileName = "BossWeaponLoadout",
        menuName = "RPG/Combat/Boss Weapon Loadout")]
    public class BossWeaponLoadout : ScriptableObject
    {
        // Список доступных конфигураций оружия
        [SerializeField] private EnemyWeaponConfig[] weapons;

        // Возвращает случайную конфигурацию оружия
        public EnemyWeaponConfig PickRandom(System.Random random)
        {
            // Если список пуст, выбрать оружие невозможно
            if (weapons == null || weapons.Length == 0)
                return null;

            // Выбираем случайный индекс массива
            int index = random.Next(0, weapons.Length);

            // Возвращаем выбранную конфигурацию оружия
            return weapons[index];
        }
    }
}