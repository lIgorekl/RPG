namespace Presentation.AI
{
    public class StunState : IEnemyState
    {
        private readonly BaseCombatEnemyBehaviour _behaviour;

        private float _timer;

        public StunState(
            BaseCombatEnemyBehaviour behaviour,
            float duration)
        {
            _behaviour = behaviour;
            _timer = duration;
        }

        public void Enter()
        {
            _behaviour.EnterStun();
        }

        public void Update()
        {
            _behaviour.TickStun(
                ref _timer);
        }

        public void Exit()
        {
            _behaviour.ExitStun();
        }
    }
}