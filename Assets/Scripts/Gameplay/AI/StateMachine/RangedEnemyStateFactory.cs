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

        public RangedIdleState CreateIdle(
            RangedEnemyBehaviour behaviour)
        {
            return new RangedIdleState(
                behaviour,
                _stateMachine);
        }

        public MaintainDistanceState CreateMaintainDistance(
            RangedEnemyBehaviour behaviour)
        {
            return new MaintainDistanceState(
                behaviour,
                _stateMachine);
        }

        public RangedAttackState CreateAttack(
            RangedEnemyBehaviour behaviour)
        {
            return new RangedAttackState(
                behaviour,
                _stateMachine);
        }

        public FleeState CreateFlee(
            BaseCombatEnemyBehaviour behaviour)
        {
            return new FleeState(behaviour);
        }

        public StunState CreateStun(
            BaseCombatEnemyBehaviour behaviour,
            float duration)
        {
            return new StunState(
                behaviour,
                duration);
        }
    }
}