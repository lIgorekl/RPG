using UnityEngine;
using App.Services;

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
            _behaviour.MovementService.Stop(_behaviour.Agent);
        }

        public void Update()
        {
            var entity = _behaviour.EnemyView.GetEntity();

            // Если игрок вышел из радиуса атаки — начинаем преследование
            if (!_behaviour.CombatEvaluator.CanMeleeAttack(
                _behaviour.Self,
                _behaviour.Player,
                _behaviour.AttackRadius))
            {
                _behaviour.StateMachine.ChangeState(
                    _behaviour.StateFactory.CreateChase(_behaviour));
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
            _behaviour.MovementService.Resume(_behaviour.Agent);
        }
    }
}