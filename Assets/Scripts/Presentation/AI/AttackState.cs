using UnityEngine;
using Core.Combat;

namespace Presentation.AI
{
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
            _timer = 0f;
        }

        public void Update()
        {
            float distance = Vector3.Distance(
                _behaviour.Self.position,
                _behaviour.Player.position);

            if (distance > _behaviour.AttackRadius)
            {
                _behaviour.StateMachine.ChangeState(
                    new ChaseState(_behaviour));
                return;
            }

            _timer -= Time.deltaTime;

            if (_timer <= 0f)
            {
                TryAttack();
                _timer = _attackCooldown;
            }
        }

        private void TryAttack()
        {
            var entity = _behaviour.EnemyView.GetEntity();
            var damage = entity.GetPhysicalDamage();

            var playerController =
                _behaviour.Player.GetComponent<Presentation.Player.PlayerController>();

            if (playerController != null)
            {
                playerController.GetEntity().ReceiveDamage(damage);
            }
        }

        public void Exit() { }
    }
}