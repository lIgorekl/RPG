using UnityEngine;

namespace Presentation.AI
{
    public class BossHeavyAttackState : IEnemyState
    {
        private readonly BossStateMachine _stateMachine;

        private float _timer;

        public BossHeavyAttackState(
            BossStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            _timer = 3f;

            var behaviour =
                _stateMachine.BossBehaviour;

            behaviour.MovementService.Stop(
                behaviour.Agent);
        }

        public void Update()
        {
            var behaviour =
                _stateMachine.BossBehaviour;

            if (!behaviour.ShouldAttack())
            {
                _stateMachine.EnterChase();
                return;
            }

            _timer -=
                Time.deltaTime *
                behaviour.AttackSpeedMultiplier;

            if (_timer <= 0f)
            {
                if (!behaviour.ShouldAttack())
                {
                    _stateMachine.EnterChase();
                    return;
                }

                behaviour.EnemyView.Attack(
                    behaviour.Player,
                    behaviour.HeavyAttackDamageMultiplier,
                    true);

                _stateMachine.EnterRecover();
            }
        }

        public void Exit()
        {
            var behaviour =
                _stateMachine.BossBehaviour;

            behaviour.MovementService.Resume(
                behaviour.Agent);
        }
    }
}
