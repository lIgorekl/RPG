namespace Presentation.AI
{
    public class MaintainDistanceState : IEnemyState
    {
        private readonly RangedEnemyStateMachine _stateMachine;

        public MaintainDistanceState(
            RangedEnemyStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Enter() { }

        public void Update()
        {
            var behaviour =
                _stateMachine.RangedBehaviour;

            if (behaviour.ShouldFlee())
            {
                _stateMachine.EnterFlee();
                return;
            }

            if (behaviour.ShouldReturnToRangedIdle())
            {
                _stateMachine.EnterRangedIdle();
                return;
            }

            if (behaviour.ShouldStartRangedAttack())
            {
                _stateMachine.EnterRangedAttack();
                return;
            }

            behaviour.UpdateMaintainDistance();
        }

        public void Exit() { }
    }
}
