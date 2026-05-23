namespace Presentation.AI
{
    public class MeleeEnemyStateMachine
        : EnemyStateMachineBase
    {
        private readonly MeleeEnemyStateFactory _factory;

        public EnemyBehaviour EnemyBehaviour =>
            (EnemyBehaviour)Behaviour;

        public MeleeEnemyStateMachine(
            EnemyBehaviour behaviour)
            : base(behaviour)
        {
            _factory = new MeleeEnemyStateFactory(this);
        }

        public void EnterIdle()
        {
            ChangeState(_factory.CreateIdle());
        }

        public void EnterChase()
        {
            ChangeState(_factory.CreateChase());
        }

        public void EnterAttack()
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
            EnterIdle();
        }
    }
}
