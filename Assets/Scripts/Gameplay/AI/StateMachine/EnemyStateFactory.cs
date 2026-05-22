namespace Presentation.AI
{
    public class EnemyStateFactory
    {
        public IdleState CreateIdle(EnemyBehaviour behaviour)
        {
            return new IdleState(behaviour);
        }

        public ChaseState CreateChase(EnemyBehaviour behaviour)
        {
            return new ChaseState(behaviour);
        }

        public AttackState CreateAttack(EnemyBehaviour behaviour)
        {
            return new AttackState(behaviour);
        }

        public FleeState CreateFlee(
            BaseCombatEnemyBehaviour behaviour)
        {
            return new FleeState(behaviour);
        }

        public RangedIdleState CreateRangedIdle(
            RangedEnemyBehaviour behaviour)
        {
            return new RangedIdleState(behaviour);
        }

        public MaintainDistanceState CreateMaintainDistance(
            RangedEnemyBehaviour behaviour)
        {
            return new MaintainDistanceState(behaviour);
        }

        public RangedAttackState CreateRangedAttack(
            RangedEnemyBehaviour behaviour)
        {
            return new RangedAttackState(behaviour);
        }

        public BossIdleState CreateBossIdle(
            BossBehaviour behaviour)
        {
            return new BossIdleState(behaviour);
        }

        public BossChaseState CreateBossChase(
            BossBehaviour behaviour)
        {
            return new BossChaseState(behaviour);
        }

        public BossAttackState CreateBossAttack(
            BossBehaviour behaviour)
        {
            return new BossAttackState(behaviour);
        }

        public BossHeavyAttackState CreateBossHeavyAttack(
            BossBehaviour behaviour)
        {
            return new BossHeavyAttackState(behaviour);
        }

        public BossRecoverState CreateBossRecover(
            BossBehaviour behaviour)
        {
            return new BossRecoverState(behaviour);
        }

        public BossStunState CreateBossStun(
            BossBehaviour behaviour,
            float duration)
        {
            return new BossStunState(behaviour, duration);
        }

        public BossEnrageState CreateBossEnrage(
            BossBehaviour behaviour)
        {
            return new BossEnrageState(behaviour);
        }

        public StunState CreateStun(
            BaseCombatEnemyBehaviour behaviour,
            float duration)
        {
            return new StunState(
                behaviour,
                duration);
        }
    }
}