namespace Presentation.AI
{
    public class MeleeEnemyStateFactory
    {
        private readonly MeleeEnemyStateMachine _stateMachine;

        public MeleeEnemyStateFactory(
            MeleeEnemyStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public IdleState CreateIdle()
        {
            return new IdleState(_stateMachine);
        }

        public ChaseState CreateChase()
        {
            return new ChaseState(_stateMachine);
        }

        public AttackState CreateAttack()
        {
            return new AttackState(_stateMachine);
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
