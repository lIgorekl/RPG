using UnityEngine;

namespace Gameplay.AI
{
    // Вспомогательный класс для проверки боевых условий
    // Используется AI врагов для принятия решений
    public class EnemyCombatEvaluator
    {
        // Проверяет находится ли цель в радиусе обнаружения
        public bool IsTargetDetected(
            Transform self,
            Transform target,
            float detectionRadius)
        {
            return Distance(self, target) <= detectionRadius;
        }

        // Проверяет может ли враг атаковать цель в ближнем бою
        public bool CanMeleeAttack(
            Transform self,
            Transform target,
            float attackRadius)
        {
            return Distance(self, target) <= attackRadius;
        }

        // Проверяет потерял ли враг цель
        public bool IsTargetLost(
            Transform self,
            Transform target,
            float detectionRadius)
        {
            return Distance(self, target) > detectionRadius;
        }

        // Проверяет находится ли цель слишком близко
        public bool IsTooClose(
            Transform self,
            Transform target,
            float minDistance)
        {
            return Distance(self, target) < minDistance;
        }

        // Проверяет находится ли цель слишком далеко
        public bool IsTooFar(
            Transform self,
            Transform target,
            float maxDistance)
        {
            return Distance(self, target) > maxDistance;
        }

        // Проверяет находится ли цель в заданном диапазоне дистанций
        public bool IsInsideRange(
            Transform self,
            Transform target,
            float minDistance,
            float maxDistance)
        {
            float distance = Distance(self, target);

            return distance >= minDistance &&
                   distance <= maxDistance;
        }

        // Вычисляет расстояние между двумя объектами
        private float Distance(
            Transform self,
            Transform target)
        {
            return Vector3.Distance(
                self.position,
                target.position);
        }
    }
}