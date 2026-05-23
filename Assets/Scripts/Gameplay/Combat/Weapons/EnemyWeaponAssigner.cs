using UnityEngine;

namespace Gameplay.Combat.Weapons
{
    public static class EnemyWeaponAssigner
    {
        public static void AssignRandomWeapon(
            GameObject enemyInstance,
            System.Random random)
        {
            if (enemyInstance == null || random == null)
                return;

            var components = enemyInstance.GetComponents<MonoBehaviour>();

            foreach (var component in components)
            {
                if (component is IEnemyWeaponAssignable assignable)
                    assignable.AssignRandomWeapon(random);
            }
        }
    }
}
