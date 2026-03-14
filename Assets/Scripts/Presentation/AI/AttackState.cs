using UnityEngine;
using Core.Combat;

namespace Presentation.AI
{
    public class AttackState : IEnemyState
    {
        private readonly EnemyBehaviour _behaviour;
        private float _attackCooldown = 1f;
        private float _timer;

        public AttackState(EnemyBehaviour behaviour)
        {
            _behaviour = behaviour;
        }

        public void Enter()
        {
            _timer = _attackCooldown;

            if (_behaviour.Agent != null)
            {
                _behaviour.Agent.isStopped = true;
            }
        }

        public void Update()
        {
            float distance = Vector3.Distance(
                _behaviour.Self.position,
                _behaviour.Player.position);

            if (distance > _behaviour.AttackRadius)
            {
                _behaviour.StateMachine.ChangeState(
                    new ChaseState(_behaviour));
                return;
            }

            _timer -= Time.deltaTime;

            if (_timer <= 0f)
            {
                TryAttack();
                _timer = _attackCooldown;
            }
        }

        private void TryAttack()
        {
            _behaviour.EnemyView.Attack(_behaviour.Player);
        }

        public void Exit()
        {
            if (_behaviour.Agent != null)
            {
                _behaviour.Agent.isStopped = false;
            }
        }
    }
}