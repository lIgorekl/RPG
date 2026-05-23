namespace Presentation.AI
{
    public class BossStateMachine
        : EnemyStateMachineBase
    {
        private readonly BossStateFactory _factory;

        public BossBehaviour BossBehaviour =>
            (BossBehaviour)Behaviour;

        public BossStateMachine(
            BossBehaviour behaviour)
            : base(behaviour)
        {
            _factory = new BossStateFactory(this);
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

        public void EnterHeavyAttack()
        {
            ChangeState(_factory.CreateHeavyAttack());
        }

        public void EnterRecover()
        {
            ChangeState(_factory.CreateRecover());
        }

        public void EnterStun(float duration)
        {
            ChangeState(
                _factory.CreateStun(duration));
        }

        public void EnterEnrage()
        {
            ChangeState(_factory.CreateEnrage());
        }

        public override void EnterDefaultState()
        {
            EnterIdle();
        }
    }
}
