using UnityEngine;

namespace Presentation.AI
{
    public class BossChaseState : IEnemyState
    {
        private readonly BossBehaviour _behaviour;

        public BossChaseState(BossBehaviour behaviour)
        {
            _behaviour = behaviour;
        }

        public void Enter() { }

        public void Update()
        {
            if (_behaviour.CombatEvaluator.IsTargetLost(
                _behaviour.Self,
                _behaviour.Player,
                _behaviour.DetectionRadius))
            {
                _behaviour.StateMachine.ChangeState(
                    _behaviour.StateFactory.CreateBossIdle(_behaviour));
                return;
            }

            if (_behaviour.CombatEvaluator.CanMeleeAttack(
                _behaviour.Self,
                _behaviour.Player,
                _behaviour.AttackRadius))
            {
                // 50% шанс сильной атаки
                if (Random.value > 0.5f)
                {
                    Debug.Log("HEAVY ATTACK");

                    _behaviour.StateMachine.ChangeState(
                        _behaviour.StateFactory.CreateBossHeavyAttack(_behaviour));
                }
                else
                {
                    Debug.Log("NORMAL ATTACK");

                    _behaviour.StateMachine.ChangeState(
                        _behaviour.StateFactory.CreateBossAttack(_behaviour));
                }
                return;
            }

            _behaviour.MovementService.RotateTo(
                _behaviour.Self,
                _behaviour.Player.position);

            _behaviour.MovementService.MoveTo(
                _behaviour.Agent,
                _behaviour.Player.position);
        }

        public void Exit() { }
    }
}