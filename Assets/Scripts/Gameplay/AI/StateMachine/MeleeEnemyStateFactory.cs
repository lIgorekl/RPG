namespace Presentation.AI
{
    public class MeleeEnemyStateFactory
    {
        public IdleState CreateIdle(
            EnemyBehaviour behaviour)
        {
            return new IdleState(behaviour);
        }

        public ChaseState CreateChase(
            EnemyBehaviour behaviour)
        {
            return new ChaseState(behaviour);
        }

        public AttackState CreateAttack(
            EnemyBehaviour behaviour)
        {
            return new AttackState(behaviour);
        }

        public FleeState CreateFlee(
            BaseCombatEnemyBehaviour behaviour)
        {
            return new FleeState(behaviour);
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