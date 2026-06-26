using UnityEngine;

namespace Gameplay.Combat.Boss
{
    // Вспомогательный класс для назначения случайных
    // вариантов босса (оружие, стихия и т.д.)
    public static class BossVariantAssigner
    {
        // Находит все компоненты, которые умеют
        // назначать варианты босса, и вызывает их
        public static void AssignRandomVariants(
            GameObject enemyInstance,
            System.Random random)
        {
            // Проверяем корректность входных данных
            if (enemyInstance == null || random == null)
                return;

            // Получаем все MonoBehaviour на объекте
            var components = enemyInstance.GetComponents<MonoBehaviour>();

            // Перебираем каждый компонент
            foreach (var component in components)
            {
                // Если компонент поддерживает назначение вариантов,
                // вызываем соответствующий метод
                if (component is IBossVariantAssignable assignable)
                    assignable.AssignRandomVariants(random);
            }
        }
    }
}