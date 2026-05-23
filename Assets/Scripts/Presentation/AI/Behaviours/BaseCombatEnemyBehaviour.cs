using UnityEngine;

namespace Presentation.AI
{
    public abstract class BaseCombatEnemyBehaviour
        : BaseEnemyBehaviour
    {
        public bool ShouldFlee()
        {
            var entity = EnemyView.GetEntity();

            float hpPercent =
                (float)entity.CurrentHP / entity.MaxHP;

            return hpPercent < 0.3f;
        }

        protected void RotateToPlayer()
        {
            Vector3 direction =
                Player.position - Self.position;

            direction.y = 0f;

            if (direction.sqrMagnitude < 0.01f)
                return;

            Self.rotation =
                Quaternion.LookRotation(direction);
        }

        public void UpdateIdle(
            ref float wanderTimer,
            float wanderDelay)
        {
            wanderTimer -= Time.deltaTime;

            if (wanderTimer <= 0f)
            {
                WanderService.TryWander(
                    Agent,
                    Self,
                    5f);

                wanderTimer = wanderDelay;
            }
        }

        protected bool IsTargetDetected()
        {
            return CombatEvaluator.IsTargetDetected(
                Self,
                Player,
                DetectionRadius);
        }

        public void EnterFlee()
        {
            MovementService.ResetPath(Agent);
            MovementService.Resume(Agent);
        }

        public void EnterStun()
        {
            MovementService.Stop(Agent);
        }

        public void ExitStun()
        {
            MovementService.Resume(Agent);
        }

        public virtual void EnterStunState(float duration)
        {
        }
    }
}
