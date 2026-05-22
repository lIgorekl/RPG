namespace Presentation.AI
{
    public class MaintainDistanceState : IEnemyState
    {
        private readonly RangedEnemyBehaviour _behaviour;
        private readonly RangedEnemyStateMachine _stateMachine;

        public MaintainDistanceState(
            RangedEnemyBehaviour behaviour,
            RangedEnemyStateMachine stateMachine)
        {
            _behaviour = behaviour;
            _stateMachine = stateMachine;
        }

        public void Enter() { }

        public void Update()
        {
            if (_behaviour.ShouldFlee())
            {
                _stateMachine.EnterFlee(_behaviour);
                return;
            }

            if (_behaviour.ShouldReturnToRangedIdle())
            {
                _stateMachine.EnterRangedIdle(_behaviour);
                return;
            }

            if (_behaviour.ShouldStartRangedAttack())
            {
                _stateMachine.EnterRangedAttack(_behaviour);
                return;
            }

            _behaviour.UpdateMaintainDistance();
        }

        public void Exit() { }
    }
}