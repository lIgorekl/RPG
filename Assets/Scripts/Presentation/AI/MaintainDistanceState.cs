using UnityEngine;

namespace Presentation.AI
{
    public class MaintainDistanceState : IEnemyState
    {
        private readonly RangedEnemyBehaviour _behaviour;

        public MaintainDistanceState(RangedEnemyBehaviour behaviour)
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
                    new RangedIdleState(_behaviour));
                return;
            }

            Vector3 direction =
                (_behaviour.Player.position - _behaviour.Self.position).normalized;

            direction.y = 0f;

            if (distance < _behaviour.MinDistance)
            {
                // Отступаем
                _behaviour.Self.position -=
                    direction * _behaviour.MoveSpeed * Time.deltaTime;
            }
            else if (distance > _behaviour.MaxDistance)
            {
                // Подходим ближе
                _behaviour.Self.position +=
                    direction * _behaviour.MoveSpeed * Time.deltaTime;
            }
            else
            {
                _behaviour.StateMachine.ChangeState(
                    new RangedAttackState(_behaviour));
            }

            _behaviour.Self.rotation =
                Quaternion.LookRotation(direction);
        }

        public void Exit() { }
    }
}