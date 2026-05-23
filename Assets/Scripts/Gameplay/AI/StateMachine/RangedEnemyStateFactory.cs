namespace Presentation.AI
{
    public class RangedEnemyStateFactory
    {
        private readonly RangedEnemyStateMachine _stateMachine;

        public RangedEnemyStateFactory(
            RangedEnemyStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public RangedIdleState CreateIdle()
        {
            return new RangedIdleState(_stateMachine);
        }

        public MaintainDistanceState CreateMaintainDistance()
        {
            return new MaintainDistanceState(_stateMachine);
        }

        public RangedAttackState CreateAttack()
        {
            return new RangedAttackState(_stateMachine);
        }

        public FleeState CreateFlee()
        {
            return new FleeState(_stateMachine);
        }

        public StunState CreateStun(float duration)
        {
            return new StunState(
                _stateMachine,
                duration);
        }
    }
}
