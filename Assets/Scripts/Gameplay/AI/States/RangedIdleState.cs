namespace Presentation.AI
{
    public class RangedIdleState : IEnemyState
    {
        private readonly RangedEnemyStateMachine _stateMachine;

        private float _wanderTimer;
        private float _wanderDelay = 2f;

        public RangedIdleState(
            RangedEnemyStateMachine stateMachine)
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
                _stateMachine.RangedBehaviour;

            if (behaviour.ShouldMaintainDistance())
            {
                _stateMachine.EnterMaintainDistance();
                return;
            }

            if (behaviour.ShouldFlee())
            {
                if (behaviour.CombatEvaluator.IsTargetDetected(
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
