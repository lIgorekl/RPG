using UnityEngine;

namespace Gameplay.Combat.Weapons
{
    // Вспомогательный класс для назначения
    // случайного оружия врагам
    public static class EnemyWeaponAssigner
    {
        // Находит все компоненты, которые поддерживают
        // назначение оружия, и вызывает у них соответствующий метод
        public static void AssignRandomWeapon(
            GameObject enemyInstance,
            System.Random random)
        {
            // Проверяем корректность входных данных
            if (enemyInstance == null || random == null)
                return;

            // Получаем все компоненты MonoBehaviour,
            // находящиеся на игровом объекте
            var components = enemyInstance.GetComponents<MonoBehaviour>();

            // Перебираем каждый компонент
            foreach (var component in components)
            {
                // Если компонент умеет получать случайное оружие,
                // назначаем его
                if (component is IEnemyWeaponAssignable assignable)
                    assignable.AssignRandomWeapon(random);
            }
        }
    }
}