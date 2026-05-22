namespace Presentation.AI
{
    public class IdleState : IEnemyState
    {
        private readonly EnemyBehaviour _behaviour;
        private readonly MeleeEnemyStateMachine _stateMachine;

        private float _wanderTimer;
        private float _wanderDelay = 3f;

        public IdleState(
            EnemyBehaviour behaviour,
            MeleeEnemyStateMachine stateMachine)
        {
            _behaviour = behaviour;
            _stateMachine = stateMachine;
        }

        public void Enter()
        {
            _wanderTimer = 0f;
        }

        public void Update()
        {
            if (_behaviour.ShouldChasePlayer())
            {
                _stateMachine.EnterChase(_behaviour);
                return;
            }

            if (_behaviour.ShouldFlee())
            {
                if (_behaviour.CombatEvaluator
                    .IsTargetDetected(
                        _behaviour.Self,
                        _behaviour.Player,
                        _behaviour.DetectionRadius))
                {
                    _stateMachine.EnterFlee(_behaviour);
                    return;
                }
            }

            _behaviour.UpdateIdle(
                ref _wanderTimer,
                _wanderDelay);
        }

        public void Exit() { }
    }
}