namespace Presentation.AI
{
    public class ChaseState : IEnemyState
    {
        private readonly EnemyBehaviour _behaviour;
        private readonly MeleeEnemyStateMachine _stateMachine;

        public ChaseState(
            EnemyBehaviour behaviour,
            MeleeEnemyStateMachine stateMachine)
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

            if (_behaviour.ShouldReturnToIdle())
            {
                _stateMachine.EnterIdle(_behaviour);
                return;
            }

            if (_behaviour.ShouldAttackPlayer())
            {
                _stateMachine.EnterAttack(_behaviour);
                return;
            }

            _behaviour.UpdateChase();
        }

        public void Exit() { }
    }
}