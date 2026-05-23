namespace Presentation.AI
{
    public class BossStateFactory
    {
        private readonly BossStateMachine _stateMachine;

        public BossStateFactory(
            BossStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public BossIdleState CreateIdle()
        {
            return new BossIdleState(_stateMachine);
        }

        public BossChaseState CreateChase()
        {
            return new BossChaseState(_stateMachine);
        }

        public BossAttackState CreateAttack()
        {
            return new BossAttackState(_stateMachine);
        }

        public BossHeavyAttackState CreateHeavyAttack()
        {
            return new BossHeavyAttackState(_stateMachine);
        }

        public BossRecoverState CreateRecover()
        {
            return new BossRecoverState(_stateMachine);
        }

        public BossStunState CreateStun(float duration)
        {
            return new BossStunState(
                _stateMachine,
                duration);
        }

        public BossEnrageState CreateEnrage()
        {
            return new BossEnrageState(_stateMachine);
        }
    }
}
