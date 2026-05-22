namespace Presentation.AI
{
    public class FleeState : IEnemyState
    {
        private readonly BaseCombatEnemyBehaviour _behaviour;

        private float _recalculateTimer;

        private const float FleeDistance = 10f;
        private const float RecalculateDelay = 1.5f;

        public FleeState(
            BaseCombatEnemyBehaviour behaviour)
        {
            _behaviour = behaviour;
        }

        public void Enter()
        {
            _behaviour.EnterFlee();

            _recalculateTimer = 0f;
        }

        public void Update()
        {
            _behaviour.TickFlee(
                ref _recalculateTimer,
                FleeDistance,
                RecalculateDelay);
        }

        public void Exit() { }
    }
}