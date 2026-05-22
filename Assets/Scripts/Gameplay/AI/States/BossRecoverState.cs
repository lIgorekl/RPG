namespace Presentation.AI
{
    public class BossRecoverState : IEnemyState
    {
        private readonly BossBehaviour _behaviour;

        private float _timer;

        public BossRecoverState(
            BossBehaviour behaviour)
        {
            _behaviour = behaviour;
        }

        public void Enter()
        {
            _timer = 3f;

            _behaviour.MovementService.Stop(
                _behaviour.Agent);
        }

        public void Update()
        {
            _behaviour.TickRecover(
                ref _timer);
        }

        public void Exit()
        {
            _behaviour.MovementService.Resume(
                _behaviour.Agent);
        }
    }
}