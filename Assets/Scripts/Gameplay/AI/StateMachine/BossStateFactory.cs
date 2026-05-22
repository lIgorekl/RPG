namespace Presentation.AI
{
    public class BossStateFactory
    {
        public BossIdleState CreateIdle(
            BossBehaviour behaviour)
        {
            return new BossIdleState(behaviour);
        }

        public BossChaseState CreateChase(
            BossBehaviour behaviour)
        {
            return new BossChaseState(behaviour);
        }

        public BossAttackState CreateAttack(
            BossBehaviour behaviour)
        {
            return new BossAttackState(behaviour);
        }

        public BossHeavyAttackState CreateHeavyAttack(
            BossBehaviour behaviour)
        {
            return new BossHeavyAttackState(behaviour);
        }

        public BossRecoverState CreateRecover(
            BossBehaviour behaviour)
        {
            return new BossRecoverState(behaviour);
        }

        public BossStunState CreateStun(
            BossBehaviour behaviour,
            float duration)
        {
            return new BossStunState(
                behaviour,
                duration);
        }

        public BossEnrageState CreateEnrage(
            BossBehaviour behaviour)
        {
            return new BossEnrageState(behaviour);
        }
    }
}