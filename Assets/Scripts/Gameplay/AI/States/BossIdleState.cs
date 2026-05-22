namespace Presentation.AI
{
    public class BossIdleState : IEnemyState
    {
        private readonly BossBehaviour _behaviour;

        private float _wanderTimer;
        private float _wanderDelay = 3f;

        public BossIdleState(
            BossBehaviour behaviour)
        {
            _behaviour = behaviour;
        }

        public void Enter()
        {
            _wanderTimer = 0f;
        }

        public void Update()
        {
            _behaviour.TickIdle(
                ref _wanderTimer,
                _wanderDelay);
        }

        public void Exit() { }
    }
}