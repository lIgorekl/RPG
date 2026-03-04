using UnityEngine;

namespace Presentation.AI
{
    public class IdleState : IEnemyState
    {
        private readonly EnemyBehaviour _behaviour;

        public IdleState(EnemyBehaviour behaviour)
        {
            _behaviour = behaviour;
        }

        public void Enter() { }

        public void Update()
        {
            float distance = Vector3.Distance(
                _behaviour.Self.position,
                _behaviour.Player.position);

            if (distance <= _behaviour.DetectionRadius)
            {
                _behaviour.StateMachine.ChangeState(
                    new ChaseState(_behaviour));
            }
        }

        public void Exit() { }
    }
}