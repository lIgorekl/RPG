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

            _behaviour.Agent.ResetPath();
            _behaviour.Agent.isStopped = false;
        }

        public void Update()
        {
            var entity = _behaviour.EnemyView.GetEntity();

            float hpPercent =
                (float)entity.CurrentHP / entity.MaxHP;

            float distanceToPlayer = Vector3.Distance(
                _behaviour.Self.position,
                _behaviour.Player.position);

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

                Vector3 target =
                    _behaviour.Self.position + direction * _fleeDistance;

                if (NavMesh.SamplePosition(target, out NavMeshHit hit, 2f, NavMesh.AllAreas))
                {
                    _behaviour.Agent.SetDestination(hit.position);
                }

                _recalculateTimer = 1.5f;
            }

            // Если убежал далеко — вернуться в обычное состояние
            float fleeExitDistance = _behaviour.DetectionRadius * 1.5f;

            if (distanceToPlayer > fleeExitDistance)
            {
                ReturnToDefaultState();
            }
        }

        private void ReturnToDefaultState()
        {
            if (_behaviour is EnemyBehaviour melee)
            {
                _behaviour.StateMachine.ChangeState(new IdleState(melee));
            }
            else if (_behaviour is RangedEnemyBehaviour ranged)
            {
                _behaviour.StateMachine.ChangeState(new RangedIdleState(ranged));
            }
        }

        public void Exit() { }
    }
}