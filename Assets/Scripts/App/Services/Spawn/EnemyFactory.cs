using Gameplay.Combat.Boss;
using Gameplay.Combat.Weapons;
using Presentation.AI;
using Presentation.Scene;
using UnityEngine;

namespace App.Services.Spawn
{
    public class EnemyFactory : IEnemyFactory
    {
        public BaseEnemyView Create(
            EnemySpawnDefinition definition,
            Vector3 position,
            Quaternion rotation,
            string enemyId,
            Transform player,
            System.Random random)
        {
            var instance = Object.Instantiate(
                definition.Prefab,
                position,
                rotation);

            var view = instance.GetComponent<BaseEnemyView>();

            if (view == null && definition.IsBoss)
                view = instance.AddComponent<BossEnemyView>();

            if (random != null)
            {
                EnemyWeaponAssigner.AssignRandomWeapon(instance, random);
                BossVariantAssigner.AssignRandomVariants(instance, random);
            }

            if (view == null)
            {
                Object.Destroy(instance);
                Debug.LogError(
                    $"EnemyFactory: prefab '{definition.Prefab?.name}' " +
                    $"(spawn def '{definition.name}') has no BaseEnemyView.");
                return null;
            }

            view.SetEnemyId(enemyId);

            var behaviour = instance.GetComponent<BaseEnemyBehaviour>();
            if (behaviour != null && player != null)
                behaviour.SetPlayer(player);

            return view;
        }
    }
}
