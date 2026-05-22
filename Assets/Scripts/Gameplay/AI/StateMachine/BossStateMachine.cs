namespace Presentation.AI
{
    public class BossStateMachine
        : EnemyStateMachineBase
    {
        private readonly BossStateFactory _factory;

        public BossStateMachine()
        {
            _factory = new BossStateFactory();
        }

        public override void EnterDefaultState(
            BaseEnemyBehaviour behaviour)
        {
            EnterBossIdle((BossBehaviour)behaviour);
        }

        public void EnterBossIdle(BossBehaviour behaviour)
        {
            ChangeState(
                _factory.CreateIdle(behaviour));
        }

        public void EnterBossChase(BossBehaviour behaviour)
        {
            ChangeState(
                _factory.CreateChase(behaviour));
        }

        public void EnterBossAttack(BossBehaviour behaviour)
        {
            ChangeState(
                _factory.CreateAttack(behaviour));
        }

        public void EnterBossHeavyAttack(BossBehaviour behaviour)
        {
            ChangeState(
                _factory.CreateHeavyAttack(behaviour));
        }

        public void EnterBossRecover(BossBehaviour behaviour)
        {
            ChangeState(
                _factory.CreateRecover(behaviour));
        }

        public void EnterBossStun(
            BossBehaviour behaviour,
            float duration)
        {
            ChangeState(
                _factory.CreateStun(
                    behaviour,
                    duration));
        }

        public void EnterBossEnrage(BossBehaviour behaviour)
        {
            ChangeState(
                _factory.CreateEnrage(behaviour));
        }
    }
}