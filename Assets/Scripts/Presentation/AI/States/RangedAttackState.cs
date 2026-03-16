using UnityEngine;

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
            if (_behaviour.Agent != null)
                _behaviour.Agent.isStopped = true;
        }

        public void Update()
        {
            // Если враг оглушён — ничего не делаем
            if (_behaviour.EnemyView.IsStunned)
                return;

            float distance = Vector3.Distance(
                _behaviour.Self.position,
                _behaviour.Player.position);

            // Если дистанция изменилась — возвращаемся к удержанию дистанции
            if (distance < _behaviour.MinDistance ||
                distance > _behaviour.MaxDistance)
            {
                _behaviour.StateMachine.ChangeState(
                    new MaintainDistanceState(_behaviour));
                return;
            }

            // Поворачиваемся к игроку
            Vector3 direction =
                _behaviour.Player.position - _behaviour.Self.position;

            direction.y = 0f;

            if (direction.sqrMagnitude > 0.01f)
            {
                _behaviour.Self.rotation =
                    Quaternion.LookRotation(direction);
            }

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
            _behaviour.ShootProjectile();
        }

        public void Exit()
        {
            // Возвращаем управление NavMeshAgent
            if (_behaviour.Agent != null)
                _behaviour.Agent.isStopped = false;
        }
    }
}