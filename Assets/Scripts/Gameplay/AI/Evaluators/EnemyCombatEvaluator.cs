using UnityEngine;

namespace Gameplay.AI
{
    public class EnemyCombatEvaluator
    {
        public bool IsTargetDetected(
            Transform self,
            Transform target,
            float detectionRadius)
        {
            return Distance(self, target) <= detectionRadius;
        }

        public bool CanMeleeAttack(
            Transform self,
            Transform target,
            float attackRadius)
        {
            return Distance(self, target) <= attackRadius;
        }

        public bool IsTargetLost(
            Transform self,
            Transform target,
            float detectionRadius)
        {
            return Distance(self, target) > detectionRadius;
        }

        public bool IsTooClose(
            Transform self,
            Transform target,
            float minDistance)
        {
            return Distance(self, target) < minDistance;
        }

        public bool IsTooFar(
            Transform self,
            Transform target,
            float maxDistance)
        {
            return Distance(self, target) > maxDistance;
        }

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