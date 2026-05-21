using UnityEngine;
using UnityEngine.AI;

namespace Presentation.AI
{
    public class FleeState : IEnemyState
    {
        private readonly BaseEnemyBehaviour _behaviour;

        private float _fleeDistance = 10f;
        private float _recalculateTimer = 1.5f;

        public FleeState(BaseEnemyBehaviour behaviour)
        {
            _behaviour = behaviour;
        }

        public void Enter()
        {
            if (_behaviour.Agent == null)
                return;

            _behaviour.MovementService.ResetPath(_behaviour.Agent);
            _behaviour.MovementService.Resume(_behaviour.Agent);
        }

        public void Update()
        {
            var entity = _behaviour.EnemyView.GetEntity();

            float hpPercent =
                (float)entity.CurrentHP / entity.MaxHP;

            // ВЫХОД ЕСЛИ HP НОРМ
            if (hpPercent >= 0.3f)
            {
                ReturnToDefaultState();
                return;
            }

            _recalculateTimer -= Time.deltaTime;

            if (_recalculateTimer <= 0f || _behaviour.Agent.velocity.magnitude < 0.1f)
            {
                Vector3 direction =
                    _behaviour.Self.position - _behaviour.Player.position;

                // ЕСЛИ СЛИШКОМ БЛИЗКО — ДАЁМ СЛУЧАЙНОЕ НАПРАВЛЕНИЕ
                if (direction.sqrMagnitude < 0.01f)
                {
                    direction = Random.insideUnitSphere;
                    direction.y = 0f;
                }

                direction.Normalize();

                _behaviour.MovementService.TryMoveToRandomDirection(
                    _behaviour.Agent,
                    _behaviour.Self,
                    direction,
                    _fleeDistance);

                _recalculateTimer = 1.5f;
            }

            // Если убежал далеко — вернуться в обычное состояние
            float fleeExitDistance = _behaviour.DetectionRadius * 1.5f;

            if (_behaviour.CombatEvaluator.IsTargetLost(
                _behaviour.Self,
                _behaviour.Player,
                fleeExitDistance))
            {
                ReturnToDefaultState();
            }
        }

        private void ReturnToDefaultState()
        {
            _behaviour.StateMachine.ChangeState(
                _behaviour.CreateDefaultState());
        }

        public void Exit() { }
    }
}