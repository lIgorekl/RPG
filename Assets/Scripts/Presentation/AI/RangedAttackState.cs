using UnityEngine;
using Core.Combat;
using Presentation.Player;
using Presentation.Combat;

namespace Presentation.AI
{
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

            if (_behaviour.Agent != null)
            {
                _behaviour.Agent.isStopped = true;
            }
        }

        public void Update()
        {
            if (_behaviour.EnemyView.IsStunned)
                return;
            
            float distance = Vector3.Distance(
                _behaviour.Self.position,
                _behaviour.Player.position);

            if (distance < _behaviour.MinDistance ||
                distance > _behaviour.MaxDistance)
            {
                _behaviour.StateMachine.ChangeState(
                    new MaintainDistanceState(_behaviour));
                return;
            }

            Vector3 direction =
                _behaviour.Player.position - _behaviour.Self.position;

            direction.y = 0f;

            if (direction.sqrMagnitude > 0.01f)
            {
                _behaviour.Self.rotation =
                    Quaternion.LookRotation(direction);
            }

            _timer -= Time.deltaTime;

            if (_timer <= 0f)
            {
                TryAttack();
                _timer = _cooldown;
            }
            Debug.Log("Ranged attacking");
        }

        private void TryAttack()
        {
            // запускаем анимацию стрельбы
            if (_behaviour.EnemyView.Animator != null)
            {
                _behaviour.EnemyView.Animator.SetTrigger("Attack");
            }

            var prefab = _behaviour.ProjectilePrefab;

            var projectile = Object.Instantiate(
                prefab,
                _behaviour.ProjectileSpawnPoint.position,
                Quaternion.identity
            );

            Vector3 direction =
                _behaviour.Player.position - _behaviour.ProjectileSpawnPoint.position;

            direction.y = 0f;
            direction.Normalize();

            projectile.transform.forward = direction;

            var entity = _behaviour.EnemyView.GetEntity();
            var damage = entity.GetMagicalDamage();

            projectile.Initialize(damage, _behaviour.Self);
        }

        public void Exit()
        {
            if (_behaviour.Agent != null)
            {
                _behaviour.Agent.isStopped = false;
            }
        }
    }
}