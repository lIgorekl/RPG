namespace Presentation.AI
{
    public abstract class EnemyStateMachineBase
    {
        public BaseEnemyBehaviour Behaviour { get; }

        protected IEnemyState _currentState;

        public IEnemyState CurrentState => _currentState;

        protected EnemyStateMachineBase(
            BaseEnemyBehaviour behaviour)
        {
            Behaviour = behaviour;
        }

        protected void ChangeState(IEnemyState newState)
        {
            _currentState?.Exit();

            _currentState = newState;

            _currentState?.Enter();
        }

        public void Update()
        {
            _currentState?.Update();
        }

        public abstract void EnterDefaultState();
    }
}
