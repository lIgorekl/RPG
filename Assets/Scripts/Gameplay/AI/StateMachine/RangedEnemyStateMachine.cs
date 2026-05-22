namespace Presentation.AI
{
    public class RangedEnemyStateMachine
        : EnemyStateMachineBase
    {
        private readonly EnemyStateFactory _factory;

        public RangedEnemyStateMachine()
        {
            _factory = new EnemyStateFactory();
        }

        public void EnterRangedIdle(
            RangedEnemyBehaviour behaviour)
        {
            ChangeState(
                _factory.CreateRangedIdle(behaviour));
        }

        public void EnterMaintainDistance(
            RangedEnemyBehaviour behaviour)
        {
            ChangeState(
                _factory.CreateMaintainDistance(behaviour));
        }

        public void EnterRangedAttack(
            RangedEnemyBehaviour behaviour)
        {
            ChangeState(
                _factory.CreateRangedAttack(behaviour));
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
            EnterRangedIdle(
                (RangedEnemyBehaviour)behaviour);
        }
    }
}