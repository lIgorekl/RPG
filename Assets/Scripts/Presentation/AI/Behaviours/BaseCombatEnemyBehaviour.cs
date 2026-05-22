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

                /*
         * =========================
         * COMMON STATES
         * =========================
         */

        public void EnterDefaultState()
        {
            StateMachine.EnterDefaultState(this);
        }

        public void TickStun(
            ref float timer)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                EnterDefaultState();
            }
        }

        public void TickFlee(
            ref float recalculateTimer,
            float fleeDistance,
            float recalculateDelay)
        {
            var entity = EnemyView.GetEntity();

            float hpPercent =
                (float)entity.CurrentHP / entity.MaxHP;

            // HP восстановился
            if (hpPercent >= 0.3f)
            {
                EnterDefaultState();
                return;
            }

            recalculateTimer -= Time.deltaTime;

            if (recalculateTimer <= 0f ||
                Agent.velocity.magnitude < 0.1f)
            {
                Vector3 direction =
                    Self.position - Player.position;

                // overlap protection
                if (direction.sqrMagnitude < 0.01f)
                {
                    direction =
                        Random.insideUnitSphere;

                    direction.y = 0f;
                }

                direction.Normalize();

                MovementService.TryMoveToRandomDirection(
                    Agent,
                    Self,
                    direction,
                    fleeDistance);

                recalculateTimer =
                    recalculateDelay;
            }

            float fleeExitDistance =
                DetectionRadius * 1.5f;

            if (CombatEvaluator.IsTargetLost(
                Self,
                Player,
                fleeExitDistance))
            {
                EnterDefaultState();
            }
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
    }
}