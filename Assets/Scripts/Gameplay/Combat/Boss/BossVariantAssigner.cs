using UnityEngine;

namespace Gameplay.Combat.Boss
{
    public static class BossVariantAssigner
    {
        public static void AssignRandomVariants(
            GameObject enemyInstance,
            System.Random random)
        {
            if (enemyInstance == null || random == null)
                return;

            var components = enemyInstance.GetComponents<MonoBehaviour>();

            foreach (var component in components)
            {
                if (component is IBossVariantAssignable assignable)
                    assignable.AssignRandomVariants(random);
            }
        }
    }
}
