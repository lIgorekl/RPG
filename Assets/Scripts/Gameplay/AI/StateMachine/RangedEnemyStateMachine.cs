namespace Presentation.AI
{
    public class RangedEnemyStateMachine
        : EnemyStateMachineBase
    {
        private readonly RangedEnemyStateFactory _factory;

        public RangedEnemyBehaviour RangedBehaviour =>
            (RangedEnemyBehaviour)Behaviour;

        public RangedEnemyStateMachine(
            RangedEnemyBehaviour behaviour)
            : base(behaviour)
        {
            _factory = new RangedEnemyStateFactory(this);
        }

        public void EnterRangedIdle()
        {
            ChangeState(_factory.CreateIdle());
        }

        public void EnterMaintainDistance()
        {
            ChangeState(
                _factory.CreateMaintainDistance());
        }

        public void EnterRangedAttack()
        {
            ChangeState(_factory.CreateAttack());
        }

        public void EnterFlee()
        {
            ChangeState(_factory.CreateFlee());
        }

        public void EnterStun(float duration)
        {
            ChangeState(
                _factory.CreateStun(duration));
        }

        public override void EnterDefaultState()
        {
            EnterRangedIdle();
        }
    }
}
