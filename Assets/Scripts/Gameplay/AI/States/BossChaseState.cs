using UnityEngine;

namespace Presentation.AI
{
    public class BossChaseState : IEnemyState
    {
        private readonly BossStateMachine _stateMachine;

        public BossChaseState(
            BossStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter() { }

        public void Update()
        {
            var behaviour =
                _stateMachine.BossBehaviour;

            if (behaviour.ShouldReturnToIdle())
            {
                _stateMachine.EnterIdle();
                return;
            }

            if (behaviour.ShouldAttack())
            {
                if (Random.value > 0.5f)
                {
                    _stateMachine.EnterHeavyAttack();
                }
                else
                {
                    _stateMachine.EnterAttack();
                }

                return;
            }

            behaviour.MovementService.RotateTo(
                behaviour.Self,
                behaviour.Player.position);

            behaviour.MovementService.MoveTo(
                behaviour.Agent,
                behaviour.Player.position);
        }

        public void Exit() { }
    }
}
