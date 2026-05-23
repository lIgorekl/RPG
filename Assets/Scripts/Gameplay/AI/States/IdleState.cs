namespace Presentation.AI
{
    public class IdleState : IEnemyState
    {
        private readonly MeleeEnemyStateMachine _stateMachine;

        private float _wanderTimer;
        private float _wanderDelay = 3f;

        public IdleState(
            MeleeEnemyStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            _wanderTimer = 0f;
        }

        public void Update()
        {
            var behaviour =
                _stateMachine.EnemyBehaviour;

            if (behaviour.ShouldChasePlayer())
            {
                _stateMachine.EnterChase();
                return;
            }

            if (behaviour.ShouldFlee())
            {
                if (behaviour.CombatEvaluator
                    .IsTargetDetected(
                        behaviour.Self,
                        behaviour.Player,
                        behaviour.DetectionRadius))
                {
                    _stateMachine.EnterFlee();
                    return;
                }
            }

            behaviour.UpdateIdle(
                ref _wanderTimer,
                _wanderDelay);
        }

        public void Exit() { }
    }
}
