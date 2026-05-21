using UnityEngine;
using App.Services;

namespace Presentation.AI
{
    // Состояние атаки дальнего врага.
    // Враг останавливается, поворачивается к игроку и стреляет с кулдауном.
    public class RangedAttackState : IEnemyState
    {
        private readonly RangedEnemyBehaviour _behaviour;

        private float _cooldown = 2f;
        private float _timer;

        public RangedAttackState(RangedEnemyBehaviour behaviour)
        {
            _behaviour = behaviour;
        }

        public void Enter()
        {
            _timer = _cooldown;

            // Останавливаем движение во время атаки
            _behaviour.MovementService.Stop(_behaviour.Agent);
        }

        public void Update()
        {
            var entity = _behaviour.EnemyView.GetEntity();

            // Если дистанция изменилась — возвращаемся к удержанию дистанции
            if (!_behaviour.CombatEvaluator.IsInsideRange(
                _behaviour.Self,
                _behaviour.Player,
                _behaviour.MinDistance,
                _behaviour.MaxDistance))
            {
                _behaviour.StateMachine.ChangeState(
                    _behaviour.StateFactory.CreateMaintainDistance(_behaviour));
                return;
            }

            // Поворачиваемся к игроку
            _behaviour.MovementService.RotateTo(
                _behaviour.Self,
                _behaviour.Player.position);

            // Таймер атаки
            _timer -= Time.deltaTime;

            if (_timer <= 0f)
            {
                TryAttack();
                _timer = _cooldown;
            }
        }

        private void TryAttack()
        {
            // Запускаем анимацию атаки
            if (_behaviour.EnemyView.Animator != null)
            {
                _behaviour.EnemyView.Animator.SetTrigger("Attack");
            }

            // Стреляем снарядом
            _behaviour.PerformAttack();
        }

        public void Exit()
        {
            // Возвращаем управление NavMeshAgent
            _behaviour.MovementService.Resume(_behaviour.Agent);
        }
    }
}