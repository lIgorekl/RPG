using UnityEngine;

namespace Presentation.AI
{
    // Состояние атаки для врага ближнего боя.
    // Враг останавливается и атакует игрока с заданным кулдауном.
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

            // Останавливаем движение во время атаки
            if (_behaviour.Agent != null)
                _behaviour.Agent.isStopped = true;
        }

        public void Update()
        {
            float distance = Vector3.Distance(
                _behaviour.Self.position,
                _behaviour.Player.position);

            // Если игрок вышел из радиуса атаки — начинаем преследование
            if (distance > _behaviour.AttackRadius)
            {
                _behaviour.StateMachine.ChangeState(
                    new ChaseState(_behaviour));
                return;
            }

            // Таймер атаки
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
            // Возвращаем управление NavMeshAgent
            if (_behaviour.Agent != null)
                _behaviour.Agent.isStopped = false;
        }
    }
}