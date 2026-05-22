namespace Presentation.AI
{
    public class BossStateMachine
        : EnemyStateMachineBase
    {
        private readonly EnemyStateFactory _factory;

        public BossStateMachine()
        {
            _factory = new EnemyStateFactory();
        }

        public override void EnterDefaultState(
            BaseEnemyBehaviour behaviour)
        {
            EnterBossIdle((BossBehaviour)behaviour);
        }

        public void EnterBossIdle(BossBehaviour behaviour)
        {
            ChangeState(
                _factory.CreateBossIdle(behaviour));
        }

        public void EnterBossChase(BossBehaviour behaviour)
        {
            ChangeState(
                _factory.CreateBossChase(behaviour));
        }

        public void EnterBossAttack(BossBehaviour behaviour)
        {
            ChangeState(
                _factory.CreateBossAttack(behaviour));
        }

        public void EnterBossHeavyAttack(BossBehaviour behaviour)
        {
            ChangeState(
                _factory.CreateBossHeavyAttack(behaviour));
        }

        public void EnterBossRecover(BossBehaviour behaviour)
        {
            ChangeState(
                _factory.CreateBossRecover(behaviour));
        }

        public void EnterBossStun(
            BossBehaviour behaviour,
            float duration)
        {
            ChangeState(
                _factory.CreateBossStun(
                    behaviour,
                    duration));
        }

        public void EnterBossEnrage(BossBehaviour behaviour)
        {
            ChangeState(
                _factory.CreateBossEnrage(behaviour));
        }
    }
}