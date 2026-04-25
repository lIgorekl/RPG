using UnityEngine;

namespace Presentation.AI
{
    // Состояние преследования игрока.
    // Враг бежит за игроком, пока тот находится в радиусе обнаружения.
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

            // Если игрок слишком далеко — возвращаемся в Idle
            if (distance > _behaviour.DetectionRadius)
            {
                _behaviour.StateMachine.ChangeState(
                    new IdleState(_behaviour));
                return;
            }

            // Если игрок рядом — начинаем атаку
            if (distance <= _behaviour.AttackRadius)
            {
                _behaviour.StateMachine.ChangeState(
                    new AttackState(_behaviour));
                return;
            }

            // Направление на игрока
            Vector3 direction =
                _behaviour.Player.position - _behaviour.Self.position;

            direction.y = 0f;

            // Плавный поворот к игроку
            if (direction.sqrMagnitude > 0.01f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);

                _behaviour.Self.rotation =
                    Quaternion.Slerp(
                        _behaviour.Self.rotation,
                        lookRotation,
                        10f * Time.deltaTime);
            }

            // Движение через NavMesh
            _behaviour.Agent.SetDestination(_behaviour.Player.position);
        }

        public void Exit() { }
    }
}