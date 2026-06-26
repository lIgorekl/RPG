using Gameplay.Combat.Boss;
using Gameplay.Combat.Weapons;
using Presentation.AI;
using Presentation.Scene;
using UnityEngine;

namespace App.Services.Spawn
{
    // Фабрика создания врагов.
    // Создает экземпляр врага, назначает ему параметры
    // и подготавливает к работе в игре.
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
            // Создаем объект врага по префабу
            var instance = Object.Instantiate(
                definition.Prefab,
                position,
                rotation);

            // Получаем компонент представления врага
            var view = instance.GetComponent<BaseEnemyView>();

            // Если создается босс, а компонента нет,
            // добавляем его автоматически
            if (view == null && definition.IsBoss)
                view = instance.AddComponent<BossEnemyView>();

            // Назначаем случайное оружие и варианты босса
            if (random != null)
            {
                EnemyWeaponAssigner.AssignRandomWeapon(instance, random);
                BossVariantAssigner.AssignRandomVariants(instance, random);
            }

            // Проверяем, что объект действительно является врагом
            if (view == null)
            {
                Object.Destroy(instance);

                Debug.LogError(
                    $"EnemyFactory: prefab '{definition.Prefab?.name}' " +
                    $"(spawn def '{definition.name}') has no BaseEnemyView.");

                return null;
            }

            // Присваиваем уникальный идентификатор врагу
            view.SetEnemyId(enemyId);

            // Передаем врагу ссылку на игрока
            var behaviour = instance.GetComponent<BaseEnemyBehaviour>();

            if (behaviour != null && player != null)
                behaviour.SetPlayer(player);

            return view;
        }
    }
}