using System;
using Gameplay.Combat.Weapons;
using UnityEngine;

namespace Gameplay.Combat.Boss
{
    [CreateAssetMenu(
        fileName = "BossWeaponLoadout",
        menuName = "RPG/Combat/Boss Weapon Loadout")]
    public class BossWeaponLoadout : ScriptableObject
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
