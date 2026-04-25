using UnityEngine;

namespace Presentation.AI
{
    // Состояние удержания дистанции для дальнего врага.
    // Враг старается держаться между MinDistance и MaxDistance от игрока.
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

            // Если игрок слишком далеко — возвращаемся в Idle
            if (distance > _behaviour.DetectionRadius)
            {
                _behaviour.StateMachine.ChangeState(
                    new RangedIdleState(_behaviour));
                return;
            }

            // Направление к игроку
            Vector3 direction =
                _behaviour.Player.position - _behaviour.Self.position;

            direction.y = 0f;
            direction.Normalize();

            // Если игрок слишком близко — отходим назад
            if (distance < _behaviour.MinDistance)
            {
                Vector3 target =
                    _behaviour.Self.position - direction * 2f;

                _behaviour.Agent.SetDestination(target);
            }
            // Если игрок слишком далеко — подходим ближе
            else if (distance > _behaviour.MaxDistance)
            {
                Vector3 target =
                    _behaviour.Self.position + direction * 2f;

                _behaviour.Agent.SetDestination(target);
            }
            // Если дистанция подходящая — начинаем атаковать
            else
            {
                _behaviour.StateMachine.ChangeState(
                    new RangedAttackState(_behaviour));
            }

            // Всегда поворачиваемся к игроку
            _behaviour.Self.rotation =
                Quaternion.LookRotation(direction);
        }

        public void Exit() { }
    }
}