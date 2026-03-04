using UnityEngine;

namespace Presentation.AI
{
    public class ChaseState : IEnemyState
    {
        private readonly EnemyBehaviour _behaviour;

        public ChaseState(EnemyBehaviour behaviour)
        {
            _behaviour = behaviour;
        }

        public void Enter() { }

        public void Update()
        {
            float distance = Vector3.Distance(
                _behaviour.Self.position,
                _behaviour.Player.position);

            if (distance > _behaviour.DetectionRadius)
            {
                _behaviour.StateMachine.ChangeState(
                    new IdleState(_behaviour));
                return;
            }

            if (distance <= _behaviour.AttackRadius)
            {
                _behaviour.StateMachine.ChangeState(
                    new AttackState(_behaviour));
                return;
            }

            Vector3 direction =
                (_behaviour.Player.position - _behaviour.Self.position).normalized;

            direction.y = 0f;

            _behaviour.Self.position +=
                direction * _behaviour.MoveSpeed * Time.deltaTime;

            _behaviour.Self.rotation =
                Quaternion.LookRotation(direction);
        }

        public void Exit() { }
    }
}