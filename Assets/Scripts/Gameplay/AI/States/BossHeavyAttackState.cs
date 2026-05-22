namespace Presentation.AI
{
    public class BossHeavyAttackState : IEnemyState
    {
        private readonly BossBehaviour _behaviour;

        private float _timer;

        public BossHeavyAttackState(
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
            _behaviour.TickHeavyAttack(
                ref _timer);
        }

        public void Exit()
        {
            _behaviour.MovementService.Resume(
                _behaviour.Agent);
        }
    }
}