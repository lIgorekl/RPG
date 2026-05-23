using System;
using UnityEngine;

namespace Gameplay.Combat.Weapons
{
    [CreateAssetMenu(
        fileName = "EnemyWeaponLoadout",
        menuName = "RPG/Combat/Enemy Weapon Loadout")]
    public class EnemyWeaponLoadout : ScriptableObject
    {
        [SerializeField] private EnemyWeaponConfig[] weapons;

        public EnemyWeaponConfig PickRandom(System.Random random)
        {
            if (weapons == null || weapons.Length == 0)
                return null;

            int index = random.Next(0, weapons.Length);
            return weapons[index];
        }
    }
}
