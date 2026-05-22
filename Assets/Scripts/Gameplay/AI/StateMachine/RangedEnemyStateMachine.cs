namespace Presentation.AI
{
    public class RangedEnemyStateMachine
        : EnemyStateMachineBase
    {
        private readonly RangedEnemyStateFactory _factory;

        public RangedEnemyStateMachine()
        {
            _factory = new RangedEnemyStateFactory(this);
        }

        public void EnterRangedIdle(
            RangedEnemyBehaviour behaviour)
        {
            ChangeState(
                _factory.CreateIdle(behaviour));
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
            EnterRangedIdle(
                (RangedEnemyBehaviour)behaviour);
        }
    }
}