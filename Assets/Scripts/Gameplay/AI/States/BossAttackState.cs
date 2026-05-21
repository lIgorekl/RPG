using UnityEngine;

namespace Presentation.AI
{
    public class BossAttackState : IEnemyState
    {
        private readonly BossBehaviour _behaviour;

        private float _cooldown = 1.5f;
        private float _timer;

        public BossAttackState(BossBehaviour behaviour)
        {
            _behaviour = behaviour;
        }

        public void Enter()
        {
            Debug.Log("ENTER NORMAL ATTACK");
            _timer = _cooldown;

            _behaviour.MovementService.Stop(_behaviour.Agent);
        }

        public void Update()
        {
            if (!_behaviour.CombatEvaluator.CanMeleeAttack(
                _behaviour.Self,
                _behaviour.Player,
                _behaviour.AttackRadius))
            {
                _behaviour.StateMachine.ChangeState(
                    _behaviour.StateFactory.CreateBossChase(_behaviour));
                return;
            }

            _timer -= Time.deltaTime * _behaviour.AttackSpeedMultiplier;

            if (_timer <= 0f)
            {
                _behaviour.EnemyView.Attack(_behaviour.Player, 1f);
                _timer = _cooldown;
            }
        }

        public void Exit()
        {
            _behaviour.MovementService.Resume(_behaviour.Agent);
        }
    }
}