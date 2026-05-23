using UnityEngine;

namespace Presentation.AI
{
    public class FleeState : IEnemyState
    {
        private readonly EnemyStateMachineBase _stateMachine;

        private BaseCombatEnemyBehaviour Behaviour =>
            (BaseCombatEnemyBehaviour)_stateMachine.Behaviour;

        private float _recalculateTimer;

        private const float FleeDistance = 10f;
        private const float RecalculateDelay = 1.5f;

        public FleeState(
            EnemyStateMachineBase stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            Behaviour.EnterFlee();

            _recalculateTimer = 0f;
        }

        public void Update()
        {
            var entity = Behaviour.EnemyView.GetEntity();

            float hpPercent =
                (float)entity.CurrentHP / entity.MaxHP;

            if (hpPercent >= 0.3f)
            {
                _stateMachine.EnterDefaultState();
                return;
            }

            _recalculateTimer -= Time.deltaTime;

            if (_recalculateTimer <= 0f ||
                Behaviour.Agent.velocity.magnitude < 0.1f)
            {
                Vector3 direction =
                    Behaviour.Self.position -
                    Behaviour.Player.position;

                if (direction.sqrMagnitude < 0.01f)
                {
                    direction =
                        Random.insideUnitSphere;

                    direction.y = 0f;
                }

                direction.Normalize();

                Behaviour.MovementService.TryMoveToRandomDirection(
                    Behaviour.Agent,
                    Behaviour.Self,
                    direction,
                    FleeDistance);

                _recalculateTimer =
                    RecalculateDelay;
            }

            float fleeExitDistance =
                Behaviour.DetectionRadius * 1.5f;

            if (Behaviour.CombatEvaluator.IsTargetLost(
                Behaviour.Self,
                Behaviour.Player,
                fleeExitDistance))
            {
                _stateMachine.EnterDefaultState();
            }
        }

        public void Exit() { }
    }
}
