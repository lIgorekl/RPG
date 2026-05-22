namespace Presentation.AI
{
    public class BossStunState : IEnemyState
    {
        private readonly BossBehaviour _behaviour;

        private float _timer;

        public BossStunState(
            BossBehaviour behaviour,
            float duration)
        {
            _behaviour = behaviour;
            _timer = duration;
        }

        public void Enter()
        {
            _behaviour.MovementService.Stop(
                _behaviour.Agent);

            if (_behaviour.EnemyView.Animator != null)
            {
                _behaviour.EnemyView.Animator
                    .SetTrigger("Hurt");
            }
        }

        public void Update()
        {
            _behaviour.TickStun(
                ref _timer);
        }

        public void Exit()
        {
            _behaviour.MovementService.Resume(
                _behaviour.Agent);
        }
    }
}