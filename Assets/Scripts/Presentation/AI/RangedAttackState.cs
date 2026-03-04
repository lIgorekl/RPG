using UnityEngine;
using Core.Combat;
using Presentation.Player;

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
            _timer = 0f;
        }

        public void Update()
        {
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
            var entity = _behaviour.EnemyView.GetEntity();
            var damage = entity.GetMagicalDamage();

            var playerController = _behaviour.PlayerController;
            if (playerController != null)
            {
                playerController.GetEntity().ReceiveDamage(damage);
            }
        }

        public void Exit() { }
    }
}