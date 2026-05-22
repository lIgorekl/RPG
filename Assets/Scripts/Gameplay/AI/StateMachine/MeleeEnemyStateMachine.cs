namespace Presentation.AI
{
    public class MeleeEnemyStateMachine
        : EnemyStateMachineBase
    {
        private readonly MeleeEnemyStateFactory _factory;

        public MeleeEnemyStateMachine()
        {
            _factory = new MeleeEnemyStateFactory();
        }

        public void EnterIdle(
            EnemyBehaviour behaviour)
        {
            ChangeState(
                _factory.CreateIdle(behaviour));
        }

        public void EnterChase(
            EnemyBehaviour behaviour)
        {
            ChangeState(
                _factory.CreateChase(behaviour));
        }

        public void EnterAttack(
            EnemyBehaviour behaviour)
        {
            ChangeState(
                _factory.CreateAttack(behaviour));
        }

        public void EnterFlee(
            BaseCombatEnemyBehaviour behaviour)
        {
            ChangeState(
                _factory.CreateFlee(behaviour));
        }

        public void EnterStun(
            BaseCombatEnemyBehaviour behaviour,
            float duration)
        {
            ChangeState(
                _factory.CreateStun(
                    behaviour,
                    duration));
        }

        public override void EnterDefaultState(
            BaseEnemyBehaviour behaviour)
        {
            EnterIdle((EnemyBehaviour)behaviour);
        }
    }
}