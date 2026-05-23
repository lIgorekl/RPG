namespace Presentation.AI
{
    public class ChaseState : IEnemyState
    {
        private readonly MeleeEnemyStateMachine _stateMachine;

        public ChaseState(
            MeleeEnemyStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter() { }

        public void Update()
        {
            var behaviour =
                _stateMachine.EnemyBehaviour;

            if (behaviour.ShouldFlee())
            {
                _stateMachine.EnterFlee();
                return;
            }

            if (behaviour.ShouldReturnToIdle())
            {
                _stateMachine.EnterIdle();
                return;
            }

            if (behaviour.ShouldAttackPlayer())
            {
                _stateMachine.EnterAttack();
                return;
            }

            behaviour.UpdateChase();
        }

        public void Exit() { }
    }
}
