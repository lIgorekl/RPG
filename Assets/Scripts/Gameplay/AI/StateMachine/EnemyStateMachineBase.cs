namespace Presentation.AI
{
    public abstract class EnemyStateMachineBase
    {
        protected IEnemyState _currentState;

        public IEnemyState CurrentState => _currentState;

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

        public abstract void EnterDefaultState(
            BaseEnemyBehaviour behaviour);
    }
}