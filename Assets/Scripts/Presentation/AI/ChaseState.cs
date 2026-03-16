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
                _behaviour.Player.position - _behaviour.Self.position;

            direction.y = 0f;

            if (direction.sqrMagnitude > 0.01f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);

                _behaviour.Self.rotation =
                    Quaternion.Slerp(
                        _behaviour.Self.rotation,
                        lookRotation,
                        10f * Time.deltaTime);
            }

            _behaviour.Agent.SetDestination(_behaviour.Player.position);
        }

        public void Exit() { }
    }
}